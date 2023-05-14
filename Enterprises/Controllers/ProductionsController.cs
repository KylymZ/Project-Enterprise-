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
    public class ProductionsController : Controller
    {


        public Production GetValuesProductionByID(short Id)
        {
            Production production = new Production();

            using (EnterprisesContext db = new EnterprisesContext())
            {
                SqlParameter ID = new("@ID", Id);
                var listproduction = db.Production.FromSqlRaw("SelectProduction @ID", ID).ToList();
                if (listproduction != null)
                {
                    foreach (var item in listproduction)
                    {
                        production = item;
                    }
                }
            }
            return production;

        }

        public List<Finproducts> GetValuesAllFinproducts()
        {
            List<Finproducts> finproducts = new List<Finproducts>();
            using (EnterprisesContext db = new EnterprisesContext())
            {
                finproducts = db.Finproducts.FromSqlRaw("SelectAllFinproducts").ToList();
            }
            return finproducts;
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
            List<Finproducts> finproducts = new List<Finproducts>();
            List<Employees> employees = new List<Employees>();
            List<Production> productions = new List<Production>();
            Finproducts finproductsNav = new Finproducts();
            Employees employeesNav = new Employees();
            List<Production> productionsData = new List<Production>();


            employees = await Task.Run(() => GetValuesAllEmployees());
            finproducts = await Task.Run(() => GetValuesAllFinproducts());

            using (EnterprisesContext db = new EnterprisesContext())
            {
                productions = await db.Production.FromSqlRaw("SelectAllProduction").ToListAsync();
            }

            if (productions != null && productions.Count != 0)
            {
                foreach (var fin in finproducts)
                {
                    foreach (var emp in employees)
                    {
                        foreach (var item in productions)
                        {
                            if (fin.Id == item.Product && emp.Id == item.Employee)
                            {
                                employeesNav = emp;
                                finproductsNav = fin;
                                productionsData.Add(new Production()
                                {

                                    Id = item.Id,
                                    Product = item.Product,
                                    CountProduction = item.CountProduction,
                                    Date = item.Date,
                                    Employee = item.Employee,
                                    ProductNavigation = finproductsNav,
                                    EmployeeNavigation = employeesNav,
                                });
                            }
                        }
                    }
                }
            }

            return View(productionsData);
        }

        public IActionResult Create()
        {
            var productlist = GetValuesAllFinproducts();
            var employeelist = GetValuesAllEmployees();
            ViewData["Employee"] = new SelectList(employeelist, "Id", "Fullname");
            ViewData["Product"] = new SelectList(productlist, "Id", "Name");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] Production production)
        {
            if (ModelState.IsValid)
            {
				SqlParameter count = new SqlParameter("@count", production.CountProduction);
				SqlParameter ID = new SqlParameter("@ID", production.Product);
				SqlParameter p = new SqlParameter("@p", SqlDbType.Int);
				p.Direction= ParameterDirection.Output;

				using (EnterprisesContext db = new EnterprisesContext())
				{
					var resultrow = await db.Database.ExecuteSqlRawAsync("SP_Productions @count,@ID,@p output", count, ID,p);

				}

                var k = Convert.ToInt32(p.Value);


                if (k == 0)
                {
					SqlParameter Product = new SqlParameter("@Product", production.Product);
					SqlParameter CountProduction = new SqlParameter("@CountProduction", production.CountProduction);
					SqlParameter Date = new SqlParameter("@Date", production.Date);
					SqlParameter Employee = new SqlParameter("@Employee", production.Employee);

					using (EnterprisesContext db = new EnterprisesContext())
					{
						var resultrow = await db.Database.ExecuteSqlRawAsync("InsertProduction @Product,@CountProduction,@Date,@Employee"
							, Product, CountProduction, Date, Employee);

					}
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewBag.Message = "Не хватает количества сырья!";
                }
            }
            var productlist =await Task.Run(()=>GetValuesAllFinproducts());
            var result =await Task.Run(()=>GetValuesProductionByID(production.Id));
            var employeeList =await Task.Run(()=>GetValuesAllEmployees());
            ViewData["Employee"] = new SelectList(employeeList, "Id", "Fullname", result.Employee);
            ViewData["Product"] = new SelectList(productlist, "Id", "Name", result.Product);
            return View(result);
        }

        public async Task<IActionResult> Edit(short id)
        {

			var productlist = await Task.Run(() => GetValuesAllFinproducts());
			var result = await Task.Run(() => GetValuesProductionByID(id));
			var employeeList = await Task.Run(() => GetValuesAllEmployees());
			if (result == null)
            {
                return NotFound();
            }
			ViewData["Employee"] = new SelectList(employeeList, "Id", "Fullname", result.Employee);
			ViewData["Product"] = new SelectList(productlist, "Id", "Name", result.Product);
			return View(result);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(short id, [FromForm] Production production)
        {
           
            if (ModelState.IsValid)
            {

				SqlParameter count = new SqlParameter("@count", production.CountProduction);
				SqlParameter ID = new SqlParameter("@ID", production.Product);
				SqlParameter p = new SqlParameter("@p", SqlDbType.Int);
				p.Direction = ParameterDirection.Output;

				using (EnterprisesContext db = new EnterprisesContext())
				{
					var resultrow = await db.Database.ExecuteSqlRawAsync("UpdateProduction2reset @count,@ID,@p output", count, ID, p);

				}

				var k = Convert.ToInt32(p.Value);
				
                if (k == 0)
                {

					SqlParameter IDPr = new SqlParameter("@ID", id);
					SqlParameter Product = new SqlParameter("@Product", production.Product);
					SqlParameter CountProduction = new SqlParameter("@CountProduction", production.CountProduction);
					SqlParameter Date = new SqlParameter("@Date", production.Date);
					SqlParameter Employee = new SqlParameter("@Employee", production.Employee);

					using (EnterprisesContext db = new EnterprisesContext())
					{
						var resultrow = await db.Database.ExecuteSqlRawAsync("UpdateProduction @ID,@Product,@CountProduction,@Date,@Employee"
							, IDPr, Product, CountProduction, Date, Employee);

					}
                    return RedirectToAction(nameof(Index));

                }
                else
                {
                    ViewBag.Message = "Не хватает количества сырья!";
                }

            }
			var productlist = await Task.Run(() => GetValuesAllFinproducts());
			var result = await Task.Run(() => GetValuesProductionByID(production.Id));
			var employeeList = await Task.Run(() => GetValuesAllEmployees());
			ViewData["Employee"] = new SelectList(employeeList, "Id", "Fullname", result.Employee);
			ViewData["Product"] = new SelectList(productlist, "Id", "Name", result.Product);
			return View(result);
        }

        public async Task<IActionResult> Delete(short id)
        {
			Finproducts finproducts = new Finproducts();
			Employees employees = new Employees();
			var resultProd = await Task.Run(() => GetValuesProductionByID(id));
			if (resultProd == null)
			{
				return NotFound();
			}

			using (EnterprisesContext db = new EnterprisesContext())
			{
				SqlParameter ID = new("@ID", resultProd.Product);
				var listfinproducts = await db.Finproducts.FromSqlRaw("SelectFinproducts @ID", ID).ToListAsync();
				if (listfinproducts != null)
				{
					foreach (var item in listfinproducts)
					{
						finproducts = item;
					}
				}
			}
			using (EnterprisesContext db = new EnterprisesContext())
			{
				SqlParameter ID = new("@ID", resultProd.Employee);
				var listemployees = await db.Employees.FromSqlRaw("SelectEmployee @ID", ID).ToListAsync();
				if (listemployees != null)
				{
					foreach (var item in listemployees)
					{
						employees = item;
					}
				}
			}

			resultProd.ProductNavigation = finproducts;
			resultProd.EmployeeNavigation = employees;
            return View(resultProd);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(short id)
        {

            using (EnterprisesContext db = new EnterprisesContext())
            {
                SqlParameter ID = new SqlParameter("@ID", id);
                var resultrow = await db.Database.ExecuteSqlRawAsync("DeleteProduction @ID", ID);
            }
            return RedirectToAction(nameof(Index));
        }

    }
}
