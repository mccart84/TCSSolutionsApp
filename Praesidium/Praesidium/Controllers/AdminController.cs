using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Praesidium.Data_Models.Admin;
using PagedList;
using Praesidium.Models;



namespace Praesidium.Controllers
{
    public class AdminController : Controller
    {
        private AdminEntities db = new AdminEntities();

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoginValidation(string username, string password)
        {
            if (ModelState.IsValid)
            {
                var account = db.ShUsers.FirstOrDefault(x => x.Username == username && x.password == password);
                if (account != null)
                {
                    Session["User"] = account.RecId;
                    Session["Role"] = account.ShUserType.Type;
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Login");
        }

        public ActionResult Logout()
        {
            Session["User"] = null;
            return RedirectToAction("Login");
        }

        // GET: Admin
        public ActionResult Index()
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            var page = db.ShSyNavigationItems.FirstOrDefault(x => x.Controller == "Admin" && x.Action == "Index");
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

        #region [Navigation Items Admin]
        public ActionResult NavigationItems(string sortOrder, int? page)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            page = page == null ? 1 : page;

            ViewBag.SectionSortParm = sortOrder == "Section" ? "section_desc" : "Section";
            ViewBag.DisplayTextSortParm = sortOrder == "DisplayText" ? "displayText_desc" : "DisplayText";
            ViewBag.IsActiveSortParm = sortOrder == "IsActive" ? "isActive_desc" : "IsActive";

            var navItems = db.ShSyNavigationItems.Include(s => s.ShSySection);

            switch (sortOrder)
            {
                case "section_desc":
                    navItems = navItems.OrderByDescending(x => x.ShSySection.Name);
                    break;
                case "Section":
                    navItems = navItems.OrderBy(x => x.ShSySection.Name);
                    break;
                case "displayText_desc":
                    navItems = navItems.OrderByDescending(x => x.DisplayText);
                    break;
                case "DisplayText":
                    navItems = navItems.OrderBy(x => x.DisplayText);
                    break;
                case "isActive_desc":
                    navItems = navItems.OrderByDescending(x => x.IsActive);
                    break;
                case "IsActive":
                    navItems = navItems.OrderBy(x => x.IsActive);
                    break;
                default:
                    navItems = navItems.OrderBy(x => x.ShSySection.Name);
                    break;
            }

            var sections = db.ShSySections.ToList();

            ViewBag.Sections = sections;

            int pageSize = 9;
            int pageNumber = (page ?? 1);

            var pageId = db.ShSyNavigationItems.FirstOrDefault(x => x.Controller == "Admin" && x.Action == "NavigationItems");
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

            return View(navItems.ToPagedList(pageNumber, pageSize));
        }

        public JsonResult GetSelectedRecord(int? id)
        {
            var selectedItem = db.ShSyNavigationItems.Select(x => new
            {
                x.Controller,
                x.Action,
                x.DisplayText,
                x.IsActive,
                x.FkShSySection,
                x.ShSySection.Name,
                x.RecId,
                x.ParentId
            }).ToList();
            return Json(selectedItem);
        }

        public ActionResult NavigationItemsDetails(int? id)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShSyNavigationItem shSyNavigationItem = db.ShSyNavigationItems.Find(id);
            if (shSyNavigationItem == null)
            {
                return HttpNotFound();
            }
            return View(shSyNavigationItem);
        }

        public ActionResult NavigationItemsCreate()
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            ViewBag.FkShSySection = new SelectList(db.ShSySections, "RecId", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NavigationItemsCreate([Bind(Include = "RecId,Controller,Action,DisplayText,IsActive,FkShSySection,SortOrder,ParentId")] ShSyNavigationItem shSyNavigationItem)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (ModelState.IsValid)
            {
                db.ShSyNavigationItems.Add(shSyNavigationItem);
                db.SaveChanges();
                return RedirectToAction("NavigationItems");
            }

            ViewBag.FkShSySection = new SelectList(db.ShSySections, "RecId", "Name", shSyNavigationItem.FkShSySection);
            return View(shSyNavigationItem);
        }

        public ActionResult NavigationItemsEdit(int? id)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShSyNavigationItem shSyNavigationItem = db.ShSyNavigationItems.Find(id);
            if (shSyNavigationItem == null)
            {
                return HttpNotFound();
            }
            ViewBag.FkShSySection = new SelectList(db.ShSySections, "RecId", "Name", shSyNavigationItem.FkShSySection);
            return View(shSyNavigationItem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NavigationItemsEdit([Bind(Include = "RecId,Controller,Action,DisplayText,IsActive,FkShSySection,SortOrder,ParentId")] ShSyNavigationItem shSyNavigationItem)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (ModelState.IsValid)
            {
                db.Entry(shSyNavigationItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("NavigationItems");
            }
            ViewBag.FkShSySection = new SelectList(db.ShSySections, "RecId", "Name", shSyNavigationItem.FkShSySection);
            return View(shSyNavigationItem);
        }

        public ActionResult NavigationItemsDelete(int? id)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShSyNavigationItem shSyNavigationItem = db.ShSyNavigationItems.Find(id);
            if (shSyNavigationItem == null)
            {
                return HttpNotFound();
            }
            return View(shSyNavigationItem);
        }

        [HttpPost, ActionName("NavigationItemsDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult NavigationItemsDeleteConfirmed(int id)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            ShSyNavigationItem shSyNavigationItem = db.ShSyNavigationItems.Find(id);
            db.ShSyNavigationItems.Remove(shSyNavigationItem);
            db.SaveChanges();
            return RedirectToAction("NavigationItems");
        }

        #endregion

        #region[Sections]
        public ActionResult Sections(string sortOrder, int? page)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            page = page == null ? 1 : page;

            var sections = db.ShSySections.OrderBy(x => x.Name);

            int pageSize = 9;
            int pageNumber = (page ?? 1);

            var pageId = db.ShSyNavigationItems.FirstOrDefault(x => x.Controller == "Admin" && x.Action == "Sections");
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

            return View(sections.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult SectionsDetails(int? id)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShSySection shSySection = db.ShSySections.Find(id);
            if (shSySection == null)
            {
                return HttpNotFound();
            }
            return View(shSySection);
        }

        public ActionResult SectionsCreate()
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SectionsCreate([Bind(Include = "RecId,Name,IsActive")] ShSySection shSySection)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (ModelState.IsValid)
            {
                db.ShSySections.Add(shSySection);
                db.SaveChanges();
                return RedirectToAction("Sections");
            }

            return View(shSySection);
        }

        public ActionResult SectionsEdit(int? id)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShSySection shSySection = db.ShSySections.Find(id);
            if (shSySection == null)
            {
                return HttpNotFound();
            }
            return View(shSySection);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SectionsEdit([Bind(Include = "RecId,Name,IsActive")] ShSySection shSySection)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (ModelState.IsValid)
            {
                db.Entry(shSySection).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Sections");
            }
            return View(shSySection);
        }

        public ActionResult SectionsDelete(int? id)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShSySection shSySection = db.ShSySections.Find(id);
            if (shSySection == null)
            {
                return HttpNotFound();
            }
            return View(shSySection);
        }

        [HttpPost, ActionName("SectionsDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult SectionsDeleteConfirmed(int id)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            ShSySection shSySection = db.ShSySections.Find(id);
            db.ShSySections.Remove(shSySection);
            db.SaveChanges();
            return RedirectToAction("Sections");
        }


        #endregion

        #region[Users]
        public ActionResult Users(string sortOrder, int? page)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (Session["Role"] == "Moderator")
            {
                return RedirectToAction("Index");
            }
            page = page == null ? 1 : page;
            var users = db.ShUsers.OrderBy(x => x.Username);

            int pageSize = 9;
            int pageNumber = (page ?? 1);

            var pageId = db.ShSyNavigationItems.FirstOrDefault(x => x.Controller == "Admin" && x.Action == "Users");
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

            return View(users.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult UsersDetails(int? id)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (Session["Role"] == "Moderator")
            {
                return RedirectToAction("Index");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShUser shUser = db.ShUsers.Find(id);
            if (shUser == null)
            {
                return HttpNotFound();
            }
            return View(shUser);
        }

        public ActionResult UsersCreate()
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (Session["Role"] == "Moderator")
            {
                return RedirectToAction("Index");
            }
            ViewBag.FkShUserType = new SelectList(db.ShUserTypes, "RecId", "Type");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UsersCreate([Bind(Include = "RecId,FirstName,LastName,Username,email,FkShUserType,UserCreatedBy,UserUpdatedBy,UserDeletedBy,DateCreated,DateUpdated,DateDeleted,IsDeleted,password")] ShUser shUser)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (Session["Role"] == "Moderator")
            {
                return RedirectToAction("Index");
            }
            if (ModelState.IsValid)
            {
                db.ShUsers.Add(shUser);
                db.SaveChanges();
                return RedirectToAction("Users");
            }

            ViewBag.FkShUserType = new SelectList(db.ShUserTypes, "RecId", "Type", shUser.FkShUserType);
            return View(shUser);
        }

        public ActionResult UsersEdit(int? id)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (Session["Role"] == "Moderator")
            {
                return RedirectToAction("Index");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShUser shUser = db.ShUsers.Find(id);
            if (shUser == null)
            {
                return HttpNotFound();
            }
            ViewBag.FkShUserType = new SelectList(db.ShUserTypes, "RecId", "Type", shUser.FkShUserType);
            return View(shUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UsersEdit([Bind(Include = "RecId,FirstName,LastName,Username,email,FkShUserType,UserCreatedBy,UserUpdatedBy,UserDeletedBy,DateCreated,DateUpdated,DateDeleted,IsDeleted,password")] ShUser shUser)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (Session["Role"] == "Moderator")
            {
                return RedirectToAction("Index");
            }
            if (ModelState.IsValid)
            {
                db.Entry(shUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Users");
            }
            ViewBag.FkShUserType = new SelectList(db.ShUserTypes, "RecId", "Type", shUser.FkShUserType);
            return View(shUser);
        }

        public ActionResult UsersDelete(int? id)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (Session["Role"] == "Moderator")
            {
                return RedirectToAction("Index");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShUser shUser = db.ShUsers.Find(id);
            if (shUser == null)
            {
                return HttpNotFound();
            }
            return View(shUser);
        }

        [HttpPost, ActionName("UsersDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult UsersDeleteConfirmed(int id)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (Session["Role"] == "Moderator")
            {
                return RedirectToAction("Index");
            }
            ShUser shUser = db.ShUsers.Find(id);
            db.ShUsers.Remove(shUser);
            db.SaveChanges();
            return RedirectToAction("Users");
        }
        #endregion

        #region[User Types]

        public ActionResult UserTypes(string sortOrder, int? page)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            if (Session["Role"] == "Moderator")
            {
                return RedirectToAction("Index");
            }
            page = page == null ? 1 : page;

            var userTypes = db.ShUserTypes.ToList();

            int pageSize = 9;
            int pageNumber = (page ?? 1);

            var pageId = db.ShSyNavigationItems.FirstOrDefault(x => x.Controller == "Admin" && x.Action == "UserTypes");
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

            return View(userTypes.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult UserTypesDetails(int? id)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (Session["Role"] == "Moderator")
            {
                return RedirectToAction("Index");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShUserType shUserType = db.ShUserTypes.Find(id);
            if (shUserType == null)
            {
                return HttpNotFound();
            }
            return View(shUserType);
        }

        public ActionResult UserTypesCreate()
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (Session["Role"] == "Moderator")
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserTypesCreate([Bind(Include = "RecId,Type,UserCreatedBy,UserUpdatedBy,UserDeletedBy,DateCreated,DateUpdated,DateDeleted,IsDeleted")] ShUserType shUserType)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (Session["Role"] == "Moderator")
            {
                return RedirectToAction("Index");
            }
            if (ModelState.IsValid)
            {
                db.ShUserTypes.Add(shUserType);
                db.SaveChanges();
                return RedirectToAction("UserTypes");
            }

            return View(shUserType);
        }

        public ActionResult UserTypesEdit(int? id)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (Session["Role"] == "Moderator")
            {
                return RedirectToAction("Index");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShUserType shUserType = db.ShUserTypes.Find(id);
            if (shUserType == null)
            {
                return HttpNotFound();
            }

            ViewBag.Access = db.ShRfUserTypeAccesses.Where(x => x.FkShUserType == id).ToList();
            return View(shUserType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserTypesEdit([Bind(Include = "RecId,Type,UserCreatedBy,UserUpdatedBy,UserDeletedBy,DateCreated,DateUpdated,DateDeleted,IsDeleted")] ShUserType shUserType)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (Session["Role"] == "Moderator")
            {
                return RedirectToAction("Index");
            }
            if (ModelState.IsValid)
            {
                db.Entry(shUserType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("UserTypes");
            }
            return View(shUserType);
        }

        public ActionResult UserTypesDelete(int? id)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (Session["Role"] == "Moderator")
            {
                return RedirectToAction("Index");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShUserType shUserType = db.ShUserTypes.Find(id);
            if (shUserType == null)
            {
                return HttpNotFound();
            }
            return View(shUserType);
        }

        [HttpPost, ActionName("UserTypesDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (Session["Role"] == "Moderator")
            {
                return RedirectToAction("Index");
            }
            ShUserType shUserType = db.ShUserTypes.Find(id);
            db.ShUserTypes.Remove(shUserType);
            db.SaveChanges();
            return RedirectToAction("UserTypes");
        }

        #endregion

        // Addition for Site Documentation

        #region [Site Documentation Admin]
        public ActionResult Site_Documentation()
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            var pageId = db.ShSyNavigationItems.FirstOrDefault(x => x.Controller == "Admin" && x.Action == "Site_Documentation");
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

        #endregion

        #region [Files Admin]
        public ActionResult Files(string sortOrder, int? page)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            var pageId = db.ShSyNavigationItems.FirstOrDefault(x => x.Controller == "Admin" && x.Action == "Files");
            var model = new Models.Navigation.NavigationModel();
            var isActive = false;
            page = page == null ? 1 : page;
            int pageSize = 9;
            int pageNumber = (page ?? 1);
            if (pageId != null)
            {
                isActive = model.PageAvailable(pageId.RecId);
            }

            if (!isActive)
            {
                return RedirectToAction("Index", "Home");
            }
            var filelist = db.FileViews.OrderBy(m => m.DateUploaded); //.Take(5);
            return View(filelist.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult FilesCreate()
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            //FileWeb newfile = new FileWeb();
            //var navitems = db.ShSyNavigationItems.Where((m => (m.FkShSySection == 2) || (m.FkShSySection == 3)));
            var cblist = new List<FileWeb.CheckModel>();
            foreach (var t in db.ShSyNavigationItems.Where((m => (m.FkShSySection == 2) || (m.FkShSySection == 3))))
            {
                FileWeb.CheckModel cm = new FileWeb.CheckModel
                {
                    Checked = false,
                    Id = t.RecId,
                    Name = t.DisplayText,
                    FkNavId = t.FkShSySection
                };
                cblist.Add(cm);
            }
            var model = new ShFile
            {
                Sections = cblist
            };
            ViewBag.sections = new SelectList(db.ShSySections.Where(m => ((bool)m.IsActive) && (m.RecId != 1)), "RecID", "Name");

            ViewBag.users = new SelectList(db.ShUsers, "RecID", "Username");

            return View(model);
        }

        [HttpPost]
        //public ActionResult FilesCreate(ShFile model, HttpPostedFileBase upload)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            if (upload != null && upload.ContentLength > 0)
        //            {

        //                model.FileName = upload.FileName;
        //                using (var reader = new System.IO.BinaryReader(upload.InputStream))
        //                {
        //                    model.FileStore = reader.ReadBytes(upload.ContentLength);
        //                }
        //                model.ContentType = upload.ContentType;
        //                model.DateUploaded = DateTime.Now;

        //                db.ShFiles.Add(model);
        //                db.SaveChanges();

        //                var navitems = db.ShSyNavigationItems.Where((m => (m.FkShSySection == 2) || (m.FkShSySection == 3)));
        //                model.ShFileKeywords = new List<ShFileKeyword>();



        //                db.SaveChanges();
        //            }

        //            return RedirectToAction("Files");
        //        }
        //    }
        //    catch (Exception ex /* dex */)
        //    {
        //        throw ex;
        //    }

        //    return View();
        //}
        public ActionResult FilesCreate(FormCollection collection, HttpPostedFileBase upload)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            try
            {
                if (ModelState.IsValid)
                {
                    if (upload != null && upload.ContentLength > 0)
                    {
                        ShFile model = new ShFile();
                        TryUpdateModel(model);

                        model.FileName = upload.FileName;
                        model.Description = Server.HtmlDecode(Request.Form["Description"]);
                        using (var reader = new System.IO.BinaryReader(upload.InputStream))
                        {
                            model.FileStore = reader.ReadBytes(upload.ContentLength);
                        }
                        model.ContentType = upload.ContentType;
                        model.UploadedBy = Convert.ToInt32(Session["User"]);
                        model.DateUploaded = DateTime.Now;

                        db.ShFiles.Add(model);
                        db.SaveChanges();
                        string[] blah = Request.Form["SectionList"].Split(',');
                        foreach (var c in blah)
                        {
                            ShFileKeyword fk = new ShFileKeyword();
                            fk.FkShFile = model.RecId;
                            fk.Keyword = c;
                            db.ShFileKeywords.Add(fk);
                        }
                        
                        db.SaveChanges();
                    }

                    return RedirectToAction("Files");
                }
            }
            catch (Exception ex /* dex */)
            {
                throw ex;
            }

            return View();
        }

        public ActionResult FilesEdit(int? id)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var file = db.ShFiles.Find(id);



            if (file == null)
                return HttpNotFound();

            ViewBag.section = new SelectList(db.ShSySections, "RecID", "Name", file.FkShSySection);
            ViewBag.uploadusers = file.UploadedBy != null ? new SelectList(db.ShUsers, "RecID", "Username", file.UploadedBy) : new SelectList(db.ShUsers, "RecID", "Username");
            ViewBag.modusers = file.ModifiedBy != null ? new SelectList(db.ShUsers, "RecID", "Username", file.ModifiedBy) : new SelectList(db.ShUsers, "RecID", "Username");
            return View(file);
        }

        [HttpPost]
        public ActionResult FilesEdit(ShFile model, HttpPostedFileBase upload)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            try
            {
                if (ModelState.IsValid)
                {
                    if (upload != null && upload.ContentLength > 0)
                    {

                        model.FileName = upload.FileName;
                        using (var reader = new System.IO.BinaryReader(upload.InputStream))
                        {
                            model.FileStore = reader.ReadBytes(upload.ContentLength);
                        }
                        model.ContentType = upload.ContentType;
                        model.DateUploaded = DateTime.Now;

                        db.ShFiles.Add(model);
                        db.SaveChanges();
                    }

                    return RedirectToAction("Files");
                }
            }
            catch (Exception ex /* dex */)
            {
                throw ex;
            }

            return View();
        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}