using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ForMyPhpApplict_site_application.Model;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
namespace ForMyPhpApplict_site_application.Data
{
    public class ServerDirectories
    {
        public static bool ValidName(string name)
        {
            foreach (string s in path_chars)
                if (name.Contains(s))
                    return false;
            return true;
        }
        public static string SafeName(string name)
        {
            string s = "";
            int pos = name.Length-1;
            while (name[pos].ToString() != @"\" && name[pos].ToString() != "/" && name[pos].ToString() != "|")
                pos--;
            pos += 1;
            for (; pos < name.Length; pos++)
                s += name[pos].ToString();
            return s;
        }
        public static string GetRootDirectory(string dir)
        {
            if (Path.Combine(dir) == Path.Combine(BasePath.RootPath + "/", BasePath.UserFalesPath))
                return "";
            int p = dir.Length - 1;
            while (dir[p].ToString() != @"\" && dir[p].ToString() != "/" && dir[p].ToString() != "|")
                p--;
            p--;
            string v = "";
            for (int i = 0; i < p; i++)
                v += dir[p].ToString();
            return v;
        }
        public static readonly string[]path_chars= { @"//", @"\\", @"||", @"\", @"|" };
        public static string ToWebPath(string path)
        {
            string[] appf = path_chars;
            string e = Path.Combine(BasePath.RootPath + "/", "");
            string p = path.Replace(e, "");
            for(int i = 0; i < appf.Length; i++)
            {
                string v = p.Replace(appf[i], @"/");
                p = v;
            }
            return "../"+ p;
        }
        public static DirectoryModel GetDirectory(string dir)
        {
            if (Directory.Exists(dir))
            {
                string[] ldir = Directory.GetDirectories(dir);
                string[] fname = Directory.GetFiles(dir);
                DirectoryModel model = new DirectoryModel
                {
                    root_dir = GetRootDirectory(dir),
                    directories = ldir,
                    name = dir,
                    files = fname,
                    safename = SafeName(dir),
                    safe_directory_name = new string[ldir.Length],
                    safe_file_names = new string[fname.Length]
                };
                for (int i = 0; i < model.safe_file_names.Length; i++)
                    model.safe_file_names[i] = SafeName(model.files[i]);
                for (int l = 0; l < model.safe_directory_name.Length; l++)
                    model.safe_directory_name[l] = SafeName(model.directories[l]);
                return model;
            }
            return null;
        }
        public static Task AddFile(IFormFile file,string dir)
        {
            if (Directory.Exists(dir))
            {
                return Task.Run(delegate { 
string f_n = Path.Combine(dir + "/", file.FileName);
                    if (File.Exists(f_n))
                    {
                        File.Delete(f_n);
                    }
                using (Stream f = File.Open(f_n, FileMode.Create, FileAccess.Write, FileShare.Read))
                {
                     file.CopyTo(f);
                }
                });
                
            }
            else return new Task(delegate { Console.WriteLine($"direcoty not found!{dir}"); });
        }
    }
}
