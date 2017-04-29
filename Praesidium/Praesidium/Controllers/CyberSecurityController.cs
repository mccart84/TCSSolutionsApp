using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Praesidium.Data_Models.Admin;
using Praesidium.DAL;

namespace Praesidium.Controllers
{
    public class CyberSecurityController : Controller
    {
        private AdminEntities db = new AdminEntities();
        // GET: CyberSecurity
        public ActionResult Index()
        {
            var pageId = db.ShSyNavigationItems.FirstOrDefault(x => x.Controller == "CyberSecurity" && x.Action == "Index");
            var model = new Models.Navigation.NavigationModel();
            var isActive = false;
            if (pageId != null)
            {
                isActive = model.PageAvailable(pageId.RecId);
            }

            if (!isActive)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public ActionResult About()
        {
            var pageId = db.ShSyNavigationItems.FirstOrDefault(x => x.Controller == "CyberSecurity" && x.Action == "About");
            var model = new Models.Navigation.NavigationModel();
            var isActive = false;
            if (pageId != null)
            {
                isActive = model.PageAvailable(pageId.RecId);
            }

            if (!isActive)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public ActionResult Resources()
        {
            var pageId = db.ShSyNavigationItems.FirstOrDefault(x => x.Controller == "CyberSecurity" && x.Action == "Resources");
            var model = new Models.Navigation.NavigationModel();
            var isActive = false;
            if (pageId != null)
            {
                isActive = model.PageAvailable(pageId.RecId);
            }

            if (!isActive)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.files = Files.GetFilesBySection(3).OrderByDescending(x => x.ViewCount);
            return View();
        }

        public ActionResult Adults()
        {
            var pageId = db.ShSyNavigationItems.FirstOrDefault(x => x.Controller == "CyberSecurity" && x.Action == "Adults");
            var model = new Models.Navigation.NavigationModel();
            var isActive = false;
            if (pageId != null)
            {
                isActive = model.PageAvailable(pageId.RecId);
            }

            if (!isActive)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public ActionResult Teachers()
        {
            var pageId = db.ShSyNavigationItems.FirstOrDefault(x => x.Controller == "CyberSecurity" && x.Action == "Teachers");
            var model = new Models.Navigation.NavigationModel();
            var isActive = false;
            if (pageId != null)
            {
                isActive = model.PageAvailable(pageId.RecId);
            }

            if (!isActive)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public ActionResult Teen()
        {
            var pageId = db.ShSyNavigationItems.FirstOrDefault(x => x.Controller == "CyberSecurity" && x.Action == "Teen");
            var model = new Models.Navigation.NavigationModel();
            var isActive = false;
            if (pageId != null)
            {
                isActive = model.PageAvailable(pageId.RecId);
            }

            if (!isActive)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public ActionResult Youth()
        {
            var pageId = db.ShSyNavigationItems.FirstOrDefault(x => x.Controller == "CyberSecurity" && x.Action == "Youth");
            var model = new Models.Navigation.NavigationModel();
            var isActive = false;
            if (pageId != null)
            {
                isActive = model.PageAvailable(pageId.RecId);
            }

            if (!isActive)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public ActionResult Statistics()
        {
            var pageId = db.ShSyNavigationItems.FirstOrDefault(x => x.Controller == "CyberSecurity" && x.Action == "Statistics");
            var model = new Models.Navigation.NavigationModel();
            var isActive = false;
            if (pageId != null)
            {
                isActive = model.PageAvailable(pageId.RecId);
            }

            if (!isActive)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public ActionResult LawEnforcement()
        {
            var pageId = db.ShSyNavigationItems.FirstOrDefault(x => x.Controller == "CyberSecurity" && x.Action == "LawEnforcement");
            var model = new Models.Navigation.NavigationModel();
            var isActive = false;
            if (pageId != null)
            {
                isActive = model.PageAvailable(pageId.RecId);
            }

            if (!isActive)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public ActionResult Quiz(int? quizId)
        {
            var pageId = db.ShSyNavigationItems.FirstOrDefault(x => x.Controller == "CyberSecurity" && x.Action == "Quiz");
            var model = new Models.Navigation.NavigationModel();
            var isActive = false;
            if (pageId != null)
            {
                isActive = model.PageAvailable(pageId.RecId);
            }

            if (!isActive)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.QuizId = quizId;
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
                    file.Description = HttpUtility.HtmlDecode(file.Description);
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
            return PartialView("~/Views/Shared/_FileItems.cshtml", fileList);
        }
    }
}