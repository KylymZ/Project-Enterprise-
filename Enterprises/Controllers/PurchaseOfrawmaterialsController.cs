using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Enterprises;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Enterprises.Controllers
{
    public class PurchaseOfrawmaterialsController : Controller
    {

        public PurchaseOfrawmaterials GetValuesPurchaseOfrawmaterialsByID(short Id)
        {
			PurchaseOfrawmaterials purchaseOfrawmaterials = new PurchaseOfrawmaterials();

			using (EnterprisesContext db = new EnterprisesContext())
			{
				SqlParameter ID = new("@ID", Id);
				var listpurchrawmaterials = db.PurchaseOfrawmaterials.FromSqlRaw("SelectPurchaseOfrawmaterials @ID", ID).ToList();
				if (listpurchrawmaterials != null)
				{
					foreach (var item in listpurchrawmaterials)
					{
						purchaseOfrawmaterials = item;
					}
				}
			}
			
            return purchaseOfrawmaterials;

        }

		public List<Rawmaterials> GetValuesAllRawmaterials()
		{
			List<Rawmaterials> rawmaterials = new List<Rawmaterials>();
			using (EnterprisesContext db = new EnterprisesContext())
			{

				rawmaterials = db.Rawmaterials.FromSqlRaw("SelectAllRawmaterials").ToList();
			}

			return rawmaterials;
		}

		public List<Employees> GetValuesAllEmployees()
		{
			List<Employees> employees = new List<Employees>();
			using (EnterprisesContext db = new EnterprisesContext())
			{

				employees = db.Employees.FromSqlRaw("SelectAllEmployees").ToList();
			}

			return employees;
		}


		public async Task<IActionResult> Index() 
        {
			List<Rawmaterials> rawmaterials = new List<Rawmaterials>();
			List<Employees> employees = new List<Employees>();
			List<PurchaseOfrawmaterials> purchaseOfrawmaterials = new List<PurchaseOfrawmaterials>();

			Rawmaterials rawmaterialsNav = new Rawmaterials();
			Employees employeesNav = new Employees();
			List<PurchaseOfrawmaterials> purchaseOfrawmaterialsData = new List<PurchaseOfrawmaterials>();


			employees = await Task.Run(() => GetValuesAllEmployees());
			rawmaterials = await Task.Run(() => GetValuesAllRawmaterials());

			using (EnterprisesContext db = new EnterprisesContext())
			{
				purchaseOfrawmaterials = await db.PurchaseOfrawmaterials.FromSqlRaw("SelectAllPurchaseOfrawmaterials").ToListAsync();
			}

            if(purchaseOfrawmaterials!=null && purchaseOfrawmaterials.Count != 0)
            {
                foreach (var emp in employees)
                {
                    foreach(var raw in rawmaterials)
                    {
                        foreach (var item in purchaseOfrawmaterials)
                        {
                            if(emp.Id==item.Employee && raw.Id == item.RawMaterials)
                            {
								employeesNav = emp;
								rawmaterialsNav = raw;

								purchaseOfrawmaterialsData.Add(new PurchaseOfrawmaterials() 
								{
									Id = item.Id,
									RawMaterials = item.RawMaterials,
									CountPur = item.CountPur,
									Sum = item.Sum,
									Date = item.Date,
									Employee = item.Employee,
									RawMaterialsNavigation = rawmaterialsNav,
									EmployeeNavigation = employeesNav,
								});
							}
                        }
                    }
                }
            }

            return View(purchaseOfrawmaterialsData);
        }


        public IActionResult Create()
        {
            var employeelist = GetValuesAllEmployees();
            var rawmateriallist = GetValuesAllRawmaterials();
            ViewData["Employee"] = new SelectList(employeelist, "Id", "Fullname");
            ViewData["RawMaterials"] = new SelectList(rawmateriallist, "Id", "Name");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] PurchaseOfrawmaterials purchaseOfrawmaterials)
        {
			if (ModelState.IsValid)
			{

				SqlParameter sum = new SqlParameter("@sum", purchaseOfrawmaterials.Sum);
				SqlParameter p = new SqlParameter("@p", SqlDbType.Int);
				p.Direction = ParameterDirection.Output;

				using (EnterprisesContext db = new EnterprisesContext())
				{
					var resultrow = await db.Database.ExecuteSqlRawAsync("SP_Zakupka @sum,@p output", sum, p);

				}

				var k = Convert.ToInt32(p.Value);
				
                if (k == 0)  
                {
					SqlParameter RawMaterials = new SqlParameter("@RawMaterials", purchaseOfrawmaterials.RawMaterials);
					SqlParameter CountPur = new SqlParameter("@CountPur", purchaseOfrawmaterials.CountPur);
					SqlParameter Sum = new SqlParameter("@Sum", purchaseOfrawmaterials.Sum);
					SqlParameter Date = new SqlParameter("@Date", purchaseOfrawmaterials.Date);

					SqlParameter Employee = new SqlParameter("@Employee", purchaseOfrawmaterials.Employee);

					using (EnterprisesContext db = new EnterprisesContext())
					{
						var resultrow = await db.Database.ExecuteSqlRawAsync("InsertPurchaseOfrawmaterials @RawMaterials,@CountPur,@Sum,@Date,@Employee"
							, RawMaterials, CountPur, Sum,Date, Employee);

					}
					
                    return RedirectToAction(nameof(Index)); 
                }
                else 
                {
                    ViewBag.Massage = "Не хватает бюджета для закупки!";
                }
            }

            var employeelist = await Task.Run(()=>GetValuesAllEmployees());
            var rawmaterialslist = await Task.Run(() => GetValuesAllRawmaterials());
            var result = await Task.Run(() => GetValuesPurchaseOfrawmaterialsByID(purchaseOfrawmaterials.Id));
            ViewData["Employee"] = new SelectList(employeelist, "Id", "Fullname", purchaseOfrawmaterials.Employee);
            ViewData["RawMaterials"] = new SelectList(rawmaterialslist, "Id", "Name", purchaseOfrawmaterials.RawMaterials);
            return View(result);
        }


        public async Task<IActionResult> Edit(short id)
        {

			var employeelist = await Task.Run(() => GetValuesAllEmployees());
			var rawmaterialslist = await Task.Run(() => GetValuesAllRawmaterials());
			var resultPur = await Task.Run(() => GetValuesPurchaseOfrawmaterialsByID(id));

			if (resultPur == null)
            {
                return NotFound();
            }
            ViewData["Employee"] = new SelectList(employeelist, "Id", "Fullname");
            ViewData["RawMaterials"] = new SelectList(rawmaterialslist, "Id", "Name");
            return View(resultPur);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(short id, [FromForm] PurchaseOfrawmaterials purchaseOfrawmaterials)
        {
            if (ModelState.IsValid)
            {
				SqlParameter ID = new SqlParameter("@ID", id);
				SqlParameter RawMaterials = new SqlParameter("@RawMaterials", purchaseOfrawmaterials.RawMaterials);
				SqlParameter CountPur = new SqlParameter("@CountPur", purchaseOfrawmaterials.CountPur);
				SqlParameter Sum = new SqlParameter("@Sum", purchaseOfrawmaterials.Sum);
				SqlParameter Date = new SqlParameter("@Date", purchaseOfrawmaterials.Date);

				SqlParameter Employee = new SqlParameter("@Employee", purchaseOfrawmaterials.Employee);

				using (EnterprisesContext db = new EnterprisesContext())
				{
					var resultrow = await db.Database.ExecuteSqlRawAsync("UpdatePurchaseOfrawmaterials @ID,@RawMaterials,@CountPur,@Sum,@Date,@Employee"
						, ID,RawMaterials, CountPur, Sum, Date, Employee);

				}
				
                return RedirectToAction(nameof(Index));
            }
			var employeelist = await Task.Run(() => GetValuesAllEmployees());
			var rawmaterialslist = await Task.Run(() => GetValuesAllRawmaterials());
			var result = await Task.Run(() => GetValuesPurchaseOfrawmaterialsByID(purchaseOfrawmaterials.Id));
			ViewData["Employee"] = new SelectList(employeelist, "Id", "Fullname", purchaseOfrawmaterials.Employee);
			ViewData["RawMaterials"] = new SelectList(rawmaterialslist, "Id", "Name", purchaseOfrawmaterials.RawMaterials);
			return View(result);
        }

        public async Task<IActionResult> Delete(short id)
        {
			Rawmaterials rawmaterials = new Rawmaterials();
			Employees employees = new Employees();
			
			var resultPur = await Task.Run(() => GetValuesPurchaseOfrawmaterialsByID(id));
			if (resultPur == null)
            {
                return NotFound();
            }
			using (EnterprisesContext db = new EnterprisesContext())
			{
				SqlParameter ID = new("@ID", resultPur.Employee);
				var listemployees = await db.Employees.FromSqlRaw("SelectEmployee @ID", ID).ToListAsync();
				if (listemployees != null)
				{
					foreach (var item in listemployees)
					{
						employees = item;
					}
				}
			}
			using (EnterprisesContext db = new EnterprisesContext())
			{
				SqlParameter ID = new("@ID", resultPur.RawMaterials);
				var listrawmaterials = await db.Rawmaterials.FromSqlRaw("SelectRawmaterials @ID", ID).ToListAsync();
				if (listrawmaterials != null)
				{
					foreach (var item in listrawmaterials)
					{
						rawmaterials = item;
					}
				}
			}
			resultPur.EmployeeNavigation = employees;
			resultPur.RawMaterialsNavigation = rawmaterials;
            return View(resultPur);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(short id)
        {
			using (EnterprisesContext db = new EnterprisesContext())
			{
				SqlParameter ID = new SqlParameter("@ID", id);
				var resultrow = await db.Database.ExecuteSqlRawAsync("DeletePurchaseOfrawmaterials @ID", ID);
			}
			
            return RedirectToAction(nameof(Index));
        }
    }
}
