using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForMyPhpApplict_site_application.Data
{
    public class BasePath
    {
        public static string RootPath { get; private set; }
        public const string UserFalesPath = "html_content";
        public static void SetPath(string p)
        {
            if (RootPath != null)
                lock (RootPath)
                {
                    RootPath = p;
                }
            else RootPath = p;
        }
    }
}
