using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Souq.Models;
using System.Diagnostics;

namespace Souq.Controllers
{
    public class HomeController : Controller
    {

        SouqcomContext db = new SouqcomContext();

        public IActionResult Index()
        {
            var result = new indexVm();

            try
            {
                result.Categories = db.Categories.ToList();
                result.products = db.Products.ToList();
                result.Reviews = db.Reviews.ToList();
                result.Latestproducts = db.Products.OrderByDescending(x => x.EntryDate).Take(4).ToList();
            }
            catch (Exception ex)
            {
                // سجل الاستثناء أو قم بإظهار رسالة مناسبة
                Debug.WriteLine($"Error fetching data: {ex.Message}");
            }

            return View(result);
        }

        public IActionResult Privacy()
        {
            return View();
        }
        [Authorize]
        public IActionResult Cart()
        {
            var result = db.Carts.Include(x => x.Product).Where(x => x.UserId == User.Identity.Name).ToList();
            return View(result);
        }
        [Authorize]
        public IActionResult AddProducttoCart(int id)
        {
            var price = db.Products.Find(id).Price;
          var item=  db.Carts.FirstOrDefault(x => x.ProductId == id && x.UserId == User.Identity.Name);
            if (item != null)
            {
                item.Qty += 1;
                db.SaveChanges();
            }
            else
            {
                db.Carts.Add(new Cart { ProductId = id, UserId = User.Identity.Name, Qty = 1, Price = price });
                db.SaveChanges();
            }
            return RedirectToAction("Cart");        
        }
        public IActionResult Categories()
        {

            var cats = db.Categories.ToList();
            ViewBag.isAdmin = true; 
            return View(cats);
        }
        public IActionResult Products(int id)
        {
            var Products = db.Products.Where(x => x.Catid == id).ToList();
            return View(Products);
        }
        [Authorize]
        public IActionResult orders()
        {
            var orders = db.Orders.Include(x=>x.OrderDetails).ThenInclude(x=>x.Product).Where(x => x.UserId == User.Identity.Name).ToList();
            return View(orders);
        }
        public IActionResult currentProduct(int id)
        {
            var product = db.Products
                             .Include(x => x.Cat) // التأكد من تضمين الفئة المرتبطة
                             .Include(x => x.ProductImages) // التأكد من تضمين الصور المرتبطة
                             .FirstOrDefault(x => x.Id == id); // البحث عن المنتج بناءً على المعرف

            if (product == null)
            {
                return NotFound(); // إذا لم يتم العثور على المنتج
            }

            return View(product); // عرض المنتج في الـ View
        }

        [HttpGet]
        public IActionResult ProductSearch(string xname)
        {
            var Products = new List<Product>();
            if (string.IsNullOrEmpty(xname))
                Products = db.Products.ToList();
            else
                 Products = db.Products.Where(x => x.Name.Contains(xname)).ToList();
            return View(Products);
        }
        [HttpPost]
        public IActionResult sendreview( ReviewVm model)
        {
            string x = model.age;
            db.Reviews.Add(new Review {Name=model.Name , Email=model.Email, Subject=model.Subject,Description=model.Description });
            db.SaveChanges();
            return RedirectToAction("Index");  
        }
        [HttpPost]
        [Authorize]
        public IActionResult AddOrder(Order model)
        {
            Order o = new Order {Email=model.Email,Mobile=model.Mobile,Name=model.Name,Address=model.Address,UserId=User.Identity.Name };
            var cartItems= db.Carts.Where(x => x.UserId == User.Identity.Name).ToList();
            foreach (var item in cartItems)
            {
                var total = item.Qty * item.Price;
                o.OrderDetails.Add(new OrderDetail { Qty = item.Qty, Price = item.Price, ProductId = item.ProductId, Totalprice = total });
            }
            db.Carts.RemoveRange(cartItems);
            db.Orders.Add(o);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}