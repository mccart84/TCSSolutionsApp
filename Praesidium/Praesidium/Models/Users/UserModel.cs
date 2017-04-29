using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Praesidium.Models.Users
{
    public class UserItem
    {
        public int RecId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string email { get; set; }
        public int? FkShUserType { get; set; }
        public bool? IsDeleted { get; set; }
    }
    

    public class UserModel
    {
        public List<UserItem> UserItem;

        public List<UserItem> GetUserItems()
        {
            using (var biz = new DAL.Admin.UserItems())
            {
                return UserItem = biz.GetUserItems().ToList();
            }
        }
    }
}