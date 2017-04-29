using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Praesidium.Data_Models.Admin;
using Praesidium.Models;

namespace Praesidium.Controllers
{
    public class LoginController : Controller
    {
        private AdminEntities db = new AdminEntities();

        public ActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoginValidation(LoginViewModel login)
        {
            if (ModelState.IsValid)
            {
                var account = db.ShUsers.FirstOrDefault(x => x.Username == login.Username && x.password == login.Password);
                if (account != null)
                {
                    Session["User"] = account.RecId;
                    return RedirectToAction("Index", "Admin");
                }
            }
            return RedirectToAction("Login");
        }

        public ActionResult Logout()
        {
            Session["User"] = null;
            return RedirectToAction("Login");
        }
    }
}