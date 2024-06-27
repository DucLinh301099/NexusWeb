using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NexusWeb.Models;

namespace NexusWeb.Controllers
{
	[Authorize(Roles = "Admin,Retail Store Employee,Technical staff,Accounts Department Officer")]
	public class ConnectionsController : Controller
    {
        private readonly NexusWebAppContext _context;

        public ConnectionsController(NexusWebAppContext context)
        {
            _context = context;
        }

        // GET: Connections
        public async Task<IActionResult> Index()
        {
            var naxusWebAppContext = _context.Connections.Include(c => c.ConnectionType);
            return View(await naxusWebAppContext.ToListAsync());
        }

        // GET: Connections/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var connection = await _context.Connections
                .Include(c => c.ConnectionType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (connection == null)
            {
                return NotFound();
            }

            return View(connection);
        }

        // GET: Connections/Create
        public IActionResult Create()
        {
            ViewData["ConnectionTypeId"] = new SelectList(_context.ConnectionTypes, "Id", "Id");
            return View();
        }

        // POST: Connections/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,ConnectionTypeId")] Connection connection)
        {
            if (ModelState.IsValid)
            {
                _context.Add(connection);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ConnectionTypeId"] = new SelectList(_context.ConnectionTypes, "Id", "Id", connection.ConnectionTypeId);
            return View(connection);
        }

        // GET: Connections/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var connection = await _context.Connections.FindAsync(id);
            if (connection == null)
            {
                return NotFound();
            }
            ViewData["ConnectionTypeId"] = new SelectList(_context.ConnectionTypes, "Id", "Id", connection.ConnectionTypeId);
            return View(connection);
        }

        // POST: Connections/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,ConnectionTypeId")] Connection connection)
        {
            if (id != connection.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(connection);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConnectionExists(connection.Id))
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
            ViewData["ConnectionTypeId"] = new SelectList(_context.ConnectionTypes, "Id", "Id", connection.ConnectionTypeId);
            return View(connection);
        }

        // GET: Connections/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var connection = await _context.Connections
                .Include(c => c.ConnectionType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (connection == null)
            {
                return NotFound();
            }

            return View(connection);
        }

        // POST: Connections/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var connection = await _context.Connections.FindAsync(id);
            if (connection != null)
            {
                _context.Connections.Remove(connection);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ConnectionExists(int id)
        {
            return _context.Connections.Any(e => e.Id == id);
        }
    }
}
