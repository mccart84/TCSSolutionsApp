using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Praesidium.Data_Context.Admin; // References the context class for Admin Context
using Praesidium.Models; // References the model for this page

namespace Praesidium.DAL.Admin
{
    public class REFERENCE : AdminContext // Inherit from the context class
    {
        //basic get statement
    //    public List<object> GetItems()
    //    {
    //        var items = _context.TABLENAME.Where(x => x.PROPERTY == true && x.PROPERTY == "someText").Select(new CLASSFROMMODEL
    //        {
    //            x.ITEM,
    //            x.ITEM
    //        }).ToList();
    //    }
    //
    //    return items
    }
}