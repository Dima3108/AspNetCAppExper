using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
namespace ForMyPhpApplict_site_application
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel(opt =>
                    {
                        opt.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(1);
                        opt.Limits.MaxConcurrentConnections = 50;
                        opt.Limits.KeepAliveTimeout = TimeSpan.FromSeconds(60);
                        opt.Limits.Http2.MaxStreamsPerConnection = 25;
                        opt.Limits.Http2.HeaderTableSize = 4096;
                        opt.Limits.Http2.MaxFrameSize = 16_384;
                        opt.Limits.Http2.MaxRequestHeaderFieldSize = 8192;
                        opt.Limits.Http2.InitialConnectionWindowSize = 131_072;
                        opt.Limits.Http2.InitialStreamWindowSize = 98_304;
                        opt.Limits.Http2.KeepAlivePingDelay = TimeSpan.FromSeconds(30);
                        opt.Limits.Http2.KeepAlivePingTimeout = TimeSpan.FromMinutes(1);
                    });
                   
                    webBuilder.UseStartup<Startup>();
                });
    }
}
