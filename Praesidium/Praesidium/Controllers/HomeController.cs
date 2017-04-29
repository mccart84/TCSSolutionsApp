using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Praesidium.Data_Models.Admin;
using System.Text.RegularExpressions;
using Praesidium.DAL;
using Praesidium.Models;

namespace Praesidium.Controllers
{
    public class HomeController : Controller
    {
        private AdminEntities db = new AdminEntities();
        public ActionResult Index()
        {
            var pageId = db.ShSyNavigationItems.FirstOrDefault(x => x.Controller == "Home" && x.Action == "Index");
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
            ViewBag.newFiles = Files.GetFilesByDate(4);
            return View();
        }
        public ActionResult CISTeam()
        {
            var pageId = db.ShSyNavigationItems.FirstOrDefault(x => x.Controller == "Home" && x.Action == "CISTeam");
            var model = new Models.Navigation.NavigationModel();
            var isActive = true;
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
        public ActionResult FileUpload()
        {
            var pageId = db.ShSyNavigationItems.FirstOrDefault(x => x.Controller == "Home" && x.Action == "FileUpload");
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
            var pageId = db.ShSyNavigationItems.FirstOrDefault(x => x.Controller == "Home" && x.Action == "About");
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
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            var pageId = db.ShSyNavigationItems.FirstOrDefault(x => x.Controller == "Home" && x.Action == "Contact");
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
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult DropDownPages()
        {
            var pageId = db.ShSyNavigationItems.FirstOrDefault(x => x.Controller == "Home" && x.Action == "DropDownPages");
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
            ViewBag.Message = "Pages By Section.";

            return View();
        }

        public PartialViewResult NavigationItems(bool? IsAdmin)
        {
            var model = new Models.Navigation.NavigationModel();

            if (IsAdmin == true)
            {
                var userId = (int)Session["User"];
                model.GetAdminNavItems(userId);
            }
            else
            {
                model.GetNavItems();
            }
            
            return PartialView("~/Views/Shared/_Navigation.cshtml", model);
        }

        public PartialViewResult FileItems(string FileId, int n)
        {
            var db = new AdminEntities();
            List<ShFile> files = new List<ShFile>();
            foreach (var t in db.ShFileKeywords.Where(m => m.Keyword == FileId))
            {
                files.Add((ShFile)db.ShFiles.Where(a => a.RecId == t.FkShFile));
            }
            files = (List<ShFile>)files.OrderBy(m => m.DateUploaded);
            files = (List<ShFile>) files.Take(n);

            return PartialView("~/Views/Shared/_FileItems.cshtml",files);
        }

        public ActionResult Search(string searchTerm, int? page)
        {
            if (!String.IsNullOrEmpty(searchTerm))
            {
                //searchTerm = "trafficking";
                page = page ?? 1;
                searchTerm = searchTerm = Regex.Replace(searchTerm, @"[^\sa-zA-Z]", string.Empty).Trim();
                var _service = new SearchService();
                ;
                var data = _service.Search(searchTerm, page);

                //var model = new SearchResult();
                //model.PageTitle = "Search";

                //model.KeyWords = searchTerm;
                //model.Results = data;
                //model.CurrentPage = page.Value;
                ViewBag.SearchString = searchTerm;
                return View(data);
            }
            else
            {
                return Index();
            }
        }
    }
}