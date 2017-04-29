using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Praesidium.Models.Navigation
{
    public class NavItem
    {
        public int RecId { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string DisplayText { get; set; }
        public bool IsActive { get; set; }
        public int ParentId { get; set; }
        public int FkShSySection { get; set; }
        public int SortOrder { get; set; }
        public string Section { get; set; }
        public List<NavSubItems> SubPages { get; set; }
    }

    public class NavSubItems
    {
        public int RecId { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string DisplayText { get; set; }
        public bool IsActive { get; set; }
        public int? ParentId { get; set; }
        public int FkShSySection { get; set; }
        public int SortOrder { get; set; }
    }

    public class AdminNavItem
    {
        public int RecId { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string DisplayText { get; set; }
        public bool IsActive { get; set; }
        public int ParentId { get; set; }
        public int FkShSySection { get; set; }
        public int SortOrder { get; set; }
        public string Section { get; set; }
    }

    public class NavigationModel
    {
        public List<NavItem> NavItems;
        public List<AdminNavItem> AdminNavItems;

        public List<NavItem> GetNavItems()
        {
            using (var biz = new DAL.NavigationItems())
            {
                return NavItems = biz.GetActiveNavigationItems().ToList();
            }
        }

        public List<AdminNavItem> GetAdminNavItems(int user)
        {
            using (var biz = new DAL.NavigationItems())
            {
                return AdminNavItems = biz.GetAdminNavItems(user).ToList();
            }
        }

        public bool PageAvailable(int pageId)
        {
            using (var biz = new DAL.NavigationItems())
            {
                return biz.PageAvailable(pageId);
            }
        }

        public bool PageAvailable(int pageId)
        {
            using (var biz = new DAL.NavigationItems())
            {
                return biz.PageAvailable(pageId);
            }
        }
    }
}