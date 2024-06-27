using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NexusWeb.Helpers;
using NexusWeb.Models;

namespace NexusWeb.Controllers
{
    public class UsersController : Controller
    {
        private readonly NexusWebAppContext _context;

        public UsersController(NexusWebAppContext context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            ViewData["CountTotal"] = _context.Users.Where(x => x.Level != 0).Count();
            var naxusWebAppContext = _context.Users.Where(x => x.Level != 0).Include(i => i.Orders).Include(i=> i.Invoices);
            return View(await naxusWebAppContext.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        [HttpGet]
        public IActionResult Create() => View();

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Create(User user, IFormFile ImageData)
        {
            if (ModelState.IsValid)
            {
                var check = _context.Users.SingleOrDefault(u => u.UserName == user.UserName);
                if (check != null)
                {
                    ViewData["Message"] = "Username already exists !";
                    return View();
                }else
                {

                    User UserN = new User {
                        UserName = user.UserName,
                        Password = user.Password.ToMd5Hash(user.RandomKey),
                        Status = 1,
                        Level = user.Level,
                        RandomKey = user.RandomKey,
                        FullName = user.FullName,
                        Address = user.Address,
                        Phone = user.Phone,
                    };
                    if (ImageData != null)
                    {
                        var folder = "images";
                        MyUtil.UploadHinh(ImageData, folder);
                        UserN.Image = ImageData.FileName;

                    }
                    else
                    {
                        UserN.Image = "";
                    }

                    


                    try
                    {
                        _context.Users.Add(UserN);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    catch (Exception ex)
                    {
                        ViewData["Message"] = $"{ex.Message} shh";
                        return View();
                    }

                }

            }
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, User user, IFormFile ImageData)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if(ImageData!= null)
                {
                    string folder = "images";
                    MyUtil.UploadHinh(ImageData, folder);
                    user.Image = ImageData.FileName;
                }
                try
                {

                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
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
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
