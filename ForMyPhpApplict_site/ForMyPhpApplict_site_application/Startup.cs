using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles.Infrastructure;
using Microsoft.AspNetCore.StaticFiles;
using ForMyPhpApplict_site_application.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Peachpie.AspNetCore.Web;
using Peachpie.Runtime;
namespace ForMyPhpApplict_site_application
{
    public class Startup
    {
        
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddRazorPages();
            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                //options.Cookie.HttpOnly = true;
            });
            services.AddMvc(opt =>
            {
                opt.EnableEndpointRouting = false;
                
            });
            services.AddDirectoryBrowser();
           
            services.AddHttpContextAccessor();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
        options.SlidingExpiration = true;
        options.LoginPath = "/Login/";
    });
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddPhp();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            BasePath.SetPath(env.ContentRootPath);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }
            app.UseSession();
            
            var cookiePolicyOptions = new CookiePolicyOptions
            {
                MinimumSameSitePolicy = SameSiteMode.Strict,
            };
           // app.UsePhp("/",rootPath:Path.Combine(BasePath.RootPath+"/",BasePath.UserFalesPath));
            var f_p= new PhysicalFileProvider(
                   Path.Combine(env.ContentRootPath,BasePath.UserFalesPath)

                    );
            
app.UseFileServer(new FileServerOptions
            {
                FileProvider =f_p,
                RequestPath="/"+BasePath.UserFalesPath,
                EnableDirectoryBrowsing=true
                ,
                StaticFileOptions={
                    OnPrepareResponse = ctx =>
                    {
                        ctx.Context.Response.Headers.Append("charset","utf-8");
                        ctx.Context.Response.Headers.Append("Content-Type","text/html; charset=utf-8");
                       // ctx.Context.Response.Headers.Append( "Cache-Control", $"public, max-age={TimeSpan.FromSeconds(TimeSpan.FromMinutes(5).TotalSeconds)}");
                    }
                    
                }

            }) ;
            app.UseStaticFiles();
             app.UseAuthentication();
            app.UseAuthorization();
 app.UseCookiePolicy(cookiePolicyOptions);
            app.UseRouting();
           
            app.UseDefaultFiles();
            app.UseMvc(opt =>
            {
                opt.MapRoute(default, "{controller=home}/{action=Index}/{id?}");
                
            });
            //app.UseMiddleware<MiddleWare.PhpFileDetectMiddleware>();
           
          /*  app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });*/
        }
    }
}
