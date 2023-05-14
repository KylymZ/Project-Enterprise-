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
    public class PositionsController : Controller
    {
        public Positions GetValuesPositionsByID(int Id)
        {
            Positions positions = new Positions();

            using (EnterprisesContext db = new EnterprisesContext())
            {
                SqlParameter ID = new("@ID", Id);
                var listpositions = db.Positions.FromSqlRaw("SelectPosition @ID", ID).ToList();
                if (listpositions != null)
                {
                    foreach (var item in listpositions)
                    {
                        positions = item;
                    }
                }
            }
            return positions;

        }

        public async Task<IActionResult> Index()
        {
			List<Positions> positions = new List<Positions>();

			using (EnterprisesContext db = new EnterprisesContext())
			{
				positions = await db.Positions.FromSqlRaw("SelectAllPosition").ToListAsync();
			}
			
            return View(positions);
        }


        public IActionResult Create()
        {
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] Positions positions)
        {
            if (ModelState.IsValid)
            {
				SqlParameter Name = new SqlParameter("@Name", positions.Position);
				
				using (EnterprisesContext db = new EnterprisesContext())
				{
					var resultrow = await db.Database.ExecuteSqlRawAsync("InsertPositions @Name", Name);

				}
				
                return RedirectToAction(nameof(Index));
            }
            var result = await Task.Run(()=>GetValuesPositionsByID(positions.Id));
            return View(result);
        }

        
        public async Task<IActionResult> Edit(short id)
        {

			var result = await Task.Run(() => GetValuesPositionsByID(id));
			if (result == null)
            {
                return NotFound();
            }
            return View(result);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(short id, [FromForm] Positions positions)
        {
            if (ModelState.IsValid)
            {
				SqlParameter ID = new SqlParameter("@ID", id);
				SqlParameter Name = new SqlParameter("@Name", positions.Position);

				using (EnterprisesContext db = new EnterprisesContext())
				{
					var resultrow = await db.Database.ExecuteSqlRawAsync("UpdatePositions @ID,@Name", ID, Name);

				}
				
                return RedirectToAction(nameof(Index));
            }
			var result = await Task.Run(() => GetValuesPositionsByID(id));
			return View(result);
        }

        
        public async Task<IActionResult> Delete(short id)
        {
			var result = await Task.Run(() => GetValuesPositionsByID(id));

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
				var resultrow = await db.Database.ExecuteSqlRawAsync("DeletePositions @ID", ID);

			}
			
            return RedirectToAction(nameof(Index));
        }

    }
}
