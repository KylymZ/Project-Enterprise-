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
    public class FinproductsController : Controller
    {
        public Finproducts GetValuesFinproductsByID(int Id)
        {
			Finproducts finproducts = new Finproducts();

			using (EnterprisesContext db = new EnterprisesContext())
			{
				SqlParameter ID = new("@ID", Id);
				var listFinproducts = db.Finproducts.FromSqlRaw("SelectFinproducts @ID", ID).ToList();
				if (listFinproducts != null)
				{
					foreach (var item in listFinproducts)
					{
						finproducts = item;
					}
				}
			}
            return finproducts;

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
			List<Finproducts> finproducts = new List<Finproducts>();
			Units unitsNav = new Units();
			List<Finproducts> finproductsData = new List<Finproducts>();

			units = await Task.Run(() => GetValuesAllUnits());
			using (EnterprisesContext db = new EnterprisesContext())
			{
				finproducts = await db.Finproducts.FromSqlRaw("SelectAllFinproducts").ToListAsync();
			}
			if (finproducts != null)
			{
				foreach (var unit in units)
				{
					foreach (var fin in finproducts)
					{
						if (unit.Id == fin.Unit)
						{
							unitsNav = unit;

							finproductsData.Add(new Finproducts()
							{
								Id = fin.Id,
								Name = fin.Name,
								Unit = fin.Unit,
								Sum = fin.Sum,
								Countproducts = fin.Countproducts,
								UnitNavigation = unitsNav,
							});
						}
					}
				}
			}
            return View(finproductsData);
        }

        public IActionResult Create()
        {
            var unit = GetValuesAllUnits();
            ViewData["Unit"] = new SelectList(unit, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] Finproducts finproducts)
        {
            if (ModelState.IsValid)
            {
				SqlParameter Name = new SqlParameter("@Name", finproducts.Name);
				SqlParameter Unit = new SqlParameter("@Unit", finproducts.Unit);
				SqlParameter Sum = new SqlParameter("@Sum", finproducts.Sum);
				SqlParameter Countproducts = new SqlParameter("@Countproducts", finproducts.Countproducts);
				using (EnterprisesContext db = new EnterprisesContext())
				{
					var resultrow = await db.Database.ExecuteSqlRawAsync("InsertFinproducts @Name,@Unit" +
						",@Sum,@Countproducts", Name, Unit, Sum, Countproducts);

				}

                return RedirectToAction(nameof(Index));
            }
            var unitslist = await Task.Run(()=>GetValuesAllUnits());
            var result = await Task.Run(()=>GetValuesFinproductsByID(finproducts.Id));

            ViewData["Unit"] = new SelectList(unitslist, "Id", "Name", result.Unit);
            return View(finproducts);
        }

       
        public async Task<IActionResult> Edit(short id)
        {
            
            var unitslist = await Task.Run(()=>GetValuesAllUnits());
            var result = await Task.Run(()=>GetValuesFinproductsByID(id));

            if (result == null || unitslist==null)
            {
                return NotFound();
            }
            ViewData["Unit"] = new SelectList(unitslist, "Id", "Name", result.Unit);
            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(short id, [FromForm] Finproducts finproducts)
        {
            if (ModelState.IsValid)
            {
				SqlParameter ID = new SqlParameter("@ID", id);
				SqlParameter Name = new SqlParameter("@Name", finproducts.Name);
				SqlParameter Unit = new SqlParameter("@Unit", finproducts.Unit);
				SqlParameter Sum = new SqlParameter("@Sum", finproducts.Sum);
				SqlParameter Countproducts = new SqlParameter("@Countproducts", finproducts.Countproducts);
				using (EnterprisesContext db = new EnterprisesContext())
				{
					var resultrow = await db.Database.ExecuteSqlRawAsync("UpdateFinproducts @ID,@Name,@Unit" +
						",@Sum,@Countproducts", ID,Name, Unit, Sum, Countproducts);

				}

                return RedirectToAction(nameof(Index));
            }
			var unitslist = await Task.Run(() => GetValuesAllUnits());
			var result = await Task.Run(() => GetValuesFinproductsByID(finproducts.Id));

			ViewData["Unit"] = new SelectList(unitslist, "Id", "Name", result.Unit);
			return View(result);
        }

        public async Task<IActionResult> Delete(short id)
        {
			var resultFin = GetValuesFinproductsByID(id);
			if (resultFin == null)
			{
				return NotFound();
			}
			Units units = new Units();

			using (EnterprisesContext db = new EnterprisesContext())
			{
				SqlParameter ID = new("@ID", resultFin.Unit);
				var listUnits = await db.Units.FromSqlRaw("SelectUnits @ID", ID).ToListAsync();
				if (listUnits != null)
				{
					foreach (var item in listUnits)
					{
						units = item;
					}
				}
			}

			resultFin.UnitNavigation = units;
            return View(resultFin);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(short id)
        {
			using (EnterprisesContext db = new EnterprisesContext())
			{
				SqlParameter ID = new SqlParameter("@ID", id);
				var resultrow = await db.Database.ExecuteSqlRawAsync("DeleteFinproducts @ID", ID);
			}
			
            return RedirectToAction(nameof(Index));
        }  
    }
}
