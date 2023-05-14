using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Enterprises;
using Microsoft.Data.SqlClient;

namespace Enterprises.Controllers
{
    public class UnitsController : Controller
    {
        public Units GetValuesUnitsByID(int Id)
        {
            Units units = new Units();

            using (EnterprisesContext db = new EnterprisesContext())
            {
                SqlParameter ID = new("@ID", Id);
                var listunits = db.Units.FromSqlRaw("SelectUnits @ID", ID).ToList();
                if (listunits != null)
                {
                    foreach (var item in listunits)
                    {
                        units = item;
                    }
                }
            }
         
            return units;

        }

        public async Task<IActionResult> Index()
        {
            List<Units> units = new List<Units>();

            using (EnterprisesContext db = new EnterprisesContext())
            {
                units = await db.Units.FromSqlRaw("SelectAllUnits").ToListAsync();
            }

           
            return View(units);
        }

        
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] Units units)
        {
            if (ModelState.IsValid)
            {
                SqlParameter Name = new SqlParameter("@Name", units.Name);

                using (EnterprisesContext db = new EnterprisesContext())
                {
                    var resultrow = await db.Database.ExecuteSqlRawAsync("InsertUnits @Name", Name);

                }
              
                return RedirectToAction(nameof(Index));
            }
            
            return View(units);
        }

        public async Task<IActionResult> Edit(short id)
        {
          
            var result = await Task.Run(() => GetValuesUnitsByID(id));
            if (result == null)
            {
                return NotFound();
            }
            return View(result);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(short id, [FromForm] Units units)
        {
            if (id != units.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                SqlParameter ID = new SqlParameter("@ID", id);
                SqlParameter Name = new SqlParameter("@Name", units.Name);

                using (EnterprisesContext db = new EnterprisesContext())
                {
                    var resultrow = await db.Database.ExecuteSqlRawAsync("UpdateUnits @ID,@Name", ID,Name);

                }
               
                return RedirectToAction(nameof(Index));
            }
            var result =await Task.Run(()=>GetValuesUnitsByID(id));
            return View(result);
        }

        public async Task<IActionResult> Delete(short id)
        {
            var result = await Task.Run(() => GetValuesUnitsByID(id));
            if (result == null)
            {
                return NotFound();
            }

            return View(result);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(short id)
        {
            SqlParameter ID = new SqlParameter("@ID", id);

            using (EnterprisesContext db = new EnterprisesContext())
            {
                var resultrow = await db.Database.ExecuteSqlRawAsync("DeleteUnits @ID", ID);

            }
           
            return RedirectToAction(nameof(Index));
        }

    }
}
