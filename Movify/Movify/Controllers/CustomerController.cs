using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Console;
using static Movify.Utils.AlgoUtil;
using Movify.Models;
using System.ComponentModel.DataAnnotations;

namespace Movify.Controllers
{
    public class CustomerController : Controller
    {
        private MovifyContext context;
        public CustomerController()
        {
            context = new MovifyContext();
        }

        public IActionResult Index()
        {
            return View();
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private bool IsValidPhone(string phone)
        {
            if (phone.Length != 10)
            {
                return false;
            }
            foreach (var c in phone)
            {
                if (!Char.IsDigit(c))
                {
                    return false;
                }
            }

            return true;
        }

        [HttpGet]
        [HttpPost]
        public IActionResult Register(string email, string password, DateTime dob, string phone, string name, string address)
        {
            string method = HttpContext.Request.Method;
            if (method == "GET")
            {
                return View();
            }

            var errors = new List<string>();
            // TODO: validation

            if (email == null)
            {
                errors.Add("Email must not be empty");
            }
            else if (!IsValidEmail(email))
            {
                errors.Add("Email address wrong format");
            }
            else if (context.Customers.Find(email) != null)
            {
                errors.Add("Your email has been signed up");
            }

            if (dob == null)
            {
                errors.Add("Date of birth must not be empty");
            }

            if (phone == null)
            {
                errors.Add("Phone must not be empty");
            }
            else if (!IsValidPhone(phone))
            {
                errors.Add("Phone must be 10 digits long");
            }

            if (name == null)
            {
                errors.Add("Name must not be empty");
            }

            if (password == null)
            {
                errors.Add("Password must not be empty");
            }
            else if (password.Length < 8)
            {
                errors.Add("Password must be at least 8 characters");
            }

            if (errors.Count == 0)
            {
                Customer c = new Customer
                {
                    email = email,
                    password = hashStringSHA256(password),
                    dob = dob,
                    phone = phone,
                    name = name,
                    address = address
                };

                context.Add(c);
                context.SaveChanges();

                TempData["msgresponse"] = "Successfully create new account. Let's login!";
                return Redirect("/Customer/Login");
            }
            ViewBag.errors = errors;
            ViewData["email"] = email;
            ViewData["password"] = password;
            ViewData["dob"] = dob.ToString("yyyy-MM-dd");
            ViewData["phone"] = phone;
            ViewData["name"] = name;
            ViewData["address"] = address;
            return View();
        }

        [HttpGet]
        public IActionResult Logout()
        {
            TempData["msgresponse"] = "Logout successfully";
            HttpContext.Session.Clear();
            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        [HttpPost]
        public IActionResult Login(string email = "", string password = "")
        {
            string method = HttpContext.Request.Method;
            if (method == "GET")
            {
                return View();
            }

            var hashPw = hashStringSHA256(password);
            var customer = context.Customers.Where(c => c.email == email && c.password == hashPw)
                .FirstOrDefault();
            if (customer == null)
            {
                TempData["msgresponse"] = "Email or password is incorrect";
                return View();
            }
            else
            {
                HttpContext.Session.SetString("customer", JsonSerializer.Serialize(customer));
                return Redirect("/Feed");
            }
        }
    }
}
