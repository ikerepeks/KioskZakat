using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KioskZakat.Data;
using KioskZakat.Models;
using System.IO;
using System.Data;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.Data.OleDb;
using System.Data.SqlClient;

namespace KioskZakat.Controllers
{
    public class StudentsController : Controller
    {
        private readonly KioskZakatContext _context;

        public StudentsController(KioskZakatContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ExcelUpload()
        {
            return View();
        }
        
        // List all of the student (old Index)
        public async Task<IActionResult> ListAll()
        {
            return View(await _context.Student.ToListAsync());
        }
    
        // GET: Students/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student
                .FirstOrDefaultAsync(m => m.noMatric == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("noMatric,nama,noBilik,kodProgram,semester,checkout,kunci,tag,checkoutTime")] Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return View("Index");
            }

            var student = await _context.Student.Where(x => x.noMatric == id).SingleAsync();
            if (student == null)
            {
                return View("Index");
            }
            return View(student);
        }

        // POST: Students/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("noMatric,nama,noBilik,kodProgram,semester,checkout,kunci,tag,checkoutTime")] Student student)
        {
            
            if (ModelState.IsValid)
            {
                try
                {

                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.noMatric))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return View("Index");
            }
            return View(student);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student
                .FirstOrDefaultAsync(m => m.noMatric == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var student = await _context.Student.FindAsync(id);
            _context.Student.Remove(student);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public FileResult DownloadExcel()
        {
            string Path = "Doc/Users.xlsx";
            return File(Path, "application/vnd.ms-excel", "Users.xlsx");
        }

        [HttpPost]
        public async Task<IActionResult> ImportExcelFile(IFormFile FormFile)
        {
            //get file name
            var filename = ContentDispositionHeaderValue.Parse(FormFile.ContentDisposition).FileName.Trim('"');

            //get path
            var MainPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads");

            //create directory "Uploads" if it doesn't exists
            if (!Directory.Exists(MainPath))
            {
                Directory.CreateDirectory(MainPath);
            }

            //get file path
            var filePath = Path.Combine(MainPath, FormFile.FileName);
            using (System.IO.Stream stream = new FileStream(filePath, FileMode.Create))
            {
                await FormFile.CopyToAsync(stream);
            }

            //get extension
            string extension = Path.GetExtension(filename);

            string conString = string.Empty;

            switch (extension)
            {
                case ".xls": //Excel 97-03
                    conString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties='Excel 8.0;HDR=YES'";
                    break;
                case ".xlsx": //Excel 07 and above
                    conString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties='Excel 8.0;HDR=YES'";
                    break;
            }

            DataTable dt = new DataTable();
            conString = string.Format(conString, filePath);

            using (OleDbConnection connExcel = new OleDbConnection(conString)){
                using (OleDbCommand cmdExcel = new OleDbCommand())
                {
                    using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                    {
                        cmdExcel.Connection = connExcel;

                        // Get the name of First Sheet
                        connExcel.Open();
                        DataTable dtExcelSchema;
                        dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                        string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                        connExcel.Close();

                        // Read Data from First Sheet
                        connExcel.Open();
                        cmdExcel.CommandText = "SELECT * From [" + sheetName + "]";
                        odaExcel.SelectCommand = cmdExcel;
                        odaExcel.Fill(dt);
                        connExcel.Close();
                    }
                }
            }

            // your database connection string
            conString = "Server=(localdb)\\mssqllocaldb;Database=KioskZakatContext-754d7dfc-50ad-4b72-ae40-911b90a0152f;Trusted_Connection=True;MultipleActiveResultSets=true";

            using (SqlConnection con = new SqlConnection(conString))
            {
                using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                {
                    // Set the database table name
                    sqlBulkCopy.DestinationTableName = "dbo.Student";

                    // Map Excel columns with database table

                    sqlBulkCopy.ColumnMappings.Add("NO PELAJAR", "noMatric");
                    sqlBulkCopy.ColumnMappings.Add("NAMA", "nama");
                    sqlBulkCopy.ColumnMappings.Add("NO RUMAH", "noBilik");
                    sqlBulkCopy.ColumnMappings.Add("KOD PROGRAM", "kodProgram");
                    sqlBulkCopy.ColumnMappings.Add("SEM", "semester");

                    con.Open();
                    sqlBulkCopy.WriteToServer(dt);
                    con.Close();
                }
            }
            // if code reach here, everything is okay
            ViewBag.Message = "File Imported and excel data saved into database";

            return View("ExcelUpload");
        }
        
        private bool StudentExists(string id)
        {
            return _context.Student.Any(e => e.noMatric == id);
        }
    }
}
