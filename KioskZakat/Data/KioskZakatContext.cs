using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using KioskZakat.Models;

namespace KioskZakat.Data
{
    public class KioskZakatContext : DbContext
    {
        public KioskZakatContext()
        {
        }

        public KioskZakatContext (DbContextOptions<KioskZakatContext> options)
            : base(options)
        {
        }

        public DbSet<KioskZakat.Models.Item> Item { get; set; }

        public DbSet<KioskZakat.Models.Purchase> Purchase { get; set; }

        public DbSet<KioskZakat.Models.Student> Student { get; set; }

        public DbSet<KioskZakat.Models.Checkout> Checkout { get; set; }
    }
}
