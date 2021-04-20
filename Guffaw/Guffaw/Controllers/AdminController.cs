using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Guffaw.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private string ClassName = typeof(AdminController).Name;

        public AdminController()
        {

        }

        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

    }
}