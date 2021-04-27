using Microsoft.AspNetCore.Mvc;
using PayECR;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
