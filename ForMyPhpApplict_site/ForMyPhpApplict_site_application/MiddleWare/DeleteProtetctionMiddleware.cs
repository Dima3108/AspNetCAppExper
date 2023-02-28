using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace ForMyPhpApplict_site_application.MiddleWare
{
    public class DeleteProtetctionMiddleware
    {
        private readonly RequestDelegate next;
        public DeleteProtetctionMiddleware(RequestDelegate d)
        {
            next = d;
        }
        public async Task Invoke(HttpContext context)
        {
            await Task.Run( delegate {
                if (context.Request.Method.ToUpper() == "DELETE".ToUpper())
                {
                    context.Response.StatusCode = 401;
                    context.Response.StartAsync().Wait();
                }
                else   next(context).Wait();
            });
        }
    }
}
