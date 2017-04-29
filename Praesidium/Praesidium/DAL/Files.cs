using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Praesidium.Data_Models.Admin;

namespace Praesidium.DAL
{
    public class Files
    {
        private static AdminEntities db = new AdminEntities();

        public static List<FileView> GetFiles()
        {
            var filelist = db.FileViews;
            if (filelist != null)
            {
                return filelist.ToList();
            }
            else
            {
                return null;
            }
            
        }

        public static List<FileView> GetFiles(int i)
        {
            var filelist = db.FileViews.Take(i);
            if (filelist != null)
            {
                return filelist.ToList();
            }
            else
            {
                return null;
            }
        }
        public static List<FileView> GetFilesByDate(int i)
        {
            var filelist = db.FileViews.OrderByDescending(x => x.DateUploaded).Take(i);
            if (filelist != null)
            {
                return filelist.ToList();
            }
            else
            {
                return null;
            }
        }

        public static FileView GetFile(int id)
        {
            var file = db.FileViews.FirstOrDefault(x => x.RecId == id);
            if (file != null)
            {
                return file;
            }
            else
            {
                return null;
            }
        }

        public static bool FileExists(int id)
        {
            var file = GetFile(id);
            if (file == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static List<FileView> GetFilesBySection(int id)
        {
            List<FileView> files = new List<FileView>();
            if ((db.ShSySections.FirstOrDefault(x => x.RecId == id))!=null)
            {
                List<ShSyNavigationItem> categories = db.ShSyNavigationItems.Where(x => x.ParentId == id).ToList();
                foreach (var t in categories)
                {
                    var fileids = db.ShFileKeywords.Where(x => x.Keyword.Contains(t.RecId.ToString()));
                    foreach (var y in fileids)
                    {
                        files.Add(Files.GetFile(y.FkShFile.Value));
                    }
                }
                return files.Distinct().ToList();
            }
            else
            {
                return null;
            }
        }
    }
}