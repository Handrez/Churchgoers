using Churchgoers.Web.Data.Entities;
using Churchgoers.Web.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vereyon.Web;

namespace Churchgoers.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProfessionsController : Controller
    {
        private readonly DataContext _context;
        private readonly IFlashMessage _flashMessage;

        public ProfessionsController(DataContext context, IFlashMessage flashMessage)
        {
            _context = context;
            _flashMessage = flashMessage;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Professions
                .Include(p => p.Users)
                .ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Profession profession)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(profession);
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
            return View(profession);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Profession profession = await _context.Professions.FindAsync(id);
            if (profession == null)
            {
                return NotFound();
            }
            return View(profession);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Profession profession)
        {
            if (id != profession.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(profession);
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
            return View(profession);
        } 

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Profession profession = await _context.Professions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (profession == null)
            {
                return NotFound();
            }

            try
            {
                _context.Professions.Remove(profession);
                await _context.SaveChangesAsync();
                _flashMessage.Confirmation("The profession was deleted.");
            }
            catch
            {
                _flashMessage.Danger("The profession can't be deleted because it has related records.");
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
