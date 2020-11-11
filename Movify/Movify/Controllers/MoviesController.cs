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
    public class MoviesController : Controller
    {
        private readonly MovifyContext _context;

        public MoviesController()
        {

            _context = new MovifyContext();
        }

        // GET: Movies
        public async Task<IActionResult> Index()
        {
            Customer cus = null;
            if (!Auth.isAdmin(HttpContext, ref cus)) return Unauthorized();
            ViewBag.customer = cus;

            var movifyContext = _context.Movies.Include(m => m.genre);
            return View(await movifyContext.ToListAsync());
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            Customer cus = null;
            if (!Auth.isAdmin(HttpContext, ref cus)) return Unauthorized();
            ViewBag.customer = cus;            //requestScope

            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .Include(m => m.genre)
                .FirstOrDefaultAsync(m => m.id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // GET: Movies/Create
        public IActionResult Create()
        {
            Customer cus = null;
            if (!Auth.isAdmin(HttpContext, ref cus)) return Unauthorized();
            ViewBag.customer = cus;

            ViewData["genreid"] = new SelectList(_context.Genres, "id", "name");
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,posterURL,name,description,releaseDate,actors,duration,trailerURL,genreid,status")] Movie movie)
        {

            Customer cus = null;
            if (!Auth.isAdmin(HttpContext, ref cus)) return Unauthorized();
            ViewBag.customer = cus;

            if (ModelState.IsValid)
            {
                _context.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["genreid"] = new SelectList(_context.Genres, "id", "name", movie.genreid);
            return View(movie);
        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            Customer cus = null;
            if (!Auth.isAdmin(HttpContext, ref cus)) return Unauthorized();
            ViewBag.customer = cus;

            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            ViewData["genreid"] = new SelectList(_context.Genres, "id", "name", movie.genreid);
            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,posterURL,name,description,releaseDate,actors,duration,trailerURL,genreid,status")] Movie movie)
        {
            Customer cus = null;
            if (!Auth.isAdmin(HttpContext, ref cus)) return Unauthorized();
            ViewBag.customer = cus;

            if (id != movie.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.id))
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
            ViewData["genreid"] = new SelectList(_context.Genres, "id", "name", movie.genreid);
            return View(movie);
        }

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            Customer cus = null;
            if (!Auth.isAdmin(HttpContext, ref cus)) return Unauthorized();
            ViewBag.customer = cus;

            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .Include(m => m.genre)
                .FirstOrDefaultAsync(m => m.id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Customer cus = null;
            if (!Auth.isAdmin(HttpContext, ref cus)) return Unauthorized();
            ViewBag.customer = cus;

            var movie = await _context.Movies.FindAsync(id);
            movie.status = false;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.id == id);
        }
    }
}
