using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Praesidium.Data_Models.Admin;

namespace Praesidium.DAL
{
    public class Users
    {
        private static AdminEntities db = new AdminEntities();

        public static List<ShUser> GetUsers()
        {
            var userList = db.ShUsers;
            if (userList != null)
                return userList.ToList();
            else
            {
                return null;
            }
        }

        public static ShUser GetUserById(int id)
        {
            var user = db.ShUsers.FirstOrDefault(x => x.RecId == id);
            if (user != null)
                return user;
            else
            {
                return null;
            }
        }

        public static string GetUserNameById(int id)
        {
            var user = GetUserById(id);
            if (user != null)
            {
                var username = user.FirstName + " " + user.LastName;
                return username;
            }
            else
            {
                return "No User Account Found";
            }
        }
    }
}