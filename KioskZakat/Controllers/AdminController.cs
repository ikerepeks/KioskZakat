using ESC_POS_USB_NET.Printer;
using Microsoft.AspNetCore.Mvc;
using PayECR;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace KioskZakat.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Fail()
        {
            return View();
        }


        //custom method section

        //manual settlement button
        public IActionResult Settlement()
        {
            ECR ecr;
            ecr = new ECR("COM1", 120000, 1000, "C:\\ECR_LOG", true, true);
            string ls_receive = "", ls_status = "";
            int li_status = 0;
            //use sendreceive method with c500 command with value 4 for payment method
            ecr.SendReceive("C50001", ref ls_receive, ref li_status, ref ls_status, 1000);

            //in case no com detected
            if (ls_receive.Equals(""))
            {
                return View("Fail");
            }

            return View("Index");
        }

        //checking validity admin for future development
        public IActionResult AdminValid(string pw)
        {
            if (pw.Equals("1234"))
            {
                return View("Index");
            }
            else
            {
                return RedirectToAction("Index","Home");
            }
            
        }

        public IActionResult TestPrint()
        {
            int m_iInit = -1;
            StringBuilder sPort = new StringBuilder("USB001");
            SetPrintport(sPort, 9600);
            m_iInit = SetInit();
            if (m_iInit == 0)
            {
                StringBuilder sbData = new StringBuilder("Transaction Number: 0000001");
                StringBuilder sbData1 = new StringBuilder("Servis: Infaq");
                StringBuilder sbData2 = new StringBuilder("Total Amount: RM 10.00");
                StringBuilder sbData3 = new StringBuilder("Terima Kasih kerana menggunakan kiosk Zakat");
                StringBuilder separator = new StringBuilder("-------------------------------------------");
                PrinterDLL.SetClean();
                PrinterDLL.PrintFeedline(5);
                PrinterDLL.SetAlignment(1);
                PrinterDLL.SetSizetext(1, 1);
                PrinterDLL.SetLinespace(60);
                PrinterDLL.PrintString(sbData);
                PrinterDLL.PrintString(separator);

                PrinterDLL.PrintFeedline(1);
                PrinterDLL.SetAlignment(0);
                PrinterDLL.PrintString(sbData1);
                PrinterDLL.SetAlignment(0);
                PrinterDLL.PrintString(sbData2);

                PrinterDLL.PrintFeedline(1);
                PrinterDLL.SetAlignment(1);
                PrinterDLL.PrintString(separator);

                PrinterDLL.PrintFeedline(5);
                PrinterDLL.PrintCutpaper(0);

                return View("Index");

            }
            else
            {
                return View("Fail");
            }
        }

        public IActionResult TestPrintQR()
        {
            int m_iInit = -1;
            StringBuilder sPort = new StringBuilder("USB001");
            SetPrintport(sPort, 9600);
            m_iInit = SetInit();
            if (m_iInit == 0)
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
                Printer printer = new Printer("MS-D347");

                StringBuilder sbData = new StringBuilder("This is your voucher");
                StringBuilder sbData1 = new StringBuilder(finalVoucher);
                StringBuilder sbData3 = new StringBuilder("Terima Kasih kerana menggunakan kiosk Zakat");
                StringBuilder separator = new StringBuilder("---------------------------");
                SetClean();
                PrintFeedline(5);
                SetAlignment(1);
                SetSizetext(1, 1);
                SetLinespace(60);
                PrintString(sbData);

                PrintFeedline(2);
                printer.Image(qrCodeImage);
                printer.PrintDocument();

                SetAlignment(0);
                PrintString(sbData1);

                SetAlignment(1);
                PrintString(sbData3);

                PrintFeedline(3);
                SetAlignment(1);
                PrintString(separator);

                PrintFeedline(5);
                PrintCutpaper(0);

                return View("Index");

            }
            else
            {
                return View("Fail");
            }
        }

        //Printer DLL Importer
        [DllImport("MsprintsdkRM.dll", EntryPoint = "SetInit", CharSet = CharSet.Ansi)]
        public static extern unsafe int SetInit();

        [DllImport("MsprintsdkRM.dll", EntryPoint = "SetClean", CharSet = CharSet.Ansi)]
        public static extern unsafe int SetClean();

        [DllImport("MsprintsdkRM.dll", EntryPoint = "SetAlignment", CharSet = CharSet.Ansi)]
        public static extern unsafe int SetAlignment(int iAlignment);

        [DllImport("MsprintsdkRM.dll", EntryPoint = "PrintString", CharSet = CharSet.Ansi)]
        public static extern unsafe int PrintString(StringBuilder strData);

        [DllImport("MsprintsdkRM.dll", EntryPoint = "PrintFeedline", CharSet = CharSet.Ansi)]
        public static extern unsafe int PrintFeedline(int iLine);

        [DllImport("MsprintsdkRM.dll", EntryPoint = "PrintCutpaper", CharSet = CharSet.Ansi)]
        public static extern unsafe int PrintCutpaper(int iMode);

        [DllImport("MsprintsdkRM.dll", EntryPoint = "SetPrintport", CharSet = CharSet.Ansi)]
        public static extern unsafe int SetPrintport(StringBuilder strPort, int iBaudrate);

        [DllImport("MsprintsdkRM.dll", EntryPoint = "SetSizetext", CharSet = CharSet.Ansi)]
        public static extern unsafe int SetSizetext(int iHeight, int iWidth);

        [DllImport("MsprintsdkRM.dll", EntryPoint = "SetLinespace", CharSet = CharSet.Ansi)]
        public static extern unsafe int SetLinespace(int iLinespace);

        [DllImport("MsprintsdkRM.dll", EntryPoint = "PrintDiskbmpfile", CharSet = CharSet.Ansi)]
        public static extern unsafe int PrintDiskbmpfile(StringBuilder strData);

    }
}
