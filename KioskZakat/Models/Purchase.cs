using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace KioskZakat.Models
{
    public class Purchase
    {
        public int Id { get; set; }
        public string icNum { get; set; }
        public string couponNum { get; set; }
        public DateTime validUntil { get; set; }
        public string transacTrace { get; set; }
        [ForeignKey("Item")]
        public int itemID { get; set; }
        public Item Item { get; set; }

        public Purchase()
        {

        }
    }
}
