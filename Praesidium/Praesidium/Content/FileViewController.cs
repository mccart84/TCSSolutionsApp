using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Praesidium.Content
{
    public class FileViewController : Controller
    {
        // GET: FileView
        public ActionResult Index()
        {
            return View();
        }
    }
}