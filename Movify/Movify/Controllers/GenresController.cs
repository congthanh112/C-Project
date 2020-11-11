using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Movify;
using Movify.Models;
using Movify.Repositories;
using Movify.UnitOfWorks;
using Movify.Utils;

namespace Movify.Controllers
{
    public class GenresController : Controller
    {
        private readonly MovifyContext _context;
        private readonly UnitOfWork unitOfWork;

        public GenresController()
        {
            _context = new MovifyContext();
            unitOfWork = new UnitOfWork(_context);
        }


        // GET: Genres
        public async Task<IActionResult> Index()
        {
            Customer cus = null;
            if (!Auth.isAdmin(HttpContext, ref cus)) return Unauthorized();
            ViewBag.customer = cus;

            return View(await unitOfWork.Genres.GetAllAsync());
        }

        // GET: Genres/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            Customer cus = null;
            if (!Auth.isAdmin(HttpContext, ref cus)) return Unauthorized();
            ViewBag.customer = cus;

            if (id == null)
            {
                return NotFound();
            }

            var genre = unitOfWork.Genres.Find(x => x.id == id).FirstOrDefault();

            if (genre == null)
            {
                return NotFound();
            }

            return View(genre);
        }

        // GET: Genres/Create
        public IActionResult Create()
        {
            Customer cus = null;
            if (!Auth.isAdmin(HttpContext, ref cus)) return Unauthorized();
            ViewBag.customer = cus;


            return View();
        }

        // POST: Genres/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,name,status")] Genre genre)
        {
            Customer cus = null;
            if (!Auth.isAdmin(HttpContext, ref cus)) return Unauthorized();
            ViewBag.customer = cus;

            if (ModelState.IsValid)
            {
                unitOfWork.Genres.Add(genre);
                unitOfWork.Complete();
                return RedirectToAction(nameof(Index));
            }
            return View(genre);
        }

        // GET: Genres/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            Customer cus = null;
            if (!Auth.isAdmin(HttpContext, ref cus)) return Unauthorized();
            ViewBag.customer = cus;


            if (id == null)
            {
                return NotFound();
            }

            var genre = unitOfWork.Genres.Find(x => x.id == id).FirstOrDefault();
            if (genre == null)
            {
                return NotFound();
            }
            return View(genre);
        }

        // POST: Genres/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,name,status")] Genre genre)
        {
            Customer cus = null;
            if (!Auth.isAdmin(HttpContext, ref cus)) return Unauthorized();
            ViewBag.customer = cus;


            if (id != genre.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    unitOfWork.Genres.Update(genre);
                    unitOfWork.Complete();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GenreExists(genre.id))
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
            return View(genre);
        }

        // GET: Genres/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {

            Customer cus = null;
            if (!Auth.isAdmin(HttpContext, ref cus)) return Unauthorized();
            ViewBag.customer = cus;


            if (id == null)
            {
                return NotFound();
            }

            var genre = unitOfWork.Genres.Find(x => x.id == id).FirstOrDefault();
            if (genre == null)
            {
                return NotFound();
            }

            return View(genre);
        }

        // POST: Genres/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Customer cus = null;
            if (!Auth.isAdmin(HttpContext, ref cus)) return Unauthorized();
            ViewBag.customer = cus;

            var genre = unitOfWork.Genres.Get(id);
            unitOfWork.Genres.Remove(genre);
            unitOfWork.Complete();
            return RedirectToAction(nameof(Index));
        }

        private bool GenreExists(int id)
        {
            return unitOfWork.Genres.Get(id) != null;
        }
    }
}
