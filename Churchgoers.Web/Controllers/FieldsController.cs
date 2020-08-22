using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Churchgoers.Common.Entities;
using Churchgoers.Web.Data;

namespace Churchgoers.Web.Controllers
{
    public class FieldsController : Controller
    {
        private readonly DataContext _context;

        public FieldsController(DataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Fields
                
                .ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Field field = await _context.Fields
               .Include(f => f.Districts)
               .ThenInclude(d => d.Churches)
               .FirstOrDefaultAsync(m => m.Id == id);
            if (field == null)
            {
                return NotFound();
            }

            return View(field);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Field field)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(field);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "There are a record with the same name.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }
            return View(field);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Field field = await _context.Fields.FindAsync(id);
            if (field == null)
            {
                return NotFound();
            }
            return View(field);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Field field)
        {
            if (id != field.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(field);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "There are a record with the same name.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }
            return View(field);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Field field = await _context.Fields
                .Include(f => f.Districts)
                .ThenInclude(d => d.Churches)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (field == null)
            {
                return NotFound();
            }

            _context.Fields.Remove(field);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
