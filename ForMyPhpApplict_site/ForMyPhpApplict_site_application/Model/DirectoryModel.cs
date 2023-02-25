using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForMyPhpApplict_site_application.Model
{
    public class DirectoryModel
    {
        public string root_dir { get; set; }
        public string name { get; set; }
        public string safename { get; set; }
        public string[] files { get; set; }
        public string[] safe_file_names { get; set; }
        public string[] directories { get; set; }
        public string[] safe_directory_name { get; set; }

    }
}
