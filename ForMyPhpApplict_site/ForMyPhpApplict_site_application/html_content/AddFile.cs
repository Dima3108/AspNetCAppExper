using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
namespace ForMyPhpApplict_site_application.html_content
{
    public class AddFile
    {
        public static void Add(IFormFile f) => Add(f, Path.Combine(Directory.GetCurrentDirectory(), "html_content/"));
        public static void Add(IFormFile file,string dir)
        {
            string s = dir; //;

            using(Stream f = File.Open(System.IO.Path.Combine(s+"/",file.FileName), FileMode.OpenOrCreate, FileAccess.Write))
            {
                file.CopyTo(f);
            }
        }
    }
}
