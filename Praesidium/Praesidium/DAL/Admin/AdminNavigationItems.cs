using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Praesidium.Data_Context.Admin; // References the context class for Admin Context
using Praesidium.Models.NavigationItems;// References the model for this page

namespace Praesidium.DAL.Admin
{
    public class AdminNavigationItems : AdminContext // Inherit from the context class
    {
        //basic get statement
       
        public IQueryable<AdminNavItems> GetNavigationItems()
        {
            var navItems = _context.ShSyNavigationItem.Where(x => x.RecId == true).Select(x =>new AdminNavItems
            {
                RecId= x.RecId,
                Controller = x.Controller,
                Action = x.Action,
                DisplayText = x.DisplayText,
                IsActive = x.IsActive,
                FkShSySection = x.FkShSySection,
                SortOrder = x.SortOrder,
                ParentId = x.ParentId
            }).ToList();

            return navItems;
        }
    }
}