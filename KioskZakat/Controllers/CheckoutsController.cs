using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KioskZakat.Data;
using KioskZakat.Models;
using System.Diagnostics;

namespace KioskZakat.Controllers
{
    public class CheckoutsController : Controller
    {
        private readonly KioskZakatContext _context;

        public CheckoutsController(KioskZakatContext context)
        {
            _context = context;
        }

        // GET: Checkouts
        public async Task<IActionResult> Index()
        {
            return View(await _context.Checkout.ToListAsync());
        }

        // GET: Checkouts/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var checkout = await _context.Checkout
                .FirstOrDefaultAsync(m => m.noMatric == id);
            if (checkout == null)
            {
                return NotFound();
            }

            return View(checkout);
        }

        // POST: Checkouts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var checkout = await _context.Checkout.FindAsync(id);
            _context.Checkout.Remove(checkout);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CheckoutExists(string id)
        {
            return _context.Checkout.Any(e => e.noMatric == id);
        }
        
        //save to checkout db, data taken from student db after aggreing
        [HttpGet]
        public async Task<IActionResult> SavetoDB(string matric, string nama, string noBilik, string kodProgram, string semester, bool key, bool tag)
        {
            DateTime now = DateTime.Now;
            Debug.WriteLine(now);
            Checkout data = new Checkout(matric, nama, noBilik, kodProgram, semester, key, tag, now);
            _context.Add(data);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Students", null);

        }
    }
}
