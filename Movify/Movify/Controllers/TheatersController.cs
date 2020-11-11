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
    public class TheatersController : Controller
    {
        private readonly MovifyContext _context;

        public TheatersController()
        {
            _context = new MovifyContext();
        }

        // GET: Theaters
        public async Task<IActionResult> Index()
        {
            Customer cus = null;
            if (!Auth.isAdmin(HttpContext, ref cus)) return Unauthorized();
            ViewBag.customer = cus;

            return View(await _context.Theater.ToListAsync());
        }

        // GET: Theaters/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            Customer cus = null;
            if (!Auth.isAdmin(HttpContext, ref cus)) return Unauthorized();
            ViewBag.customer = cus;
            if (id == null)
            {
                return NotFound();
            }

            var theater = await _context.Theater
                .FirstOrDefaultAsync(m => m.id == id);
            if (theater == null)
            {
                return NotFound();
            }

            return View(theater);
        }

        // GET: Theaters/Create
        public IActionResult Create()
        {
            Customer cus = null;
            if (!Auth.isAdmin(HttpContext, ref cus)) return Unauthorized();
            ViewBag.customer = cus;
            return View();
        }

        // POST: Theaters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,name,status,rows,cols")] Theater theater)
        {
            Customer cus = null;
            if (!Auth.isAdmin(HttpContext, ref cus)) return Unauthorized();
            ViewBag.customer = cus;

            if (ModelState.IsValid)
            {
                await _context.AddAsync(theater);
                await _context.SaveChangesAsync();

                for(int i = 0; i < theater.rows; ++i)
                {
                    for(int j = 0; j < theater.cols; ++j)
                    {
                        Seat seat = new Seat
                        {
                            r = i,
                            c = j,
                            theaterid = theater.id
                        };
                        await _context.AddAsync(seat);
                    }
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(theater);
        }

        // GET: Theaters/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            Customer cus = null;
            if (!Auth.isAdmin(HttpContext, ref cus)) return Unauthorized();
            ViewBag.customer = cus;
            if (id == null)
            {
                return NotFound();
            }

            var theater = await _context.Theater.FindAsync(id);
            if (theater == null)
            {
                return NotFound();
            }
            return View(theater);
        }

        // POST: Theaters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,name,status")] Theater theater)
        {
            Customer cus = null;
            if (!Auth.isAdmin(HttpContext, ref cus)) return Unauthorized();
            ViewBag.customer = cus;
            if (id != theater.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var oldTheater = await _context.Theater.FindAsync(id);
                    if (oldTheater == null) return NotFound();
                    oldTheater.name = theater.name;
                    oldTheater.status = theater.status;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TheaterExists(theater.id))
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
            return View(theater);
        }

        // GET: Theaters/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            Customer cus = null;
            if (!Auth.isAdmin(HttpContext, ref cus)) return Unauthorized();
            ViewBag.customer = cus;
            if (id == null)
            {
                return NotFound();
            }

            var theater = await _context.Theater
                .FirstOrDefaultAsync(m => m.id == id);
            if (theater == null)
            {
                return NotFound();
            }

            return View(theater);
        }

        // POST: Theaters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Customer cus = null;
            if (!Auth.isAdmin(HttpContext, ref cus)) return Unauthorized();
            ViewBag.customer = cus;

            var theater = await _context.Theater.FindAsync(id);
            theater.status = false;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TheaterExists(int id)
        {
            return _context.Theater.Any(e => e.id == id);
        }
    }
}
