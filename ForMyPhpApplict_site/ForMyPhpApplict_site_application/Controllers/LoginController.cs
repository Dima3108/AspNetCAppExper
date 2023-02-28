using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;

using System.IO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Security.Principal;
using Microsoft.AspNetCore.Session;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Authorization.Policy;
//using CaptchaCSNS;
using ForMyPhpApplict_site_application.Data;

namespace ForMyPhpApplict_site_application.Controllers
{
    public class LoginController : Controller
    {
        
        [ResponseCache(Duration =60*3, VaryByHeader = "User-Agent",Location =ResponseCacheLocation.Any)]
        public IActionResult Index()
        {

            return View("Start");
        }
        [HttpPost]
        //[ResponseCache(NoStore =true,Location =ResponseCacheLocation.None)]
        
        public IActionResult Start()
        {
            /*Captcha captcha = new Captcha(200, 100, 7);
            ViewData["b64"] = captcha.GenerateAsB64(Captcha.CaptchaType.Random);
            ViewData["keyV"] = captcha.GetHashAnswer();
            ViewData["answ"] = captcha.GetAnswer();*/
            return View("Index");
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        
        public IActionResult Login(string name,string password)
        {
           // try
            {
  string dir = Path.Combine(Directory.GetCurrentDirectory(), "Data/");
            string json_f = System.IO.File.ReadAllText(dir + "users.json");
                LoginModel[] m = (JsonSerializer.Deserialize<List<LoginModel>>(json_f)).ToArray();
                    //JsonSerializer.Deserialize<LoginModel[]>(json_f, new JsonSerializerOptions { WriteIndented = true });
                foreach(var u in m)
                {
                    if (u.user == name && u.password == password)
                    {
                        Console.WriteLine($"{u.user}:{u.password}-{name}:{password}");
  var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, name),
           // new Claim("FullName", user.FullName),
            new Claim(ClaimTypes.Role, u.role),
        };
                        var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        var authProperties = new AuthenticationProperties
                        {
                            //AllowRefresh = <bool>,
                            // Refreshing the authentication session should be allowed.

                            //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                            // The time at which the authentication ticket expires. A 
                            // value set here overrides the ExpireTimeSpan option of 
                            // CookieAuthenticationOptions set with AddCookie.

                            //IsPersistent = true,
                            // Whether the authentication session is persisted across 
                            // multiple requests. When used with cookies, controls
                            // whether the cookie's lifetime is absolute (matching the
                            // lifetime of the authentication ticket) or session-based.

                            //IssuedUtc = <DateTimeOffset>,
                            // The time at which the authentication ticket was issued.

                            //RedirectUri = <string>
                            // The full path or absolute URI to be used as an http 
                            // redirect response value.
                        };

                         HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity),
                            authProperties);
                         return Redirect("/Admin/Index");
                    }
                }
          

            }
           // catch
            {
                return Redirect("/Home/Error");
            }
          
           
        }
    }
}
