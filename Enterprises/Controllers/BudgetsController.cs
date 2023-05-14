using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Enterprises;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;


namespace Enterprises.Controllers
{

    public class BudgetsController : Controller
    {
        public Budgets GetValuesBudgetsbyID(int Id)
        {
            Budgets budgets= new Budgets();

			using (EnterprisesContext db= new EnterprisesContext())
            {
                SqlParameter ID = new("@ID", Id);
                var listbudgets = db.Budgets.FromSqlRaw("SelectBudget @ID", ID).ToList();
                if(listbudgets != null)
                {
                    foreach (var item in listbudgets)
                    {
                        budgets = item;
					}
                }
			}
            return budgets;
        }


       
        public async Task<IActionResult> Index()
        {
            List<Budgets> budgets = new List<Budgets>();

			using (EnterprisesContext db = new EnterprisesContext())
            {
				budgets = await db.Budgets.FromSqlRaw("SelectAllBudgets").ToListAsync();
            }
			return View(budgets);
		}



        public IActionResult Create()
        {
            return View();
        }

    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] Budgets budgets)
        {

            if (ModelState.IsValid)
            {
                SqlParameter Summ = new SqlParameter("@Summa", budgets.Budgetamount);
                SqlParameter SalePercentage = new SqlParameter("@SalePerc", budgets.SalePercentage);
                SqlParameter Bonus = new SqlParameter("@Bon", budgets.Bonus);

                using(EnterprisesContext db= new EnterprisesContext())
                {
                    var resultrow = await db.Database.ExecuteSqlRawAsync("InsertBudgets @Summa,@SalePerc,@Bon", Summ
                        , SalePercentage, Bonus);

				}
                return RedirectToAction(nameof(Index));
            }
            else
            {
                var result = Task.Run(() => GetValuesBudgetsbyID(budgets.Id));
                return View(result);
            }
        }

        
        public async Task<IActionResult> Edit(int id)
        {

            var result = await Task.Run(()=> GetValuesBudgetsbyID(id));
            return View(result);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromForm] Budgets budgets)
        {

            if (ModelState.IsValid)
            {
				SqlParameter ID = new SqlParameter("@ID", id);
				SqlParameter Summ = new SqlParameter("@Summa", budgets.Budgetamount);
				SqlParameter SalePercentage = new SqlParameter("@SalePerc", budgets.SalePercentage);
				SqlParameter Bonus = new SqlParameter("@Bon", budgets.Bonus);

				using (EnterprisesContext db = new EnterprisesContext())
				{
					var resultrow = await db.Database.ExecuteSqlRawAsync("UpdateBudgets @ID,@Summa,@SalePerc,@Bon", 
                        ID,Summ,SalePercentage, Bonus);

				}
				return RedirectToAction(nameof(Index));
			}
            else
            {
                var result = await Task.Run(()=> GetValuesBudgetsbyID(id));
                return View(result);
            }

        }

        public async Task<IActionResult> Delete(int id)
        {

            var result =await Task.Run(()=> GetValuesBudgetsbyID(id));
            return View(result);

        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
           			SqlParameter ID = new SqlParameter("@ID", id);
			
			using (EnterprisesContext db = new EnterprisesContext())
			{
                var resultrow = await db.Database.ExecuteSqlRawAsync("DeleteBudgeds @ID", ID);

			}
            return RedirectToAction(nameof(Index));
        }
    }
}
