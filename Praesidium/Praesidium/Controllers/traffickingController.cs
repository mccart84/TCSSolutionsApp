using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using Praesidium.Data_Models.Admin;
using Praesidium.DAL;

namespace Praesidium.Controllers
{
    public class TraffickingController : Controller
    {
        private AdminEntities db = new AdminEntities();
        // GET: Trafficking
        public ActionResult Index()
        {
            var page = db.ShSyNavigationItems.FirstOrDefault(x => x.Controller == "Trafficking" && x.Action == "Index");
            var model = new Models.Navigation.NavigationModel();
            var isActive = false;
            if (page != null)
            {
                isActive = model.PageAvailable(page.RecId);
            }

            if (!isActive)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();

        }

        public ActionResult Resources()
        {
            var page = db.ShSyNavigationItems.FirstOrDefault(x => x.Controller == "Trafficking" && x.Action == "Resources");
            var model = new Models.Navigation.NavigationModel();
            var isActive = false;
            if (page != null)
            {
                isActive = model.PageAvailable(page.RecId);
            }

            if (!isActive)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.files = Files.GetFilesBySection(2).OrderByDescending(x => x.ViewCount);
            return View();
        }

        public ActionResult Statistics()
        {
            var page = db.ShSyNavigationItems.FirstOrDefault(x => x.Controller == "Trafficking" && x.Action == "Statistics");
            var model = new Models.Navigation.NavigationModel();
            var isActive = false;
            if (page != null)
            {
                isActive = model.PageAvailable(page.RecId);
            }

            if (!isActive)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public ActionResult Adults()
        {
            var page = db.ShSyNavigationItems.FirstOrDefault(x => x.Controller == "Trafficking" && x.Action == "Adults");
            var model = new Models.Navigation.NavigationModel();
            var isActive = false;
            if (page != null)
            {
                isActive = model.PageAvailable(page.RecId);
            }

            if (!isActive)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public ActionResult Teachers()
        {
            var page = db.ShSyNavigationItems.FirstOrDefault(x => x.Controller == "Trafficking" && x.Action == "Teachers");
            var model = new Models.Navigation.NavigationModel();
            var isActive = false;
            if (page != null)
            {
                isActive = model.PageAvailable(page.RecId);
            }

            if (!isActive)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public ActionResult Teen()
        {
            var page = db.ShSyNavigationItems.FirstOrDefault(x => x.Controller == "Trafficking" && x.Action == "Teen");
            var model = new Models.Navigation.NavigationModel();
            var isActive = false;
            if (page != null)
            {
                isActive = model.PageAvailable(page.RecId);
            }

            if (!isActive)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public ActionResult Youth()
        {
            var page = db.ShSyNavigationItems.FirstOrDefault(x => x.Controller == "Trafficking" && x.Action == "Youth");
            var model = new Models.Navigation.NavigationModel();
            var isActive = false;
            if (page != null)
            {
                isActive = model.PageAvailable(page.RecId);
            }

            if (!isActive)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public ActionResult WarningSigns()
        {
            var page = db.ShSyNavigationItems.FirstOrDefault(x => x.Controller == "Trafficking" && x.Action == "WarningSigns");
            var model = new Models.Navigation.NavigationModel();
            var isActive = false;
            if (page != null)
            {
                isActive = model.PageAvailable(page.RecId);
            }

            if (!isActive)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public List<FileView> GetFilesByCategory(int catId)
        {
            AdminEntities db = new AdminEntities();
            var files = new List<FileView>();
            try
            {
                var catfiles = db.ShFileKeywords.Where(m => m.Keyword == catId.ToString());

                foreach (var t in catfiles)
                {
                    FileView file = db.FileViews.First(u => u.RecId == t.FkShFile);
                    files.Add(file);
                }

                return files.OrderBy(m => m.DownloadCount).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        public PartialViewResult FileList(int id)
        {
            var fileList = GetFilesByCategory(id);
            return PartialView("~/Views/Shared/_FileItems.cshtml",fileList);
        }
    }
}