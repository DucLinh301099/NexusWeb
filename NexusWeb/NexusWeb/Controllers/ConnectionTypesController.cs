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
	public class ConnectionTypesController : Controller
    {   
        private readonly NexusWebAppContext _context;

        public ConnectionTypesController(NexusWebAppContext context)
        {
            _context = context;
        }

        // GET: ConnectionTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.ConnectionTypes.ToListAsync());
        }

        // GET: ConnectionTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var connectionType = await _context.ConnectionTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (connectionType == null)
            {
                return NotFound();
            }

            return View(connectionType);
        }

        // GET: ConnectionTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ConnectionTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Deposit")] ConnectionType connectionType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(connectionType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(connectionType);
        }

        // GET: ConnectionTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var connectionType = await _context.ConnectionTypes.FindAsync(id);
            if (connectionType == null)
            {
                return NotFound();
            }
            return View(connectionType);
        }

        // POST: ConnectionTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Deposit")] ConnectionType connectionType)
        {
            if (id != connectionType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(connectionType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConnectionTypeExists(connectionType.Id))
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
            return View(connectionType);
        }

        // GET: ConnectionTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var connectionType = await _context.ConnectionTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (connectionType == null)
            {
                return NotFound();
            }

            return View(connectionType);
        }

        // POST: ConnectionTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var connectionType = await _context.ConnectionTypes.FindAsync(id);
            if (connectionType != null)
            {
                _context.ConnectionTypes.Remove(connectionType);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ConnectionTypeExists(int id)
        {
            return _context.ConnectionTypes.Any(e => e.Id == id);
        }
    }
}
