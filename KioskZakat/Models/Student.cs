using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KioskZakat.Models
{
    public class Student
    {   
        [Key]
        public int Id { get; set; }
        public string noMatric { get; set; }
        public string nama { get; set; }
        public string noBilik { get; set; }
        public string kodProgram { get; set; }
        public string semester { get; set; }
    }
}
