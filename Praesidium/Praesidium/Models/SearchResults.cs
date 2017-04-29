using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Praesidium.Data_Context.Admin;
using Praesidium.Data_Models.Admin;

namespace Praesidium.Models
{
    public class SearchResult
    {
        public int RecId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DateUploaded { get; set; }
        public int Rank { get; set; }
        //public string DisplayText { get; set; }
        public string ShSyNavigationItem { get; set; }
        public int TotalRecords { get; set; }
    }

    public class SearchService
    {
        public List<SearchResult> Search(string searchTerm, int? page)
        {
            using (AdminEntities db = new AdminEntities())
            
            {
                var param1 = new SqlParameter("@SearchTerm", searchTerm);
                var param2 = new SqlParameter("@CurrentPage", page);
                var result = db.Database.SqlQuery<SearchResult>("Search @SearchTerm, @CurrentPage", param1, param2).ToList();
                return result;
            }
        }
    }
}