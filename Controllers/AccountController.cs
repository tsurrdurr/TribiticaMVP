using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TribiticaMVP.Models;
using TribiticaMVP.Models.Account;

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
        public async Task<IActionResult> Create([Bind("ID,Name,Password,Email,SelfSummary")] TribiticaAccount tribiticaAccount)
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
        public async Task<IActionResult> Edit(Guid id, [Bind("ID,Name,Password,Email,SelfSummary")] TribiticaAccount tribiticaAccount)
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

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginDetails loginDetails)
        {
            if (ModelState.IsValid)
            {
                using (var dbContext = new TribiticaDbContext())
                {
                    var user = dbContext
                        .Accounts
                        .Where(a => a.Name == loginDetails.NameProvided
                        || a.Email == loginDetails.NameProvided).FirstOrDefault();

                    if (user == null)
                        throw new KeyNotFoundException("User not found.");

                    if (user.Password != loginDetails.Password)
                        throw new InvalidOperationException("Password provided was incorrect.");

                    if (user != null)
                    {
                        HttpContext.Session.SetString("UserID", user.ID.ToString());
                        HttpContext.Session.SetString("UserName", user.Name);
                    }
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult Logout()
        {
            HttpContext.Session.Remove("UserID");
            HttpContext.Session.Remove("UserName");
            return View();
        }
    }
}
