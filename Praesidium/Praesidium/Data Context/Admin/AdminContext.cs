using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Praesidium.Data_Models.Admin;

namespace Praesidium.Data_Context.Admin
{
    public abstract class AdminContext : IDisposable
    {      
        protected AdminEntities _context { get; set; }

        public AdminContext(AdminEntities context)
        {
            _context = context;
        }

        public AdminContext()
        {
            _context = new AdminEntities();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}