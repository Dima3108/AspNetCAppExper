using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using Microsoft.AspNetCore.Http;
using Pchp.Core;
using ForMyPhpApplict_site_application.Data;
using System.IO;
using System.Text;
namespace ForMyPhpApplict_site_application.MiddleWare
{
    public class PhpFileDetectMiddleware
    {
        private readonly RequestDelegate next;
        public PhpFileDetectMiddleware(RequestDelegate next_)
        {
            next = next_;
        }
       /* static readonly string[] AdditionalReferences = new[]
        {
            typeof(Peachpie.Library.Graphics.PhpGd2).Assembly.Location,
            typeof(Peachpie.Library.PDO.PDO).Assembly.Location,
            typeof(Peachpie.Library.PDO.Sqlite.PDOSqliteDriver).Assembly.Location,
            typeof(Peachpie.Library.Scripting.Standard).Assembly.Location,
            typeof(Peachpie.Library.XmlDom.XmlDom).Assembly.Location,
            typeof(Peachpie.Library.Network.CURLFunctions).Assembly.Location,
            
        };*/
        public async Task Invoke(HttpContext context)
        {
            await Task.Run(delegate {
                var path = context.Request.Path.ToString();
            //if(path.Length>4)
                if (path.Contains( ".php"))
                {
                string result = "";
                bool succes = true;
                context.Response.Headers.Append("Content-Type", "text/html; charset=utf-8");
                string s = path[path.Length - 4].ToString() + path[path.Length - 3].ToString() + path[path.Length - 2].ToString() + path[path.Length - 1].ToString();
                if (s == ".php")
                {
                    context.Response.StatusCode = 200;
                    int pos = path.Length - 5;
                    while (pos >= 0 && path[pos].ToString() != "/" && path[pos].ToString() != @"\")
                        pos--;
                    pos++;
                    string f_name = "";
                    for (; pos < path.Length - 4; pos++)
                        f_name += path[pos].ToString();
                    f_name += s;
                    var cur_dir = Path.Combine(BasePath.RootPath, BasePath.UserFalesPath);
                    
                    
                    if (File.Exists(Path.Combine(cur_dir+"/", f_name)) == false)
                    {
                        Console.WriteLine(f_name+"\n\n\n\n");
                        result = "Не найден указанный файл, проверте корректность имени файла";
                        succes = false;
                    }
                    else
                    {
                        
                        try
                        {

                                /* var AR = AdditionalReferences.ToList();


                                 using (MemoryStream cesh_s = new MemoryStream()) {
                                     using (Context php_context = Context.CreateConsole(String.Empty,typeof(Program).Assembly.Location))
                                     {

                                         var srcipt = Context.DefaultScriptingProvider.CreateScript(new Context.ScriptOptions
                                         {
                                             Context = php_context,
                                             Location = new Location(Path.Combine(cur_dir + "/", f_name), 0, 0),
                                             EmitDebugInformation = true,
                                             IsSubmission = false,
                                             AdditionalReferences =AR.ToArray() ,
                                         }, File.ReadAllText(Path.Combine(cur_dir+"/", f_name)));
                                          srcipt.Evaluate(php_context, php_context.Globals,null);

                                     }
                                     result = UTF8Encoding.UTF8.GetString(cesh_s.ToArray());
                                 }*/
                                next(context).Wait();
                                return;
                        }
                        catch(Exception e)
                        {
                            succes = false;
                            result = e.Message;
                        }
                    }
                    Task t = Task.Run( delegate {
  if (succes)
                            {
                            //writer.WriteLine(result);
                            context.Response.WriteAsync(result).Wait(); ;
                            }
                            else
                            {
                            context.Response.WriteAsync("<html>").Wait();
                            context.Response.WriteAsync("<body>").Wait();
                            context.Response.WriteAsync("<div>" + result + "</div>").Wait();
                            context.Response.WriteAsync("</body>").Wait();
                            context.Response.WriteAsync("</html>").Wait() ;
                            }
                        });
                   // using(StreamWriter writer=new StreamWriter(context.Response.Body))
                        {
                          
                       // context.Request.Headers.Append("ContentLength", context.Request.Body.Length.ToString());
                        }
              t.Wait();     
                    //await Task.Run(delegate { 
 
                       // context.Response.StartAsync().Wait();
                   // });
                }
                }
                else
           // Console.WriteLine(path);
              next(context).Wait();
            });
            
        }
    }
}
