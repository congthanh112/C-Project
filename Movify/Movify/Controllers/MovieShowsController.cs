using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Movify;
using Movify.Models;
using Movify.Utils;

namespace Movify.Controllers
{
    public class MovieShowsController : Controller
    {
        private readonly MovifyContext _context;

        public MovieShowsController()
        {
            _context = new MovifyContext();
        }

        // GET: MovieShows
        public async Task<IActionResult> Index()
        {
            Customer cus = null;
            if (!Auth.isAdmin(HttpContext, ref cus)) return Unauthorized();
            ViewBag.customer = cus;

            var movifyContext = _context.MovieShows.Include(m => m.movie).Include(m => m.theater);
            return View(await movifyContext.ToListAsync());
        }

        // GET: MovieShows/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            Customer cus = null;
            if (!Auth.isAdmin(HttpContext, ref cus)) return Unauthorized();
            ViewBag.customer = cus;

            if (id == null)
            {
                return NotFound();
            }

            var movieShow = await _context.MovieShows
                .Include(m => m.movie)
                .Include(m => m.theater)
                .Include(m => m.tickets)
                .ThenInclude(m => m.Seat)
                .Include(m => m.tickets)
                .ThenInclude(m => m.Customer)
                .FirstOrDefaultAsync(m => m.id == id);

            if (movieShow == null)
            {
                return NotFound();
            }

            return View(movieShow);
        }

        // GET: MovieShows/Create
        public IActionResult Create()
        {
            Customer cus = null;
            if (!Auth.isAdmin(HttpContext, ref cus)) return Unauthorized();
            ViewBag.customer = cus;

            ViewData["movieid"] = new SelectList(_context.Movies, "id", "name");
            ViewData["theaterid"] = new SelectList(_context.Theater, "id", "name");
            return View();
        }

        // POST: MovieShows/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,startTime,endTime,price,theaterid,movieid,status")] MovieShow movieShow)
        {
            Customer cus = null;
            if (!Auth.isAdmin(HttpContext, ref cus)) return Unauthorized();
            ViewBag.customer = cus;
            if (ModelState.IsValid)
            {
                _context.Add(movieShow);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["movieid"] = new SelectList(_context.Movies, "id", "name", movieShow.movieid);
            ViewData["theaterid"] = new SelectList(_context.Theater, "id", "name", movieShow.theaterid);
            return View(movieShow);
        }

        // GET: MovieShows/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            Customer cus = null;
            if (!Auth.isAdmin(HttpContext, ref cus)) return Unauthorized();
            ViewBag.customer = cus;

            if (id == null)
            {
                return NotFound();
            }

            var movieShow = await _context.MovieShows.FindAsync(id);
            if (movieShow == null)
            {
                return NotFound();
            }
            ViewData["movieid"] = new SelectList(_context.Movies, "id", "name", movieShow.movieid);
            ViewData["theaterid"] = new SelectList(_context.Theater, "id", "name", movieShow.theaterid);
            return View(movieShow);
        }

        // POST: MovieShows/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,startTime,endTime,price,theaterid,movieid,status")] MovieShow movieShow)
        {
            Customer cus = null;
            if (!Auth.isAdmin(HttpContext, ref cus)) return Unauthorized();
            ViewBag.customer = cus;

            if (id != movieShow.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movieShow);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieShowExists(movieShow.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["movieid"] = new SelectList(_context.Movies, "id", "name", movieShow.movieid);
            ViewData["theaterid"] = new SelectList(_context.Theater, "id", "name", movieShow.theaterid);
            return View(movieShow);
        }

        // GET: MovieShows/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            Customer cus = null;
            if (!Auth.isAdmin(HttpContext, ref cus)) return Unauthorized();
            ViewBag.customer = cus;

            if (id == null)
            {
                return NotFound();
            }

            var movieShow = await _context.MovieShows
                .Include(m => m.movie)
                .Include(m => m.theater)
                .FirstOrDefaultAsync(m => m.id == id);
            if (movieShow == null)
            {
                return NotFound();
            }

            return View(movieShow);
        }

        // POST: MovieShows/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Customer cus = null;
            if (!Auth.isAdmin(HttpContext, ref cus)) return Unauthorized();
            ViewBag.customer = cus;

            var movieShow = await _context.MovieShows.FindAsync(id);
            movieShow.status = false;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieShowExists(int id)
        {
            return _context.MovieShows.Any(e => e.id == id);
        }
    }
}
