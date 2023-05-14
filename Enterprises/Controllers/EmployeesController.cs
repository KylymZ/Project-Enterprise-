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
    public class EmployeesController : Controller
    {
        public Employees GetValuesEmployeesByID(int Id)
        {
            Employees employees = new Employees();

            using (EnterprisesContext db = new EnterprisesContext())
            {
                SqlParameter ID = new("@ID", Id);
                var listEmployee = db.Employees.FromSqlRaw("SelectEmployee @ID", ID).ToList();
                if (listEmployee != null)
                {
                    foreach (var item in listEmployee)
                    {
                        employees = item;
                    }
                }
            }
            return employees;
        }

        public List<Positions> GetValuesAllPositions()
        {
            List<Positions> positions = new List<Positions>();
			using (EnterprisesContext db = new EnterprisesContext())
			{
				positions = db.Positions.FromSqlRaw("SelectAllPosition").ToList();
			}
            return positions;
        }

       
        public async Task<IActionResult> Index()
        {
			List<Positions> positions = new List<Positions>();
			List<Employees> employees = new List<Employees>();
            Positions positionsNav = new Positions();
            List<Employees> employeesData = new List<Employees>();
            positions = await Task.Run(() => GetValuesAllPositions());


			using (EnterprisesContext db = new EnterprisesContext())
			{
				employees = await db.Employees.FromSqlRaw("SelectAllEmployees").ToListAsync();
			}

            if (employees != null)
            {
                foreach(var pos in positions)
                {
                    foreach (var emp in employees)
                    {
                        if (pos.Id == emp.Position)
                        {
							positionsNav=pos;

                            employeesData.Add(new Employees()
                            {
								Id = emp.Id,
								Fullname = emp.Fullname,
								Position = emp.Position,
								Salary = emp.Salary,
								Address = emp.Address,
								Telephone = emp.Telephone,
								PositionNavigation = positionsNav,
							});
						}
                    }
                }
            }
            return View(employeesData);
        }

        public IActionResult Create()
        {
			var positions= GetValuesAllPositions();

			ViewData["Position"] = new SelectList(positions, "Id", "Position");
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] Employees employees)
        {
            if (ModelState.IsValid)
            {
				SqlParameter Fullname = new SqlParameter("@Fullname", employees.Fullname);
				SqlParameter Position = new SqlParameter("@Position", employees.Position);
				SqlParameter Salary = new SqlParameter("@Salary", employees.Salary);
				SqlParameter Address = new SqlParameter("@Address", employees.Address);
				SqlParameter Telephone = new SqlParameter("@Telephone", employees.Telephone);
				using (EnterprisesContext db = new EnterprisesContext())
				{
					var resultrow = await db.Database.ExecuteSqlRawAsync("InsertEmployees @Fullname,@Position" +
						",@Salary,@Address,@Telephone", Fullname, Position, Salary, Address, Telephone);

				}
                return RedirectToAction(nameof(Index));
            }

            var posintionslist = await Task.Run(()=> GetValuesAllPositions());
            var result = await Task.Run(()=>GetValuesEmployeesByID(employees.Id));

            ViewData["Position"] = new SelectList(posintionslist, "Id", "Position", result.Position);
            return View(result);
        }

       
        public async Task<IActionResult> Edit(int id)
        {
           
            var posintionslist = await Task.Run(()=> GetValuesAllPositions());
            var result = await Task.Run(() => GetValuesEmployeesByID(id));

            if (result == null || posintionslist==null)
            {
                return NotFound();
            }
            ViewData["Position"] = new SelectList(posintionslist, "Id", "Position", result.Position);
            return View(result);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromForm] Employees employees)
        {
            if (ModelState.IsValid)
            {
				SqlParameter ID = new SqlParameter("@ID", id);
				SqlParameter Fullname = new SqlParameter("@Fullname", employees.Fullname);
				SqlParameter Position = new SqlParameter("@Position", employees.Position);
				SqlParameter Salary = new SqlParameter("@Salary", employees.Salary);
				SqlParameter Address = new SqlParameter("@Address", employees.Address);
				SqlParameter Telephone = new SqlParameter("@Telephone", employees.Telephone);
				using (EnterprisesContext db = new EnterprisesContext())
				{
					var resultrow = await db.Database.ExecuteSqlRawAsync("UpdateEmployees @ID,@Fullname,@Position,@Salary,@Address,@Telephone"
                        , ID,Fullname, Position, Salary, Address,Telephone);

				}
                return RedirectToAction(nameof(Index));
            }
			var posintionslist = await Task.Run(() => GetValuesAllPositions());
			var result = await Task.Run(() => GetValuesEmployeesByID(employees.Id));
			ViewData["Position"] = new SelectList(posintionslist, "Id", "Position", result.Position);
			return View(result);
        }

     
        public async Task<IActionResult> Delete(int id)
        {
            var resultEmp = GetValuesEmployeesByID(id);
			if (resultEmp == null)
			{
				return NotFound();
			}
			Positions positions = new Positions();

			using (EnterprisesContext db = new EnterprisesContext())
			{
				SqlParameter ID = new("@ID", resultEmp.Position);
				var listPos = await db.Positions.FromSqlRaw("SelectPosition @ID", ID).ToListAsync();
				if (listPos != null)
				{
					foreach (var item in listPos)
					{
						positions = item;
					}
				}
			}
			resultEmp.PositionNavigation = positions;
            return View(resultEmp);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
			SqlParameter ID = new SqlParameter("@ID", id);

			using (EnterprisesContext db = new EnterprisesContext())
			{
				var resultrow = await db.Database.ExecuteSqlRawAsync("DeleteEmployees @ID", ID);

			}
            return RedirectToAction(nameof(Index));
        }

    }
}
