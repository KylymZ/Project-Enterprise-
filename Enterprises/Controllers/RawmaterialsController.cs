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
    public class RawmaterialsController : Controller
    {
     
        public Rawmaterials GetValuesRawmaterialsByID(int Id)
        {
			Rawmaterials rawmaterials = new Rawmaterials();

			using (EnterprisesContext db = new EnterprisesContext())
			{
				SqlParameter ID = new("@ID", Id);
				var listrawmaterials = db.Rawmaterials.FromSqlRaw("SelectRawmaterials @ID", ID).ToList();
				if (listrawmaterials != null)
				{
					foreach (var item in listrawmaterials)
					{
						rawmaterials = item;
					}
				}
			}
            return rawmaterials;

        }

        public List<Units> GetValuesAllUnits()
        {
			List<Units> units = new List<Units>();
			using (EnterprisesContext db = new EnterprisesContext())
			{

				units = db.Units.FromSqlRaw("SelectAllUnits").ToList();
			}

			
            return units;
        }
		

		public async Task<IActionResult> Index()
        {
			List<Units> units = new List<Units>();
			List<Rawmaterials> rawmaterials = new List<Rawmaterials>();
			Units unitsNav = new Units();
			List<Rawmaterials> rawmaterialsData = new List<Rawmaterials>();

			units = await Task.Run(() => GetValuesAllUnits());

			using (EnterprisesContext db = new EnterprisesContext())
			{
				rawmaterials = await db.Rawmaterials.FromSqlRaw("SelectAllRawmaterials").ToListAsync();
			}

			
            if(rawmaterials!=null && rawmaterials.Count != 0)
            {
                foreach(var unit in units)
                {
                    foreach (var item in rawmaterials)
                    {
                        if (unit.Id == item.Unit)
                        {
							unitsNav = unit;
							rawmaterialsData.Add(new Rawmaterials()
							{
								Id = item.Id,
								Name = item.Name,
								Unit = item.Unit,
								Sum = item.Sum,
								CountRawm = item.CountRawm,
								UnitNavigation = unitsNav,
							});
						}
					}
                  
                }
            }
            return View(rawmaterialsData);
        }


        public IActionResult Create()
        {
            var unitlist = GetValuesAllUnits();
            ViewData["Unit"] = new SelectList(unitlist, "Id", "Name");
            return View();
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] Rawmaterials rawmaterials)
        {
            if (ModelState.IsValid)
            {
				SqlParameter Name = new SqlParameter("@Name", rawmaterials.Name);
				SqlParameter Unit = new SqlParameter("@Unit", rawmaterials.Unit);
				SqlParameter Sum = new SqlParameter("@Sum", rawmaterials.Sum);
				SqlParameter CountRawm = new SqlParameter("@CountRawm", rawmaterials.CountRawm);

				using (EnterprisesContext db = new EnterprisesContext())
				{
					var resultrow = await db.Database.ExecuteSqlRawAsync("InsertRawmaterials @Name,@Unit,@Sum,@CountRawm", 
                        Name, Unit, Sum, CountRawm);

				}
                return RedirectToAction(nameof(Index));
            }
            var unitslist = await Task.Run(()=>GetValuesAllUnits());
            var result = await Task.Run(() => GetValuesRawmaterialsByID(rawmaterials.Id));
            ViewData["Unit"] = new SelectList(unitslist, "Id", "Name", result.Unit);
            return View(result);
        }

        public async Task<IActionResult> Edit(short id)
        {
			var unitslist = await Task.Run(() => GetValuesAllUnits());
			var resultRaw = await Task.Run(() => GetValuesRawmaterialsByID(id));
			if (resultRaw == null)
            {
                return NotFound();
            }
            ViewData["Unit"] = new SelectList(unitslist, "Id", "Name", resultRaw.Unit);
            return View(resultRaw);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(short id, [FromForm] Rawmaterials rawmaterials)
        {
            if (ModelState.IsValid)
            {
				SqlParameter ID = new SqlParameter("@ID", id);
				SqlParameter Name = new SqlParameter("@Name", rawmaterials.Name);
				SqlParameter Unit = new SqlParameter("@Unit", rawmaterials.Unit);
				SqlParameter Sum = new SqlParameter("@Sum", rawmaterials.Sum);
				SqlParameter CountRawm = new SqlParameter("@CountRawm", rawmaterials.CountRawm);

				using (EnterprisesContext db = new EnterprisesContext())
				{
					var resultrow = await db.Database.ExecuteSqlRawAsync("UpdateRawmaterials @ID,@Name,@Unit,@Sum,@CountRawm",
						ID,Name, Unit, Sum, CountRawm);

				}
                return RedirectToAction(nameof(Index));
            }
			var unitslist = await Task.Run(() => GetValuesAllUnits());
			var result = await Task.Run(() => GetValuesRawmaterialsByID(rawmaterials.Id));
			ViewData["Unit"] = new SelectList(unitslist, "Id", "Name", result.Unit);
			return View(result);
        }

        // GET: Rawmaterials/Delete/5
        public async Task<IActionResult> Delete(short id)
        {
			var resultRaw= GetValuesRawmaterialsByID(id);
			if (resultRaw == null)
			{
				return NotFound();
			}
			Units units = new Units();

			using (EnterprisesContext db = new EnterprisesContext())
			{
				SqlParameter ID = new("@ID", resultRaw.Unit);
				var listUnit = await db.Units.FromSqlRaw("SelectUnits @ID", ID).ToListAsync();
				if (listUnit != null)
				{
					foreach (var item in listUnit)
					{
						units = item;
					}
				}
			}

			resultRaw.UnitNavigation = units;
            return View(resultRaw);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(short id)
        {
			using (EnterprisesContext db = new EnterprisesContext())
			{
				SqlParameter ID = new SqlParameter("@ID", id);
				var resultrow = await db.Database.ExecuteSqlRawAsync("DeleteRawmaterials @ID", ID);

			}
            return RedirectToAction(nameof(Index));
        }

    }
}
