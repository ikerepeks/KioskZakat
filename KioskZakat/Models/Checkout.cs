using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KioskZakat.Models
{
    public class Checkout
    {
        
        [Key]
        public string noMatric { get; set; }
        public string nama { get; set; }
        public string noBilik { get; set; }
        public string kodProgram { get; set; }
        public string semester { get; set; }
        public DateTime checkout { get; set; }
        public bool kunci { get; set; }
        public bool tag { get; set; }

        public Checkout(string matric, string nama, string bilik, string program, string semester, bool key, bool tag, DateTime now)
        {
            this.noMatric = matric;
            this.nama = nama;
            this.noBilik = bilik;
            this.kodProgram = program;
            this.semester = semester;
            this.kunci = key;
            this.tag = tag;
            this.checkout = now;
        }

        public Checkout()
        { }

    }
}
