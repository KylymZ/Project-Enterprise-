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
    public class SalaryEmpsController : Controller
    {
        string M = "";
        float allcountOfSalary = 0;

        public List<SalaryEmp> GetValuesSalaryEmpByYearAndMonth(int Year, int Month)
        {
            List<Employees> employees = new List<Employees>();
            List<Months> months = new List<Months>();
            List<Years> years = new List<Years>();
            List<SalaryEmp> salaryEmps = new List<SalaryEmp>();
            List<SalaryEmp> salariesData = new List<SalaryEmp>();

            Employees employeesNav = new Employees();
            Years yearsNav = new Years();
            Months monthsNav = new Months();

            employees = GetValuesAllEmployees();
            months = GetValuesAllMonths();
            years = GetValuesAllYears();



            using (EnterprisesContext db = new EnterprisesContext())
            {
                SqlParameter YearParam = new SqlParameter("@Year", Year);
                SqlParameter MonthParam = new SqlParameter("@Month", Month);
                salaryEmps = db.Salary.FromSqlRaw("SelectYearMonth @Year,@Month", YearParam, MonthParam).ToList();

            }

            if (salaryEmps.Count != 0)
            {
                foreach (var emp in employees)
                {
                    foreach (var year in years)
                    {
                        foreach (var month in months)
                        {
                            foreach (var item in salaryEmps)
                            {
                                if (emp.Id == item.Employee && year.YearName == item.Year && month.Id == item.Month)
                                {
                                    employeesNav = emp;
                                    yearsNav = year;
                                    monthsNav = month;
                                    salariesData.Add(new SalaryEmp()
                                    {
                                        Id = item.Id,
                                        Year = item.Year,
                                        Month = item.Month,
                                        Employee = item.Employee,
                                        ParticipationPurchase = item.ParticipationPurchase,
                                        ParticipationSale = item.ParticipationSale,
                                        ParticipationProduction = item.ParticipationProduction,
                                        CountParticipation = item.CountParticipation,
                                        SalaryEmployee = item.SalaryEmployee,
                                        TotalAmount = item.TotalAmount,
                                        Issued = item.Issued,
                                        Bonus = item.Bonus,
                                        YearNavigation = yearsNav,
                                        MonthNavigation = monthsNav,
                                        EmployeeNavigation = employeesNav,
                                    });
                                }
                            }
                        }
                    }
                }
            }

            return salariesData;
        }

        public List<SalaryEmp> GetValuesAllSalary()
        {
            List<Employees> employees = new List<Employees>();
            List<SalaryEmp> salaries = new List<SalaryEmp>();
            List<SalaryEmp> salariesData = new List<SalaryEmp>();

            Employees employeesNav = new Employees();
            Years yearsNav = new Years();
            Months monthsNav = new Months();

            using (EnterprisesContext db = new EnterprisesContext())
            {

                employees = db.Employees.FromSqlRaw("SelectAllEmployees").ToList();
            }


            var years = GetValuesAllYears();
            var months = GetValuesAllMonths();

            using (EnterprisesContext db = new EnterprisesContext())
            {

                salaries = db.Salary.FromSqlRaw("SelectAllSalary").ToList();
            }

            if (salaries != null && salaries.Count != 0)
            {
                foreach (var emp in employees)
                {
                    foreach (var year in years)
                    {
                        foreach (var month in months)
                        {
                            foreach (var item in salaries)
                            {
                                if (emp.Id == item.Employee && year.YearName == item.Year && month.Id == item.Month)
                                {
                                    employeesNav = emp;
                                    yearsNav = year;
                                    monthsNav = month;
                                    salariesData.Add(new SalaryEmp()
                                    {
                                        Id = item.Id,
                                        Year = item.Year,
                                        Month = item.Month,
                                        Employee = item.Employee,
                                        ParticipationPurchase = item.ParticipationPurchase,
                                        ParticipationSale = item.ParticipationSale,
                                        ParticipationProduction = item.ParticipationProduction,
                                        CountParticipation = item.CountParticipation,
                                        SalaryEmployee = item.SalaryEmployee,
                                        TotalAmount = item.TotalAmount,
                                        Issued = item.Issued,
                                        Bonus = item.Bonus,
                                        YearNavigation = yearsNav,
                                        MonthNavigation = monthsNav,
                                        EmployeeNavigation = employeesNav,
                                    });
                                }
                            }
                        }
                    }
                }
            }

            return salariesData;
        }

        public void Check(int Year, int Month)
        {
            List<SalaryEmp> salaryEmps = new List<SalaryEmp>();
            using (EnterprisesContext db = new EnterprisesContext())
            {
                SqlParameter YearParam = new SqlParameter("@Year", Year);
                SqlParameter MonthParam = new SqlParameter("@Month", Month);
                salaryEmps = db.Salary.FromSqlRaw("SelectYearMonth @Year,@Month", YearParam, MonthParam).ToList();

            }
            if (salaryEmps.Count != 0 && salaryEmps != null)
            {
                foreach (var item in salaryEmps)
                {
                    allcountOfSalary += (float)item.TotalAmount;
                }
            }
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

        public List<Years> GetValuesAllYears()
        {
            List<Years> years = new List<Years>();
            using (EnterprisesContext db = new EnterprisesContext())
            {

                years = db.Years.FromSqlRaw("SelectAllYears").ToList();
            }

            return years;
        }

        public List<Months> GetValuesAllMonths()
        {
            List<Months> months = new List<Months>();
            using (EnterprisesContext db = new EnterprisesContext())
            {

                months = db.Months.FromSqlRaw("SelectAllMonths").ToList();
            }

            return months;
        }


        public async Task<IActionResult> Index(int yearString, int monthString, string Messege)
        {
            var Year = await Task.Run(() => GetValuesAllYears());
            var Month = await Task.Run(() => GetValuesAllMonths());
            var Employee = await Task.Run(() => GetValuesAllEmployees());
            var Salary= await Task.Run(() => GetValuesAllSalary());
            Employees empNav = new Employees();
            Years yearNav = new Years();
            Months monthNav = new Months();
            List<SalaryEmp> salariesData = new List<SalaryEmp>();

            if (Salary.Count != 0)
            {
                foreach (var emp in Employee)
                {
                    foreach (var year in Year)
                    {
                        foreach (var month in Month)
                        {
                            foreach (var item in Salary)
                            {
                                if(emp.Id==item.Employee && year.YearName==item.Year && month.Id == item.Month)
                                {
                                    empNav = emp;
                                    yearNav = year;
                                    monthNav = month;
                                    salariesData.Add(new SalaryEmp()
                                    {
                                        Id = item.Id,
                                        Year = item.Year,
                                        Month = item.Month,
                                        Employee = item.Employee,
                                        ParticipationPurchase = item.ParticipationPurchase,
                                        ParticipationSale = item.ParticipationSale,
                                        ParticipationProduction = item.ParticipationProduction,
                                        CountParticipation = item.CountParticipation,
                                        SalaryEmployee = item.SalaryEmployee,
                                        TotalAmount = item.TotalAmount,
                                        Issued = item.Issued,
                                        Bonus = item.Bonus,
                                        YearNavigation = yearNav,
                                        MonthNavigation = monthNav,
                                        EmployeeNavigation = empNav,
                                    });
                                }
                            }
                        }
                    }
                }
            }
            List<string> summa = new List<string>();
            if (yearString != 0 && monthString != 0) 
            {


                ViewData["Year"] = new SelectList(Year, "YearName", "YearName", yearString);
                ViewData["Month"] = new SelectList(Month, "Id", "MonthName", monthString);

                var salaryEmp = await Task.Run(()=>GetValuesSalaryEmpByYearAndMonth(yearString, monthString));

                ViewBag.AddMessege = Messege ?? "";

                Check(yearString, Convert.ToInt32(monthString));
                summa.Add(allcountOfSalary.ToString());
                ViewBag.Summa = allcountOfSalary.ToString();
                if (salaryEmp.Count == 0) 
                {
                    SqlParameter YearParam = new SqlParameter("@Year", yearString);
                    SqlParameter MonthParam = new SqlParameter("@Month", monthString);
                 
                    using (EnterprisesContext db = new EnterprisesContext())
                    {
                        var resultrow = await db.Database.ExecuteSqlRawAsync("SP_Zarplata @Year,@Month", YearParam, MonthParam);

                    }
                    
                    return RedirectToAction("Index", new { yearString = yearString, monthString = monthString, Messege = "" });

                }
                else
                {

                    return View(salaryEmp);
                }


            }
            else // если год и месяц не были выбраны на страницу выводим все записи
            {
                ViewData["Year"] = new SelectList(Year, "YearName", "YearName");
                ViewData["Month"] = new SelectList(Month, "Id", "MonthName");
                for (int i = 0; i < salariesData.Count; i++)
                {
                    allcountOfSalary += (float)salariesData[i].TotalAmount; // Посчитываем общую сумму всех сотрудников
                }

                summa.Add(allcountOfSalary.ToString());
                ViewBag.Summa = allcountOfSalary.ToString();
            }


            return View(salariesData);
        }

        public async Task<IActionResult> IndexCopyAdd(int yearString, int monthString) // Функция для выдачи ЗП
        {

            var Year = await Task.Run(() => GetValuesAllYears());
            var Month = await Task.Run(() => GetValuesAllMonths());
            List<string> summa = new List<string>();

            allcountOfSalary = 0;

            ViewData["Year"] = new SelectList(Year, "YearName", "YearName", yearString);
            ViewData["Month"] = new SelectList(Month, "Id", "MonthName", monthString);

           

            Check(yearString, Convert.ToInt32(monthString));
            summa.Add(allcountOfSalary.ToString());

            ViewBag.Summa = allcountOfSalary;
            var salaryEmp = await Task.Run(() => GetValuesAllSalary());


            if (yearString != 0 && monthString >= 0)
            {
                Check(yearString, Convert.ToInt32(monthString));
                salaryEmp = await Task.Run(() => GetValuesSalaryEmpByYearAndMonth(yearString, monthString));

                SqlParameter YearParam = new SqlParameter("@Year", yearString);
                SqlParameter MonthParam = new SqlParameter("@Month", monthString);
                SqlParameter p = new SqlParameter("@p", SqlDbType.Int);
                p.Direction = ParameterDirection.Output;

                using (EnterprisesContext db = new EnterprisesContext())
                {
                    var resultrow = await db.Database.ExecuteSqlRawAsync("SP_IssuedAndBudget @Year,@Month,@p output", 
                        YearParam, MonthParam,p);

                }

                var k = Convert.ToInt32(p.Value);
                
                if (k == 1) 
                {
                    M = "Не хватает денег в бюджете для зачисления зарплаты!";
                    return RedirectToAction("Index", new { yearString = yearString, monthString = monthString, Messege = M });

                }
                else if (k == 2) 
                {
                    M = "По выбранной дате зарплата уже выдана!";
                    return RedirectToAction("Index", new { yearString = yearString, monthString = monthString, Messege = M });
                }
                else 
                {

                    SqlParameter YearParaminsert = new SqlParameter("@Year", yearString);
                    SqlParameter MonthParaminsert = new SqlParameter("@sum", monthString);
                    
                    using (EnterprisesContext db = new EnterprisesContext())
                    {
                        var resultrow = await db.Database.ExecuteSqlRawAsync("IssuedUpdate @Year,@Month",
                            YearParam, MonthParam);

                    }
                    return RedirectToAction("Index", new { yearString = yearString, monthString = monthString, Messege = "" });
                }
            }
            return View(salaryEmp);
        }
    }
}
