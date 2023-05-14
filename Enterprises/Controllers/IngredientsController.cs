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
    public class IngredientsController : Controller
    {
        
        public Ingredients GetValuesIngredientsByID(int Id)
        {
			Ingredients ingredients = new Ingredients();

			using (EnterprisesContext db = new EnterprisesContext())
			{
				SqlParameter ID = new("@ID", Id);
				var listIngredients = db.Ingredients.FromSqlRaw("SelectIngredients @ID", ID).ToList();
				if (listIngredients != null)
				{
					foreach (var item in listIngredients)
					{
						ingredients = item;
					}
				}
			}
            return ingredients;

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
        public List<Finproducts> GetValuesFinproductsByID(int Id)
        {
			List<Finproducts> finproducts = new List<Finproducts>();
			using (EnterprisesContext db = new EnterprisesContext())
			{
				SqlParameter ID = new("@ID", Id);
				finproducts = db.Finproducts.FromSqlRaw("SelectFinproducts @ID", ID).ToList();
			}
            return finproducts;
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

       
        public async Task<IActionResult> Index(int SearchString)
        {
			List<Finproducts> finproducts = new List<Finproducts>();
			List<Rawmaterials> rawmaterials = new List<Rawmaterials>();
			List<Ingredients> ingredients = new List<Ingredients>();
			List<Ingredients> ingredientsId = new List<Ingredients>();
			Units unitsNav = new Units();
			List<Ingredients> ingredientsData = new List<Ingredients>();
			List<Ingredients> ingredientsDataByID = new List<Ingredients>();

            Finproducts finNav= new Finproducts();
            Rawmaterials rawNav = new Rawmaterials();


			finproducts = await Task.Run(() => GetValuesAllFinproducts());
			rawmaterials = await Task.Run(() => GetValuesAllRawmaterials());

			using (EnterprisesContext db = new EnterprisesContext())
			{
				ingredients = await db.Ingredients.FromSqlRaw("SelectAllIngredients").ToListAsync();
			}

            if( ingredients != null && ingredients.Count > 0)
            {
                foreach(var fin in finproducts)
                {
                    foreach(var raw in rawmaterials)
                    {
                        foreach (var ing in ingredients)
                        {
							if (fin.Id == ing.Product && raw.Id == ing.RawMaterials)
                            {
								finNav = fin;
								rawNav = raw;
								ingredientsData.Add(new Ingredients()
								{
									Id = ing.Id,
									Product = ing.Product,
									RawMaterials = ing.RawMaterials,
									Countingred = ing.Countingred,
									ProductNavigation = finNav,
									RawMaterialsNavigation = rawNav,
								});
							}

						}
                        
                    }
                }
            }
            
            if (SearchString == 0)
            {
                var finproductsList = await Task.Run(()=>GetValuesAllFinproducts());
                ViewData["Product"] = new SelectList(finproductsList, "Id", "Name");
                return View(ingredientsData);
            }
            else
            {
				using (EnterprisesContext db = new EnterprisesContext())
				{
					SqlParameter ID = new("@Search", SearchString);
					ingredientsId = db.Ingredients.FromSqlRaw("FilterOfingr @Search", ID).ToList();
				}
				if (ingredients != null && ingredients.Count > 0)
				{
					foreach (var fin in finproducts)
					{
						foreach (var raw in rawmaterials)
						{
							foreach (var ing in ingredientsId)
							{
								if (fin.Id == ing.Product && raw.Id == ing.RawMaterials)
								{
									finNav = fin;
									rawNav = raw;
									ingredientsDataByID.Add(new Ingredients()
									{
										Id = ing.Id,
										Product = ing.Product,
										RawMaterials = ing.RawMaterials,
										Countingred = ing.Countingred,
										ProductNavigation = finNav,
										RawMaterialsNavigation = rawNav,
									});
								}

							}

						}
					}
				}

				var finproductsList = await Task.Run(()=>GetValuesAllFinproducts());
                ViewData["Product"] = new SelectList(finproductsList, "Id", "Name");

                return View(ingredientsDataByID);
            }
          
        }

       
        public IActionResult Create(int ID)
        {
            var productList = GetValuesAllFinproducts();
            var rawmaterialList = GetValuesAllRawmaterials();
            if (ID == 0)
            {
                ViewData["Product"] = new SelectList(productList, "Id", "Name");
                ViewData["RawMaterials"] = new SelectList(rawmaterialList, "Id", "Name");
            }
            else
            {
                ViewData["Product"] = new SelectList(productList.Where(e=>e.Id==ID), "Id", "Name");
                ViewData["RawMaterials"] = new SelectList(rawmaterialList, "Id", "Name");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] Ingredients ingredients)
        {
            if (ModelState.IsValid)
            {
				SqlParameter Product = new SqlParameter("@Product", ingredients.Product);
				SqlParameter RawMaterials = new SqlParameter("@RawMaterials", ingredients.RawMaterials);
				SqlParameter Countingred = new SqlParameter("@Countingred", ingredients.Countingred);
				using (EnterprisesContext db = new EnterprisesContext())
				{
					var resultrow = await db.Database.ExecuteSqlRawAsync("InsertIngredients @Product,@RawMaterials" +
						",@Countingred", Product, RawMaterials, Countingred);

				}
                int ID = Convert.ToInt32(ingredients.Product);
                return RedirectToAction("Index", new { searchString = ID });
            }
            var productlist = await Task.Run(()=>GetValuesAllFinproducts());
            var result = await Task.Run(() => GetValuesIngredientsByID(ingredients.Id));
            var rawMaterialstlist = await Task.Run(() => GetValuesAllRawmaterials());
            ViewData["Product"] = new SelectList(productlist, "Id", "Name", result.Product);
            ViewData["RawMaterials"] = new SelectList(rawMaterialstlist, "Id", "Name", result.RawMaterials);
            return View(result);
        }

        public async Task<IActionResult> Edit(short id)
        {
            
            var productlist = await Task.Run(()=>GetValuesAllFinproducts());
            var result = await Task.Run(() =>GetValuesIngredientsByID(id));
            int productID = (int)result.Product;
            var rawMaterialstlist = await Task.Run(() =>GetValuesAllRawmaterials());
            if (result == null)
            {
                return NotFound();
            }

            ViewData["Product"] = new SelectList(productlist.Where(e=>e.Id== productID), "Id", "Name");
            ViewData["RawMaterials"] = new SelectList(rawMaterialstlist, "Id", "Name", result.RawMaterials);
            return View(result);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(short id, [FromForm] Ingredients ingredients)
        {
           
            
            if (ModelState.IsValid)
            {

				SqlParameter IDIng = new SqlParameter("@ID", id);
				SqlParameter Product = new SqlParameter("@Product", ingredients.Product);
				SqlParameter RawMaterials = new SqlParameter("@RawMaterials", ingredients.RawMaterials);
				SqlParameter Countingred = new SqlParameter("@Countingred", ingredients.Countingred);
				using (EnterprisesContext db = new EnterprisesContext())
				{
					var resultrow = await db.Database.ExecuteSqlRawAsync("UpdateIngredients @ID,@Product,@RawMaterials" +
						",@Countingred", IDIng, Product, RawMaterials, Countingred);

				}
                int ID = Convert.ToInt32(ingredients.Product);
                return RedirectToAction("Index", new { searchString = ID });
                
            }
			var productlist = await Task.Run(() => GetValuesAllFinproducts());
			var result = await Task.Run(() => GetValuesIngredientsByID(id));
			var rawMaterialstlist = await Task.Run(() => GetValuesAllRawmaterials());
			ViewData["Product"] = new SelectList(productlist, "Id", "Name", result.Product);
			ViewData["RawMaterials"] = new SelectList(rawMaterialstlist, "Id", "Name", result.RawMaterials);
			
            return View(result);
        }

      
        public async Task<IActionResult> Delete(short id)
        {
            Finproducts finproducts = new Finproducts();
            Rawmaterials rawmaterials= new Rawmaterials();
			var resultIng =await Task.Run(()=>GetValuesIngredientsByID(id));
            if (resultIng == null)
            {
                return NotFound();
            }

			using (EnterprisesContext db = new EnterprisesContext())
			{
				SqlParameter ID = new("@ID", resultIng.Product);
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
				SqlParameter ID = new("@ID", resultIng.RawMaterials);
				var listrawmaterials = await db.Rawmaterials.FromSqlRaw("SelectRawmaterials @ID", ID).ToListAsync();
				if (listrawmaterials != null)
				{
					foreach (var item in listrawmaterials)
					{
						rawmaterials = item;
					}
				}
			}

			resultIng.ProductNavigation = finproducts;
			resultIng.RawMaterialsNavigation = rawmaterials;
            return View(resultIng);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(short id)
        {
            
			using (EnterprisesContext db = new EnterprisesContext())
			{
				SqlParameter ID = new SqlParameter("@ID", id);
				var resultrow = await db.Database.ExecuteSqlRawAsync("DeleteIngredients @ID", ID);
			}
			return RedirectToAction(nameof(Index));
		}
    }
}
