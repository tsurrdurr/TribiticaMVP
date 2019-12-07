using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TribiticaMVP.Models;

namespace TribiticaMVP.Controllers
{
    public class AccountController : Controller
    {
        private readonly TribiticaDbContext _context;

        public AccountController()
        {
            _context = new TribiticaDbContext();
        }

        // GET: Account
        public async Task<IActionResult> Index()
        {
            return View(await _context.Accounts.ToListAsync());
        }

        // GET: Account/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tribiticaAccount = await _context.Accounts
                .FirstOrDefaultAsync(m => m.ID == id);
            if (tribiticaAccount == null)
            {
                return NotFound();
            }

            return View(tribiticaAccount);
        }

        // GET: Account/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Account/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Email,SelfSummary")] TribiticaAccount tribiticaAccount)
        {
            if (ModelState.IsValid)
            {
                tribiticaAccount.ID = Guid.NewGuid();
                _context.Add(tribiticaAccount);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tribiticaAccount);
        }

        // GET: Account/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tribiticaAccount = await _context.Accounts.FindAsync(id);
            if (tribiticaAccount == null)
            {
                return NotFound();
            }
            return View(tribiticaAccount);
        }

        // POST: Account/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ID,Name,Email,SelfSummary")] TribiticaAccount tribiticaAccount)
        {
            if (id != tribiticaAccount.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tribiticaAccount);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TribiticaAccountExists(tribiticaAccount.ID))
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
            return View(tribiticaAccount);
        }

        // GET: Account/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tribiticaAccount = await _context.Accounts
                .FirstOrDefaultAsync(m => m.ID == id);
            if (tribiticaAccount == null)
            {
                return NotFound();
            }

            return View(tribiticaAccount);
        }

        // POST: Account/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var tribiticaAccount = await _context.Accounts.FindAsync(id);
            _context.Accounts.Remove(tribiticaAccount);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TribiticaAccountExists(Guid id)
        {
            return _context.Accounts.Any(e => e.ID == id);
        }
    }
}
