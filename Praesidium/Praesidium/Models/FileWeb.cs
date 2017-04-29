using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Praesidium.Models
{
    public class FileWeb
    {
        public class CheckModel
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public bool Checked { get; set; }
            public int FkNavId { get; set; }
        }

        public List<CheckModel> Cblist { get; set; }
        [UIHint("tinymce_jquery_full"), AllowHtml]
        public string Description { get; set; }
        public List<SelectListItem> Sections { get; set; }
        public List<SelectListItem> Uploader { get; set; }
    }
}