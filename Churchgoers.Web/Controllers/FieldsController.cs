using Churchgoers.Web.Data;
using Churchgoers.Web.Data.Entities;
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
    public class FieldsController : Controller
    {
        private readonly DataContext _context;
        private readonly IFlashMessage _flashMessage;

        public FieldsController(DataContext context, IFlashMessage flashMessage)
        {
            _context = context;
            _flashMessage = flashMessage;
        }

        //FIELDS
        public async Task<IActionResult> Index()
        {
            return View(await _context.Fields
                .Include(f => f.Districts)
                .ThenInclude(d => d.Churches)
                .ThenInclude(c => c.Users)
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
               .ThenInclude(c => c.Users)
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

            try
            {
                _context.Fields.Remove(field);
                await _context.SaveChangesAsync();
                _flashMessage.Confirmation("The field was deleted.");
            }
            catch
            {
                _flashMessage.Danger("The field can't be deleted because it has related records.");
            }
            
            return RedirectToAction(nameof(Index));
        }

        //DISTRICTS
        public async Task<IActionResult> AddDistrict(int? id)
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

            District model = new District { IdField = field.Id };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddDistrict(District district)
        {
            if (ModelState.IsValid)
            {
                Field field = await _context.Fields
                    .Include(f => f.Districts)
                    .FirstOrDefaultAsync(c => c.Id == district.IdField);
                if (field == null)
                {
                    return NotFound();
                }

                try
                {
                    district.Id = 0;
                    field.Districts.Add(district);
                    _context.Update(field);
                    await _context.SaveChangesAsync();
                    return RedirectToAction($"{nameof(Details)}/{field.Id}");
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

            return View(district);
        }

        public async Task<IActionResult> EditDistrict(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            District district = await _context.Districts.FindAsync(id);
            if (district == null)
            {
                return NotFound();
            }

            Field field = await _context.Fields.FirstOrDefaultAsync(f => f.Districts.FirstOrDefault(d => d.Id == district.Id) != null);
            district.IdField = field.Id;
            return View(district);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDistrict(District district)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(district);
                    await _context.SaveChangesAsync();
                    return RedirectToAction($"{nameof(Details)}/{district.IdField}");

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
            return View(district);
        }

        public async Task<IActionResult> DeleteDistrict(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            District district = await _context.Districts
                .Include(d => d.Churches)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (district == null)
            {
                return NotFound();
            }

            Field field = await _context.Fields.FirstOrDefaultAsync(f => f.Districts.FirstOrDefault(d => d.Id == district.Id) != null);
            try
            {
                _context.Districts.Remove(district);
                await _context.SaveChangesAsync();
                _flashMessage.Confirmation("The district was deleted.");
            }
            catch
            {
                _flashMessage.Danger("The district can't be deleted because it has related records.");
            }
            
            return RedirectToAction($"{nameof(Details)}/{field.Id}");
        }

        public async Task<IActionResult> DetailsDistrict(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            District district = await _context.Districts
                .Include(d => d.Churches)
                .ThenInclude(c => c.Users)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (district == null)
            {
                return NotFound();
            }

            Field field = await _context.Fields.FirstOrDefaultAsync(f => f.Districts.FirstOrDefault(d => d.Id == district.Id) != null);
            district.IdField = field.Id;
            return View(district);
        }

        //CHURCHES
        public async Task<IActionResult> AddChurch(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            District district = await _context.Districts.FindAsync(id);
            if (district == null)
            {
                return NotFound();
            }

            Church model = new Church { IdDistrict = district.Id };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddChurch(Church church)
        {
            if (ModelState.IsValid)
            {
                District district = await _context.Districts
                    .Include(d => d.Churches)
                    .FirstOrDefaultAsync(c => c.Id == church.IdDistrict);
                if (district == null)
                {
                    return NotFound();
                }

                try
                {
                    church.Id = 0;
                    district.Churches.Add(church);
                    _context.Update(district);
                    await _context.SaveChangesAsync();
                    return RedirectToAction($"{nameof(DetailsDistrict)}/{district.Id}");

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

            return View(church);
        }

        public async Task<IActionResult> EditChurch(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Church church = await _context.Churches.FindAsync(id);
            if (church == null)
            {
                return NotFound();
            }

            District district = await _context.Districts.FirstOrDefaultAsync(d => d.Churches.FirstOrDefault(c => c.Id == church.Id) != null);
            church.IdDistrict = district.Id;
            return View(church);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditChurch(Church church)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(church);
                    await _context.SaveChangesAsync();
                    return RedirectToAction($"{nameof(DetailsDistrict)}/{church.IdDistrict}");

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
            return View(church);
        }

        public async Task<IActionResult> DeleteChurch(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Church church = await _context.Churches
                .FirstOrDefaultAsync(m => m.Id == id);
            if (church == null)
            {
                return NotFound();
            }

            District district = await _context.Districts.FirstOrDefaultAsync(d => d.Churches.FirstOrDefault(c => c.Id == church.Id) != null);
            try
            {
                _context.Churches.Remove(church);
                await _context.SaveChangesAsync();
                _flashMessage.Confirmation("The church was deleted.");
            }
            catch
            {
                _flashMessage.Danger("The church can't be deleted because it has related records.");
            }

            return RedirectToAction($"{nameof(DetailsDistrict)}/{district.Id}");
        }
    }
}
