using Praesidium.Data_Context;
using Praesidium.Models.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Praesidium.Data_Models;
using Praesidium.Data_Context.Admin;

namespace Praesidium.DAL
{
    public class NavigationItems : AdminContext
    {
        public List<NavItem> GetActiveNavigationItems()
        {
            var navItems = _context.ShSyNavigationItems.Where(x => x.IsActive && x.ParentId == null && x.ShSySection.IsActive == true && x.ShSySection.Name != "Admin").Select(x => new NavItem
            {
                RecId = x.RecId,
                Controller = x.Controller,
                Action = x.Action,
                DisplayText = x.DisplayText,
                FkShSySection = x.FkShSySection,
                Section = x.ShSySection.Name,
                SortOrder = x.SortOrder,
                IsActive = x.IsActive,
                SubPages = _context.ShSyNavigationItems.Where(s => s.IsActive && s.ParentId == x.RecId).OrderBy(s => s.SortOrder).Select(s => new NavSubItems
                {
                    RecId = s.RecId,
                    Controller = s.Controller,
                    Action = s.Action,
                    DisplayText = s.DisplayText,
                    FkShSySection = s.FkShSySection,
                    IsActive = s.IsActive,
                    ParentId = s.ParentId,
                    SortOrder = s.SortOrder
                }).ToList()
            }).OrderBy(x => x.SortOrder).ToList();

            return navItems;
        }

        public List<AdminNavItem> GetAdminNavItems(int userId)
        {
            var userRole = _context.ShUsers.Where(x => x.RecId == userId).Select(x => x.ShUserType.Type).FirstOrDefault();
            var adminItems = _context.ShSyNavigationItems.Where(x => x.IsActive && x.ShSySection.Name == "Admin").Select(x => new AdminNavItem
            {
                RecId = x.RecId,
                Controller = x.Controller,
                Action = x.Action,
                DisplayText = x.DisplayText,
                FkShSySection = x.FkShSySection,
                Section = x.ShSySection.Name,
                SortOrder = x.SortOrder,
                IsActive = x.IsActive,
            }).OrderBy(x => x.SortOrder).ToList();

            if (userRole == "Moderator")
            {
                var itemsToRemove = new List<AdminNavItem>();
                foreach (var adminNavItem in adminItems)
                {
                    if (adminNavItem.RecId == 22 || adminNavItem.RecId == 23)
                    {
                        itemsToRemove.Add(adminNavItem);
                    }
                }

                foreach (var i in itemsToRemove)
                {
                    adminItems.Remove(i);
                }
            }

            return adminItems;
        }

        public bool PageAvailable(int pageId)
        {
            var page = _context.ShSyNavigationItems.FirstOrDefault(x => x.RecId == pageId);
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