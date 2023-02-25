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
using System.IO.Compression;
using ICSharpCode.SharpZipLib.Tar;
using ICSharpCode.SharpZipLib.GZip;
namespace ForMyPhpApplict_site_application.Controllers
{
    [Authorize]
    public class AdminController:Controller
    {
        [HttpGet]
        public IActionResult Index() => AddFiles();
        public IActionResult AddFiles()
        {
            return SetDirectory(Path.Combine(BasePath.RootPath + "/", BasePath.UserFalesPath));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public   IActionResult LoadFiles(List<IFormFile> files,string dir)
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
                        html_content.AddFile.Add(f,dir);
                    }
                }
                return SetDirectory(dir);
            }
            else
            {
                Console.WriteLine("error");
               return Redirect("Error");
            }
            
        }
        [HttpGet]
        public IActionResult SetDirectory(string dir_name)
        {
            if (dir_name!=""&&Directory.Exists(dir_name))
            {
                ViewData["dir_name"] = (string)dir_name;
                return View("Index");
            }
            else return SetDirectory(Path.Combine(BasePath.RootPath+"/",  BasePath.UserFalesPath));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddDirectory(string dir_name,string new_dir)
        {
            if (Directory.Exists(dir_name))
            {
                Directory.CreateDirectory(Path.Combine(dir_name + "/", new_dir));
                
            }
            return SetDirectory(dir_name);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteFile(string fname,string dir_name)
        {
            if (System.IO.File.Exists(fname)&&fname.Contains(Path.Combine(BasePath.RootPath+"/",BasePath.UserFalesPath)))
            {
                System.IO.File.Delete(fname);
            }
            return SetDirectory(dir_name);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult GetBackup()
        {
            var dir = Path.Combine(BasePath.RootPath + "/" + BasePath.UserFalesPath);
            using(Stream cesh_drive=System.IO. File.OpenWrite("1backup.tar.gzip"))
            {
                using(GZipOutputStream ostr=new GZipOutputStream(cesh_drive,4096))
                {
                    using(TarArchive archive = TarArchive.CreateOutputTarArchive(ostr))
                    {
                        archive.RootPath = dir;
                        archive.AsciiTranslate = true;
                        TarEntry tarEntry = TarEntry.CreateEntryFromFile(dir);
                        
                        archive.WriteEntry(tarEntry, false);
                        string[] files = Directory.GetFiles(dir);
                        foreach(var file in files)
                        {
                            Console.WriteLine("add file:" + file);
                            tarEntry = TarEntry.CreateEntryFromFile(file);
                            archive.WriteEntry(tarEntry, true);
                        }
                        //ostr.Flush();
                        Console.WriteLine("close arhive");
                        archive.Close();
                    }
                    //ostr.Flush();
                    
                }
                if (cesh_drive.CanWrite == true)
                {
                    cesh_drive.Flush();
                    cesh_drive.Close();
                }
            }
            FileStreamResult r = null;
            using(Stream s = System.IO.File.OpenRead("1backup.tar.gzip"))
            {
                Console.WriteLine("read file"); 
                return File(s, "application/gzip", "backup.tar.gzip");

                // 

            }
            Console.WriteLine("delete file");
System.IO.File.Delete("1backup.tar.gzip");
             return r;
        }
    }
}
