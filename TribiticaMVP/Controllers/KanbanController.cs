using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TribiticaMVP.Models;

namespace TribiticaMVP.Controllers
{
    public class KanbanController : Controller
    {
        // GET: Kanban
        [HttpGet]
        public ActionResult Index()
        {
            HttpContext.Session.TryGetValue("UserID", out var userGuidBytes);
            if (userGuidBytes == null)
                return RedirectToAction("Login", "Account");
            return View();
        }

        // GET: Kanban/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Kanban/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Kanban/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Kanban/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Kanban/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Kanban/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Kanban/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}