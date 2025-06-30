using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Souq.Models;

namespace Souq.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            SouqcomContext db = new SouqcomContext();
            var list = db .Products.Select(x=> new { x.Id, x.Name }).ToList();
            ViewBag.CatList = new SelectList(list,"Id","Name",3);
            return View();
        }
        public IActionResult charts()
        {
          
            return View();
        }
        [HttpPost]
        public IActionResult Create(ProductVM model)
        {
            if (ModelState.IsValid)
            {
                SouqcomContext db = new SouqcomContext();
                Category c = new Category();

                c.Name = model.CatName;
                db.Categories.Add(c);   
                db.Products.Add(new Product 
                {
                    Name = model.ProductName,
                    Price = model.ProductPrice,
                    Quantity= (int?)model.ProductQty,
                    Cat= c
                });
                db.SaveChanges();
                return View("Index");
            }
            return RedirectToAction("Index",model);
        }
    }
}