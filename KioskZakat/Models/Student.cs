using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KioskZakat.Models
{
    public class Student
    {   
        [Key]
        public string noMatric { get; set; }
        public string nama { get; set; }
        public string noBilik { get; set; }
        public string kodProgram { get; set; }
        public int semester { get; set; }
        public bool checkout { get; set; }
        public bool kunci { get; set; }
        public bool tag { get; set; }
        public DateTime checkoutTime { get; set; }

    }
}
