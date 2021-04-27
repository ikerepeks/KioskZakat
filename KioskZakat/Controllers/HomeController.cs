using KioskZakat.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ESC_POS_USB_NET.Printer;
using PayECR;
using System.Drawing;
using KioskZakat.Data;
using System.Runtime.InteropServices;
using QRCoder;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Drawing.Imaging;
using System.Net.Mime;
using Twilio.AspNet.Mvc;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace KioskZakat.Controllers
{
    public class HomeController : Controller
    {
        public static int i = 0;
        private readonly KioskZakatContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, KioskZakatContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Gerobok()
        {
            return View();
        }

        public IActionResult Infaq()
        {
            return View();
        }

        public IActionResult AdsVideo()
        {
            return View();
        }

        public IActionResult CetakBaucar()
        {
            return View();
        }

        public IActionResult Success()
        {
            return View();
        }

        public IActionResult Email()
        {
            return View();
        }

        public IActionResult Failed()
        {
            return View();
        }

        //Custom Method Section

        //send enquiry to IM20
        [HttpGet]
        public async Task<IActionResult> SendENQAsync(string amount, [Optional] string ic, int type, int itemId)
        {
            string price = "";

            //scuffed get itemid
            switch (amount){
                case "1":
                    itemId = 1006;
                    break;
                case "5":
                    itemId = 1007;
                    break;
                case "10":
                    itemId = 1008;
                    break;
                case "15":
                    itemId = 1009;
                    break;
                case "30":
                    itemId = 1010;
                    break;
                case "50":
                    itemId = 1011;
                    break;

            }

            if (amount.Equals(null))
            {
                return View("PrintTest");
            }

        
            else
            {
                price = amount; //initiating price to assign to receipt
                
                //padding for message code
                amount = amount + "00";
                amount = amount.PadLeft(12, '0');
                
                //initiate gateway object using com1
                ECR ecr;
                ecr = new ECR("COM1", 120000, 1000, "C:\\ECR_LOG", true, true);
                string ls_receive = "", ls_status = "";
                int li_status = 0;
                //use sendreceive method with c200 command with value 4 for payment method
                ecr.SendReceive("C200004"+amount, ref ls_receive, ref li_status, ref ls_status, 1000);

                
                //in case no com detected
                if (ls_receive.Equals("")) {
                    ViewBag.code = "null";
                    ViewBag.status = li_status;
                    return View("Failed");

                }

                string checker = TransacCheck(ls_receive, 1);

                //keep status,receive and code for error display
                ViewBag.code = checker;
                ViewBag.receive = ls_receive;
                ViewBag.status = ls_status;

                //check if transac is succesfull or not
                if (checker.Equals("00"))
                {
                    string TransacTrace = TransacCheck(ls_receive, 2);
                    await SaveToDatabaseAsync(ic, TransacTrace, itemId, type);
                    PrintReceipt(TransacTrace,price,type);
                    sendWhatsApp(type);
                    Settlement();

                    return View("Success");
                }
                else
                {
                    return View("Failed");
                }
            }
        }

        //print receipt after successful transac
        public IActionResult PrintReceipt(String transacNum, string amount, int type)
        {
            //determining type of service to be printed
            string item = "";
            if (type == 1) { item = "Infaq"; }
            else if (type == 2) { item = "Gerobok"; }

            Printer printer = new Printer("MS-D347 (Copy 2)");
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            printer.AlignCenter();
            printer.Append("Transaction Number:" + transacNum);
            printer.Separator();
            printer.AlignCenter();
            Bitmap image = new Bitmap(Bitmap.FromFile("wwwroot/Image/uitm.bmp"));
            printer.Image(image);
            printer.Separator();
            printer.Append("Servis: " + item);
            printer.NewLines(2);
            printer.Append("Total Amount: RM " + amount + ".00");
            printer.Separator();
            printer.Append("Terima Kasih kerana menggunakan kiosk Zakat");
            printer.Separator();
            printer.FullPaperCut();
            printer.PrintDocument();
            
            return View("Success");
        }

        //print receipt with QR code contain voucher code
        public IActionResult PrintVoucherAsync()
        {
            //grab voucher number generated after transac
            /*List<string> voucher = _context.Purchase.Select(u => u.couponNum).ToList();
            var random = new Random();
            int index = random.Next(voucher.Count);
            string finalVoucher = voucher[index];
            */

            //Demo purpose data
            var voucher = new List<string> { "UITMZAKAT001", "UITMZAKAT002"};
            var random = new Random();
            int index = random.Next(voucher.Count);
            string finalVoucher = voucher[index];

            //generate QR from voucher
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(finalVoucher, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);

            //similar printer execution with printreceipt
            Printer printer = new Printer("MS-D347 (Copy 2)");
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            printer.Separator();
            printer.AlignCenter();
            printer.Image(qrCodeImage);
            printer.Separator();
            printer.AlignCenter();
            printer.Append(finalVoucher);
            printer.NewLines(2);
            printer.Separator();
            printer.Append("Terima Kasih kerana menggunakan kiosk Zakat");
            printer.Separator();
            printer.FullPaperCut();
            printer.PrintDocument();
            
            return View("Success");
        }

        //check for status code for reply message to check trasaction status
        public string TransacCheck(string ReceivedMessege, int type)
        {
            string ResponseMsg = "",
                CardNo = "", ExpiryDate = "", StatusCode = "", ApprovalCode = "", RRN = "", TransactionTrace = "",
                BatchNumber = "", HostNo = "", TID = "", MID = "", AID = "", TC = "", CardholderName = "", CardType = "";

            ResponseMsg = ReceivedMessege.Substring(0, 4);
            string response = ResponseMsg;

            //for card insert or wave
            if (ResponseMsg.Equals("R200"))
            {
                ResponseMsg = "SALE";
                CardNo = ReceivedMessege.Substring(4, 19);
                ExpiryDate = ReceivedMessege.Substring(23, 4);
                StatusCode = ReceivedMessege.Substring(27, 2);
                ApprovalCode = ReceivedMessege.Substring(29, 6);
                RRN = ReceivedMessege.Substring(35, 12);
                TransactionTrace = ReceivedMessege.Substring(47, 6);
                BatchNumber = ReceivedMessege.Substring(53, 6);
                HostNo = ReceivedMessege.Substring(59, 2);
                TID = ReceivedMessege.Substring(61, 8);
                MID = ReceivedMessege.Substring(69, 15);
                AID = ReceivedMessege.Substring(84, 14);
                TC = ReceivedMessege.Substring(98, 16);
                CardholderName = ReceivedMessege.Substring(114, 26);

                CardType = ReceivedMessege.Substring(140, 2);
            }

            // for wallet pay
            if (ResponseMsg.Equals("G200"))
            {
                ResponseMsg = "SALE";
                StatusCode = ReceivedMessege.Substring(4, 2);
            }

            //for saving to database function
            if (type == 1)
            {
                return StatusCode;
            }
            else
            {
                return TransactionTrace;
            }

        }

        //saving to database function after success transac
        public async Task SaveToDatabaseAsync([Optional] string ic, string transacTrace, int itemId, int type)
        {
            // coupon number creation format : {PREFIX}(Unique ID) || UITMZAKAT(DATETIME.NOW)+ TYPE OF SERVICE
            string couponTemplate = "UITMZAKAT";
            string couponFinal = couponTemplate;

            switch (type)
            {
                case 1:
                    couponFinal = couponTemplate + DateTime.Now.ToString() + "INFAQ";
                    break;
                case 2:
                    couponFinal = couponTemplate + DateTime.Now.ToString() + "GEROBOK";
                    break;
            }

            //add 3 month to the coupon validity
            DateTime validUntil = DateTime.Now.AddDays(90);

            //model creation to be stored
            Purchase purchase = new Purchase() { 
                icNum = ic,
                couponNum = couponFinal,
                validUntil = validUntil,
                transacTrace = transacTrace, 
                itemID = itemId };

            _context.Add(purchase);
            await _context.SaveChangesAsync();
        }

        // Settlement done for every transaction
        public void Settlement()
        {
            ECR ecr;
            ecr = new ECR("COM1", 120000, 1000, "C:\\ECR_LOG", true, true);
            string ls_receive = "", ls_status = "";
            int li_status = 0;
            //use sendreceive method with c500 command with value 4 for payment method
            ecr.SendReceive("C50001", ref ls_receive, ref li_status, ref ls_status, 1000);
        }

        // Send QRCode through WhatsApp
        public void sendWhatsApp(int type)
        {
            string uri = "";
            string body = "";
            if (type == 1)
            {
                body = "Here is the Infaq QR Code";
                if (i == 0)
                {
                    uri = "https://i.ibb.co/7G7Rg3F/zakat1.jpg";
                    i++;
                }
                else if (i == 1)
                {
                    uri = "https://i.ibb.co/GvBcd2Q/zakat2.jpg";
                    i = 0;
                }
            }
            else if (type == 2)
            {
                body = "Here is the Gerobok Rezeki QR Code";
                uri = "https://i.ibb.co/mbmNWFg/gerobok.png";
            }

            // Twilio WhatsApp API
            TwilioClient.Init("AC1e34b2f5fe71ca33a62e676b87533b8b", "360c508840cd86cabf4d3c8ba82de62d");

            var message = MessageResource.Create(
                body: body,
                from: new Twilio.Types.PhoneNumber("whatsapp:+14155238886"),
                to: new Twilio.Types.PhoneNumber("whatsapp:+60105172014"),
                mediaUrl: new List<Uri> {new Uri(uri) }
                );

            Console.WriteLine(message.Sid);
        }

        // Send QRCode through Email
        public void sendEmail()
        {
            //Demo purpose data
            var voucher = new List<string> { "UITMZAKAT001", "UITMZAKAT002" };
            var random = new Random();
            int index = random.Next(voucher.Count);
            string finalVoucher = voucher[index];

            //generate QR from voucher
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(finalVoucher, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);
            
            //Convert QR Image to Byte
            ImageConverter ic = new ImageConverter();
            Byte[] ba = (Byte[])ic.ConvertTo(qrCodeImage, typeof(Byte[]));
            
            //Store Image ine Memory Stream
            MemoryStream qr = new MemoryStream(ba);
            ContentType contentType = new ContentType();
            contentType.MediaType = MediaTypeNames.Image.Jpeg;
            contentType.Name = "screen.jpg";
            
            // Construct email body
            string to = "shakir_1998@yahoo.com.my";
            string from = "infaqtester@gmail.com";
            MailMessage message = new MailMessage(from, to);
            string mailbody = "Here is the QR code from Infaq kiosk";
            message.Subject = "QR Code for Infaq";
            message.Body = mailbody;
            message.Attachments.Add(new Attachment(qr, contentType));
            message.BodyEncoding = Encoding.UTF8;
            message.IsBodyHtml = true;
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587); //Gmail smtp    
            System.Net.NetworkCredential basicCredential1 = new
            System.Net.NetworkCredential("infaqtester@gmail.com", "InfaqTesterAdmin");
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = basicCredential1;
            try
            {
                client.Send(message);
            }

            catch (Exception ex)
            {
                throw ex;
            }

            qr.Flush();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
