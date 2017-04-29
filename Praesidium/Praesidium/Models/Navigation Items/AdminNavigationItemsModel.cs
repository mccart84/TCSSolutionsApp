using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Praesidium.Models.NavigationItems
{
    public class AdminNavItems
    {
        public int RecId { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string DisplayText { get; set; }
        public bool IsActive { get; set; }
        public int FkShSySection { get; set; }
        public int SortOrder { get; set; }
        public int ParentId { get; set; }
    }

    
    public class NavigationItemsModel
    {
        public List<AdminNavItems> AdminNavItems;

        public List<AdminNavItems> GetNavigationItems()
        {
            using (var biz = new DAL.Admin.AdminNavigationItems())
            {
                return AdminNavItems = biz.GetNavigationItems().ToList();
            }
        }
    }
}