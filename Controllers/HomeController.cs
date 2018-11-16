using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

using Microsoft.AspNetCore.Identity;
using productcategories.Models;
using Microsoft.EntityFrameworkCore;
namespace productcategories.Controllers
{
    public class HomeController : Controller
    {
        private AllContext dbContext;

        public HomeController(AllContext context)
        {
            dbContext = context;
        }
        [HttpGet("")]
        public IActionResult Index()
        {
            List<Products> AllProducts = dbContext.products.ToList();
            ViewBag.Products = AllProducts;
            return View("Index");
        }
        [HttpGet("categories")]
        public IActionResult categoriesSplash()
        {
            List<Categories> AllCategories = dbContext.categories.ToList();
            ViewBag.Categories = AllCategories;
            return View("Categories");
        }
        [HttpPost("AddProduct")]
        public IActionResult AddProduct(Products product)
        {
            if (ModelState.IsValid)
            {
                dbContext.products.Add(product);
                dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            //rendering on a Post method to preserve model state, Keeping ViewBag Constant.
            List<Products> AllProducts = dbContext.products.ToList();
            ViewBag.Products = AllProducts;
            return View("Index", product);
        }
        [HttpPost("AddCategory")]
        public IActionResult AddCategory(Categories Category)
        {
            if (ModelState.IsValid)
            {
                dbContext.categories.Add(Category);
                dbContext.SaveChanges();
                return RedirectToAction("categoriesSplash");
            }
            //rendering on a Post method to preserve model state, Keeping ViewBag Constant.
            List<Categories> AllCategories = dbContext.categories.ToList();
            ViewBag.Categories = AllCategories;
            return View("Index", Category);
        }
        [HttpGet("products/{id}")]
        public IActionResult GetProduct(int id)
        {
            Products retrievedItem = dbContext.products.FirstOrDefault(x => x.ProductId == id);
            var productcategories = dbContext.products.Include(x => x.Associations).ThenInclude(z => z.Categories).Where(z => z.ProductId == retrievedItem.ProductId).ToList();
            List<Categories> getrid = new List<Categories>();
            foreach (var x in productcategories)
            {
                foreach (var y in x.Associations)
                {
                    Categories Not = dbContext.categories.FirstOrDefault(z => z.CategoryId == y.Categories.CategoryId);
                    getrid.Add(Not);
                }
            }
            List<Categories> AllCategories = dbContext.categories.Except(getrid).ToList();
            ViewBag.AllCategories = AllCategories;
            ViewBag.Product = productcategories;
            ViewBag.Item = retrievedItem;
            return View();
        }
        [HttpGet("categories/{id}")]
        public IActionResult GetCategory(int id)
        {
            Categories retrievedItem = dbContext.categories.FirstOrDefault(x => x.CategoryId == id);
            var CategoryProducts = dbContext.categories.Include(x => x.Associations).ThenInclude(z => z.Products).Where(z => z.CategoryId == retrievedItem.CategoryId).ToList();

            List<Products> getrid = new List<Products>();
            foreach (var x in CategoryProducts)
            {
                foreach (var y in x.Associations)
                {
                    Products Not = dbContext.products.FirstOrDefault(z => z.ProductId == y.Products.ProductId);
                    getrid.Add(Not);
                }
            }
            List<Products> AllProducts = dbContext.products.Except(getrid).ToList();
            ViewBag.Products = AllProducts;
            ViewBag.CategoryProducts = CategoryProducts;
            ViewBag.Category = retrievedItem;
            return View();
        }
        [HttpPost("AddProductAssociation")]
        public IActionResult AddProductAssociation(Associations association)
        {
            if (!dbContext.associations.Any(i => i.ProductId == association.ProductId && i.CategoryId == association.CategoryId))
            {
                if (ModelState.IsValid)
                {
                    dbContext.associations.Add(association);
                    dbContext.SaveChanges();
                    return Redirect("/categories/" + association.CategoryId);
                }
            }
            Categories retrievedItem = dbContext.categories.FirstOrDefault(x => x.CategoryId == association.CategoryId);
            var CategoryProducts = dbContext.categories.Include(x => x.Associations).ThenInclude(z => z.Products).Where(z => z.CategoryId == retrievedItem.CategoryId).ToList();
            List<Products> AllProducts = dbContext.products.ToList();
            ViewBag.Products = AllProducts;
            ViewBag.CategoryProducts = CategoryProducts;
            ViewBag.Category = retrievedItem;
            return View("GetCategory", association);
        }
        [HttpPost("AddCategoryAssociation")]
        public IActionResult AddCategoryAssociation(Associations association)
        {
            if (!dbContext.associations.Any(i => i.ProductId == association.ProductId && i.CategoryId == association.CategoryId))
            {
                if (ModelState.IsValid)
                {
                    dbContext.associations.Add(association);
                    dbContext.SaveChanges();
                    return Redirect("/products/" + association.ProductId);
                }
            }
            Products retrievedItem = dbContext.products.FirstOrDefault(x => x.ProductId == association.ProductId);
            var productcategories = dbContext.products.Include(x => x.Associations).ThenInclude(z => z.Categories).Where(z => z.ProductId == retrievedItem.ProductId).ToList();
            List<Categories> AllCategories = dbContext.categories.ToList();
            ViewBag.AllCategories = AllCategories;
            ViewBag.Product = productcategories;
            ViewBag.Item = retrievedItem;
            return View("GetProduct");
        }
    }
}