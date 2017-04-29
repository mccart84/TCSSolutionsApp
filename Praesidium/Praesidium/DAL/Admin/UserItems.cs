using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Praesidium.Data_Context.Admin; // References the context class for Admin Context
using Praesidium.Models.Users; // References the model for this page

namespace Praesidium.DAL.Admin
{
    public class UserItems : AdminContext // Inherit from the context class
    {
        public IQueryable<UserItem> GetUserItems()
        {
            var UserItems = _context.ShUsers.Where(x => x.IsDeleted != null).Select(x => new UserItem
            {
                RecId = x.RecId,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Username = x.Username,
                email = x.email,
                FkShUserType = x.FkShUserType,
                IsDeleted = x.IsDeleted
            }).OrderBy(x => x.RecId);

            return UserItems;
        }
    }
}