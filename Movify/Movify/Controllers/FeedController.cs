using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Movify.Models;
using Movify.Utils;
using static System.Console;

namespace Movify.Controllers
{
    public class FeedController : Controller
    {
        private MovifyContext context;

        public FeedController()
        {
            context = new MovifyContext();
        }

        public IActionResult ListTickets()
        {
            Customer cus = null;
            if (!Auth.isAuth(HttpContext, ref cus)) return Unauthorized();
            ViewBag.customer = cus;

            var tickets = context.Tickets
                            .Include(x => x.Customer)
                            .Include(x => x.MovieShow)
                            .ThenInclude(x => x.movie)
                            .Include(x => x.Seat)
                            .ThenInclude(x => x.theater)
                            .Where(x => x.Customer.email == cus.email && x.status)
                            .OrderByDescending(x => x.paymentDate)
                            .ToList();

            ViewData["tickets"] = tickets;
            return View();
        }

        public IActionResult SearchMovies(string pat_name = "", string ordered_by = "latest")
        {
            Customer cus = null;
            if (!Auth.isAuth(HttpContext, ref cus)) return Unauthorized();
            ViewBag.customer = cus;

            var movies = context.Movies.
                Where(x => EF.Functions.Like(x.name.ToUpper(),  $"%{pat_name}%".ToUpper()) && x.status)
                .Include(x => x.genre)
                .Include(x => x.MovieShows)
                .ThenInclude(x => x.tickets)
                .ToList();

            if (ordered_by == "latest")
            {
                movies.Sort((x, y) =>
                {
                    return -DateTime.Compare(x.releaseDate ?? default(DateTime), y.releaseDate ?? default(DateTime));
                });
            }

            if (ordered_by == "best seller")
            {
                movies.Sort((x, y) =>
                {
                    int t1 = 0;
                    int t2 = 0;
                    x.MovieShows.ToList().ForEach(ms => t1 += ms.tickets.Count);
                    y.MovieShows.ToList().ForEach(ms => t2 += ms.tickets.Count);
                    return t2 - t1;
                });
            }
            ViewData["pat_name"] = pat_name;
            ViewBag.movies = movies;
            return View();

        }

        public IActionResult Index()
        {
            // Return a feed
            return RedirectToAction(nameof(SearchMovies));
        }


        [HttpPost]
        public IActionResult BookTicketConfirmed(int id, [FromForm] string[] selected_items)
        {
            Customer cus = null;
            if (!Auth.isAuth(HttpContext, ref cus)) return Unauthorized();
            ViewBag.customer = cus;

            var ms = context.MovieShows
                        .Include(x => x.tickets)
                        .ThenInclude(x => x.Seat)
                        .Where(x => x.id == id)
                        .FirstOrDefault();

            if (ms == null) return NotFound();

            var ordered_seats = new HashSet<KeyValuePair<int, int>>();

            ms.tickets.ToList().ForEach(x =>
            {
                ordered_seats.Add(new KeyValuePair<int, int>(x.Seat.r, x.Seat.c));
            });

            if (selected_items != null) foreach (var item in selected_items)
            {
                    int r = int.Parse(item.Split("-")[0]);
                    int c = int.Parse(item.Split("-")[1]);

                    if (!ordered_seats.Contains(new KeyValuePair<int, int>(r, c)))
                    {
                        Seat s = context.Seat
                            .Where(x => x.r == r && x.c == c && x.theaterid == ms.theaterid)
                            .FirstOrDefault();

                        if (s == null) return NotFound();

                        Ticket t = new Ticket
                        {
                            seatid = s.id,
                            email = cus.email,
                            movieshowid = ms.id
                        };
                        context.Add(t);
                    }
            }

            TempData["msgresponse"] = "Your tickets has been saved";
            context.SaveChanges();
            return RedirectToAction(nameof(ListTickets));
        }

        [HttpGet]
        public IActionResult BookTicket(int id)
        {
            Customer cus = null;
            if (!Auth.isAuth(HttpContext, ref cus)) return Unauthorized();
            ViewBag.customer = cus;

            var ms = context.MovieShows
                .Include(x => x.theater)
                .Include(x => x.movie)
                .Include(x => x.tickets)
                .ThenInclude(x => x.Seat)
                .Where(x => x.id == id)
                .FirstOrDefault();

            if (ms == null) return NotFound();

            ViewData["ms"] = ms;
            var ordered_seats = new HashSet<KeyValuePair<int, int>>();
            ms.tickets.ToList().ForEach(x => ordered_seats.Add(new KeyValuePair<int, int>(x.Seat.r, x.Seat.c)));
            ViewBag.ordered_seats = ordered_seats;
           
            return View();
        }

        public IActionResult MovieDetail(int id)
        {
            // show movie detail
            Customer cus = null;
            if (!Auth.isAuth(HttpContext, ref cus)) return Unauthorized();
            ViewBag.customer = cus;

            using (var context = new MovifyContext())
            {
                var movie = context.Movies.
                    Include(x => x.genre).
                    Include(x => x.MovieShows).
                    ThenInclude(x => x.theater).
                    FirstOrDefault(x => x.id == id);
                if (movie == null) return NotFound();
                ViewBag.movie = movie;
                return View();
            }
        }


    }
}
