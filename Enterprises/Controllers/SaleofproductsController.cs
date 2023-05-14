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

namespace Enterprises.Controllers
{
    public class SaleofproductsController : Controller
    {
      
        public Saleofproducts GetValuesSaleofproductsByID(short Id)
        {
            Saleofproducts saleofproducts = new Saleofproducts();

            using (EnterprisesContext db = new EnterprisesContext())
            {
                SqlParameter ID = new("@ID", Id);
                var listsaleofproducts = db.Saleofproducts.FromSqlRaw("SelectSaleofproducts @ID", ID).ToList();
                if (listsaleofproducts != null)
                {
                    foreach (var item in listsaleofproducts)
                    {
                        saleofproducts = item;
                    }
                }
            }
            return saleofproducts;

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

            Finproducts finproductsNav = new Finproducts();
            Employees employeesNav = new Employees();
            List<Saleofproducts> saleofproducts = new List<Saleofproducts>();
            List<Saleofproducts> saleofproductsData = new List<Saleofproducts>();

            employees = await Task.Run(() => GetValuesAllEmployees());
            finproducts = await Task.Run(() => GetValuesAllFinproducts());

            using (EnterprisesContext db = new EnterprisesContext())
            {
                saleofproducts = await db.Saleofproducts.FromSqlRaw("SelectAllSaleofproducts").ToListAsync();
            }

            if (saleofproducts.Count != 0)
            {
                foreach (var emp in employees)
                {
                    foreach (var fin in finproducts)
                    {
                        foreach (var item in saleofproducts)
                        {
                            if(emp.Id==item.Employee && fin.Id == item.Product)
                            {
                                employeesNav = emp;
                                finproductsNav = fin;
                                saleofproductsData.Add(new Saleofproducts()
                                {
                                    Id = item.Id,
                                    Product = item.Product,
                                    CountSaleofpr = item.CountSaleofpr,
                                    Sum = item.Sum,
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
            return View(saleofproductsData);
        }

       
        public IActionResult Create()
        {
            var productlist = GetValuesAllFinproducts();
            var employeelist = GetValuesAllEmployees();
            ViewData["Product"] = new SelectList(productlist, "Id", "Name");
            ViewData["Employee"] = new SelectList(employeelist, "Id", "Fullname");
            return View();
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] Saleofproducts saleofproducts)
        {
            if (ModelState.IsValid)
            {
                SqlParameter count = new SqlParameter("@count", saleofproducts.CountSaleofpr);
                SqlParameter Product = new SqlParameter("@Product", saleofproducts.Product);
                SqlParameter p = new SqlParameter("@p", SqlDbType.Int);
                p.Direction = ParameterDirection.Output;

                using (EnterprisesContext db = new EnterprisesContext())
                {
                    var resultrow = await db.Database.ExecuteSqlRawAsync("SP_Product @count,@Product,@p output",
                        count, Product, p);

                }

                var k = Convert.ToInt32(p.Value);
              
                if (k == 0)
                {
                    SqlParameter ProductID = new SqlParameter("@Product", saleofproducts.Product);
                    SqlParameter CountSaleofpr = new SqlParameter("@CountSaleofpr", saleofproducts.CountSaleofpr);
                    SqlParameter Date = new SqlParameter("@Date", saleofproducts.Date);
                    SqlParameter Employee = new SqlParameter("@Employee", saleofproducts.Employee);

                    using (EnterprisesContext db = new EnterprisesContext())
                    {
                        var resultrow = await db.Database.ExecuteSqlRawAsync("InsertSaleofproducts @Product,@CountSaleofpr,@Date,@Employee"
                            , ProductID, CountSaleofpr, Date, Employee);

                    }
                    
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewBag.Message = "Не хватает количества готовых продуктов!";
                }
            }
            var productlist = await Task.Run(()=>GetValuesAllFinproducts());
            var result = await Task.Run(() => GetValuesSaleofproductsByID(saleofproducts.Id));
            var employeeList = await Task.Run(() => GetValuesAllEmployees());
            ViewData["Employee"] = new SelectList(employeeList, "Id", "Fullname", result.Employee);
            ViewData["Product"] = new SelectList(productlist, "Id", "Name", result.Product);
            return View(saleofproducts);
        }
        

        public async Task<IActionResult> Edit(short id)
        {

            var productlist = await Task.Run(() => GetValuesAllFinproducts());
            var result = await Task.Run(() => GetValuesSaleofproductsByID(id));
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
        public async Task<IActionResult> Edit(short id, [FromForm] Saleofproducts saleofproducts)
        {
           
            if (ModelState.IsValid)
            {
                SqlParameter ID = new SqlParameter("@ID", id);
                SqlParameter ProductID = new SqlParameter("@Product", saleofproducts.Product);
                SqlParameter CountSaleofpr = new SqlParameter("@CountSaleofpr", saleofproducts.CountSaleofpr);
                SqlParameter Date = new SqlParameter("@Date", saleofproducts.Date);
                SqlParameter Employee = new SqlParameter("@Employee", saleofproducts.Employee);

                using (EnterprisesContext db = new EnterprisesContext())
                {
                    var resultrow = await db.Database.ExecuteSqlRawAsync("UpdateSaleofproducts @ID,@Product,@CountSaleofpr,@Date,@Employee"
                        , ID,ProductID, CountSaleofpr, Date, Employee);

                }
              
                return RedirectToAction(nameof(Index));
            }
            var productlist = await Task.Run(() => GetValuesAllFinproducts());
            var result = await Task.Run(() => GetValuesSaleofproductsByID(saleofproducts.Id));
            var employeeList = await Task.Run(() => GetValuesAllEmployees());
            ViewData["Employee"] = new SelectList(employeeList, "Id", "Fullname", result.Employee);
            ViewData["Product"] = new SelectList(productlist, "Id", "Name", result.Product);
            return View(saleofproducts);
        }

        public async Task<IActionResult> Delete(short id)
        {

            Finproducts finproducts = new Finproducts();
            Employees employees = new Employees();

            var resultSale = await Task.Run(() => GetValuesSaleofproductsByID(id));
            if (resultSale == null)
            {
                return NotFound();
            }
            using (EnterprisesContext db = new EnterprisesContext())
            {
                SqlParameter ID = new("@ID", resultSale.Employee);
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
                SqlParameter ID = new("@ID", resultSale.Product);
                var listfinproducts = await db.Finproducts.FromSqlRaw("SelectFinproducts @ID", ID).ToListAsync();
                if (listfinproducts != null)
                {
                    foreach (var item in listfinproducts)
                    {
                        finproducts = item;
                    }
                }
            }


            resultSale.ProductNavigation = finproducts;
            resultSale.EmployeeNavigation = employees;
            return View(resultSale);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(short id)
        {
            using (EnterprisesContext db = new EnterprisesContext())
            {
                SqlParameter ID = new SqlParameter("@ID", id);
                var resultrow = await db.Database.ExecuteSqlRawAsync("DeleteSaleofproducts @ID", ID);
            }
       
            return RedirectToAction(nameof(Index));
        }
    }
}
