using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using Movify.Models;

namespace Movify.Utils
{
    public class Auth
    {
        private static bool debugMode = true;
        public static bool isAuth(HttpContext httpContext, ref Customer cus)
        {
            if (debugMode)
            {
                using (var mc = new MovifyContext())
                {
                    cus = mc.Customers.Find("kn@x.com");
                    return true;
                }
            }
            if (httpContext == null) return false;
            var session = httpContext.Session;
            if (session == null) return false;
            var cusJSON = session.Get("customer");
            if (cusJSON == null) return false;
            cus = JsonSerializer.Deserialize<Customer>(cusJSON);
            return cus.role == "customer" || cus.role == "admin";
        }

        public static bool isCustomer(HttpContext httpContext, ref Customer cus)
        {
            if (debugMode)
            {
                using (var mc = new MovifyContext())
                {
                    cus = mc.Customers.Find("kn@x.com");
                    return true;
                }
            }
            if (httpContext == null) return false;
            var session = httpContext.Session;
            if (session == null) return false;
            var cusJSON = session.Get("customer");
            if (cusJSON == null) return false;
            cus = JsonSerializer.Deserialize<Customer>(cusJSON);
            return cus.role == "customer";
        }

        public static bool isAdmin(HttpContext httpContext, ref Customer cus)
        {
            if (debugMode)
            {
                using (var mc = new MovifyContext())
                {
                    cus = mc.Customers.Find("kn@x.com");
                    return true;
                }
            }
            if (httpContext == null) return false;
            var session = httpContext.Session;
            if (session == null) return false;
            var cusJSON = session.Get("customer");
            if (cusJSON == null) return false;
            cus = JsonSerializer.Deserialize<Customer>(cusJSON);
            return cus.role == "admin";
        }
    }
}
