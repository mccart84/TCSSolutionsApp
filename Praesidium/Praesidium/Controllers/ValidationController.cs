using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Praesidium.Data_Models.Admin;

namespace Praesidium.Controllers
{
    public class ValidationController : Controller
    {
        private AdminEntities db = new AdminEntities();
        public bool PageAvailable(int pageId)
        {
            var page = db.ShSyNavigationItems.FirstOrDefault(x => x.RecId == pageId);
            if (page != null)
            {
                if (page.IsActive)
                {
                    return true;
                }
                return false;
            }
            return false;
        }
    }
}