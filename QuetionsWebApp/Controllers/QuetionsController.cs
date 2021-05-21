using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuetionsWebApp.Data;
using QuetionsWebApp.Models;

namespace QuetionsWebApp.Controllers
{
    public class QuetionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public QuetionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Quetions
        public async Task<IActionResult> Index()
        {
            return View(await _context.Quetion.ToListAsync());
        }

        // GET: Quetions/ShowSearchForm
        public async Task<IActionResult> ShowSearchForm()
        {
            return View();
        }

        // POST: Quetions/ShowSearchResults
        public async Task<IActionResult> ShowSearchResults(string SearchPhrase)
        {
            return View("Index", await _context.Quetion.Where( q => q.Question.Contains(SearchPhrase) ).ToListAsync());
        }

        // GET: Quetions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quetion = await _context.Quetion
                .FirstOrDefaultAsync(m => m.Id == id);
            if (quetion == null)
            {
                return NotFound();
            }

            return View(quetion);
        }

        [Authorize]
        // GET: Quetions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Quetions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Question,Answer")] Quetion quetion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(quetion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(quetion);
        }

        [Authorize]
        // GET: Quetions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quetion = await _context.Quetion.FindAsync(id);
            if (quetion == null)
            {
                return NotFound();
            }
            return View(quetion);
        }

        // POST: Quetions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Question,Answer")] Quetion quetion)
        {
            if (id != quetion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(quetion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuetionExists(quetion.Id))
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
            return View(quetion);
        }

        // GET: Quetions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quetion = await _context.Quetion
                .FirstOrDefaultAsync(m => m.Id == id);
            if (quetion == null)
            {
                return NotFound();
            }

            return View(quetion);
        }

        // POST: Quetions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var quetion = await _context.Quetion.FindAsync(id);
            _context.Quetion.Remove(quetion);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QuetionExists(int id)
        {
            return _context.Quetion.Any(e => e.Id == id);
        }
    }
}
