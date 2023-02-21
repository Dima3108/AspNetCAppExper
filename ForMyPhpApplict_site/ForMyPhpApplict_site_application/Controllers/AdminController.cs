using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Web;
using Microsoft.AspNetCore;
using ForMyPhpApplict_site_application.Data;
using Microsoft.AspNetCore.Authorization;
namespace ForMyPhpApplict_site_application.Controllers
{
    [Authorize]
    public class AdminController:Controller
    {
        [HttpGet]
        public IActionResult Index() => AddFiles();
        public IActionResult AddFiles()
        {
            return View("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public   IActionResult LoadFiles(List<IFormFile> files)
        {
            Console.WriteLine(Directory.Exists("html_content"));
            Console.WriteLine(files.Count);
            
            if (files.Count <= 20 && files.Count > 0)
            {
                foreach (var f in files)
                {
                    if (f.Length <= 1024 * 1024 * 2)
                    {
                        /*using (Stream s = System.IO.File.Open(Path.Combine(BasePath.RootPath, "html_content")+"/" + f.FileName, FileMode.OpenOrCreate, FileAccess.Write))
                        {
                             f.CopyTo(s);
                        }*/
                        html_content.AddFile.Add(f);
                    }
                }
                return AddFiles();
            }
            else
            {
                Console.WriteLine("error");
               return Redirect("Error");
            }
            
        }
    }
}
