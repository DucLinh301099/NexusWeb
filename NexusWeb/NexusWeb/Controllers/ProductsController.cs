using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using NexusWeb.Helpers;
using NexusWeb.Models;
using NexusWeb.ViewModels;
using static NuGet.Packaging.PackagingConstants;

namespace NexusWeb.Controllers
{
	[Authorize(Roles = "Admin,Retail Store Employee,Technical staff,Accounts Department Officer")]
	public class ProductsController : Controller
    {
        private readonly NexusWebAppContext _context;

        public ProductsController(NexusWebAppContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var naxusWebAppContext = _context.Products.Include(p => p.Category).Include(p => p.Distributor);
            return View(await naxusWebAppContext.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Distributor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            ViewData["DistributorId"] = new SelectList(_context.Distributors, "Id", "Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( ProductVM productvm)
        {
            if (ModelState.IsValid)
            {
                string folder = "images";
                
                Product NewP = new Product {
                    Name = productvm.Name
                    ,Price = productvm.Price
                    ,Description = productvm.Description
                    ,Detail = productvm.Detail,
                    DistributorId = productvm.DistributorId
                    ,CategoryId = productvm.CategoryId,
                };
                if (productvm.ImageData != null)
                {
                    MyUtil.UploadHinh(productvm.ImageData, folder);
                    NewP.AvatarImages = productvm.ImageData.FileName;
                }
                _context.Products.Add(NewP);
                await _context.SaveChangesAsync();

                Product checkId = _context.Products.SingleOrDefault(x=> x.Name == productvm.Name);
                if (!productvm.LImageData.IsNullOrEmpty())
                {

                    foreach (var itemI in productvm.LImageData)
                    {
                        ProductImage PI = new ProductImage
                        {
                            Name = itemI.FileName,
                            ProductId = checkId.Id,
                        };
                        _context.ProductImages.Add(PI);
                        await _context.SaveChangesAsync();
                        MyUtil.UploadHinh(itemI, folder);
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", productvm.CategoryId);
            ViewData["DistributorId"] = new SelectList(_context.Distributors, "Id", "Name", productvm.DistributorId);
            return View(productvm);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if(product != null)
            {
                ProductVM p = new ProductVM
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Description = product.Description,
                    Detail = product.Detail,
                    DistributorId = product.DistributorId,
                    CategoryId = product.CategoryId,

                };
                ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
                ViewData["DistributorId"] = new SelectList(_context.Distributors, "Id", "Name", product.DistributorId);
                return View(p);
            }
            else
            {
                return NotFound();
            }
            
            
            
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductVM productvm)
        {
            if (id != productvm.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                string folder = "images";
                Product P = _context.Products.FirstOrDefault(x => x.Id == id);
                if (P == null)
                {
                    return NotFound();
                }
                P.Name = productvm.Name;
                P.Price = productvm.Price;
                P.Description = productvm.Description;
                P.Detail = productvm.Detail;
                P.DistributorId = productvm.DistributorId;
                P.CategoryId = productvm.CategoryId;

                if (productvm.ImageData != null)
                {
                    MyUtil.UploadHinh(productvm.ImageData, folder);
                    P.AvatarImages = productvm.ImageData.FileName;
                }
                Product checkId = _context.Products.SingleOrDefault(x => x.Id == id);
                if (!productvm.LImageData.IsNullOrEmpty())
                {

                    foreach (var itemI in productvm.LImageData)
                    {
                        ProductImage PI = new ProductImage
                        {
                            Name = itemI.FileName,
                            ProductId = checkId.Id,
                        };
                        _context.ProductImages.Add(PI);
                        await _context.SaveChangesAsync();
                        MyUtil.UploadHinh(itemI, folder);
                    }
                }
                try
                {
                    _context.Update(P);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(P.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                
                
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", productvm.CategoryId);
            ViewData["DistributorId"] = new SelectList(_context.Distributors, "Id", "Name", productvm.DistributorId);
            return View(productvm);
        }

        // POST: Products/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }

           List<ProductImage>  LproductImage = await _context.ProductImages.Where(x=>x.ProductId==id).ToListAsync();
            foreach (var imageP in LproductImage)
            {
                id = imageP.Id;
                var productI= await _context.ProductImages.FindAsync(id);
                _context.ProductImages.Remove(productI);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
