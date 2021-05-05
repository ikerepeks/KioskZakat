using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace KioskZakat.Controllers
{
    public class PrinterDLL
    {
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
