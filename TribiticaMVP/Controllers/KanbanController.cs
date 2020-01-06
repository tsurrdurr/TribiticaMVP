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
    }
}