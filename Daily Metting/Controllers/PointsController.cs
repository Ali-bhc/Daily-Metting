using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Daily_Metting.Repositories;
using Daily_Metting.Models;
using Microsoft.AspNetCore.Authorization;
using Daily_Metting.ViewModels;
using Daily_Metting.Data;

namespace Daily_Metting.Controllers
{
    [Authorize(Policy = "AdminPolicy")]

    public class PointsController : Controller
    {
        private readonly DailyMeetingDbContext _context;

        public PointsController(DailyMeetingDbContext context)
        {
            _context = context;
        }

        // GET: Points
        public async Task<IActionResult> Index()
        {
            List<Point> points = new List<Point>();
            points=_context.Points.Include(s=>s.Category).OrderBy(s=>s.Category.Category_Name).ToList();
              return View(points);
        }

        // GET: Points/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Points == null)
            {
                return NotFound();
            }

            var point = await _context.Points
                .FirstOrDefaultAsync(m => m.PointID == id);
            if (point == null)
            {
                return NotFound();
            }

            return View(point);
        }

        // GET: Points/Create
        public IActionResult Create()
        {
            var pointView = new PointViewModel
            {
                categories = _context.Categories.ToList()
            };
            return View(pointView);
        }

        // POST: Points/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PointViewModel pointViewModel)
        {
            if (ModelState.IsValid)
            {
                var point = new Point { Point_Name=pointViewModel.Point_Name , WH_Acces=pointViewModel.WH_Acces,CS_PP_Acces=pointViewModel.CS_PP_Acces,Procurement_Acces=pointViewModel.Procurement_Acces,
                    Category=(_context.Categories.Where(c => c.CategoryID==pointViewModel.CategoryID).FirstOrDefault())};
                _context.Points.Add(point);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pointViewModel);
        }

        // GET: Points/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Points == null)
            {
                return NotFound();
            }

            var point = await _context.Points.Include(p => p.Category).FirstOrDefaultAsync(p => p.PointID == id);
            var categories = _context.Categories.ToList();
            var pointVM = new PointEditViewModel {PointID=point.PointID, Point_Name = point.Point_Name, CategoryID = point.Category.CategoryID, categories = categories, WH_Acces = point.WH_Acces, CS_PP_Acces = point.CS_PP_Acces, Procurement_Acces = point.Procurement_Acces, HasMultipleValues = point.HasMultipleValues };
            if (point == null)
            {
                return NotFound();
            }

            if (point.Category == null) // Check if Category is null
            {
                point.Category = new Category(); // Initialize with new Category
            }

            //var test  = new SelectList(_context.Categories, "CategoryID", "Category_Name", point.Category.CategoryID); // assuming the Category class has a CategoryName property
            //ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryID", "Category_Name", point.Category.CategoryID); // assuming the Category class has a CategoryName property

            return View(pointVM);
        }

        // POST: Points/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("PointID,Point_Name,WH_Acces,CS_PP_Acces,Procurement_Acces,HasMultipleValues,CategoryID")] PointEditViewModel pointVM)
        {
            try
            {
                // Read CategoryID from form values
                var category = await _context.Categories.FindAsync(pointVM.CategoryID);

                if (category == null)
                {
                    return NotFound();
                }

                var point = _context.Points.Where(p=>p.PointID== pointVM.PointID).FirstOrDefault();
                // Set the Point's Category to the found Category
                point.Point_Name=pointVM.Point_Name;
                point.CS_PP_Acces = pointVM.CS_PP_Acces;
                point.Procurement_Acces = pointVM.Procurement_Acces;
                point.WH_Acces = pointVM.WH_Acces;
                point.HasMultipleValues = pointVM.HasMultipleValues;
                point.Category = category;
                _context.Update(point);
                await _context.SaveChangesAsync();

            }
            catch (DbUpdateConcurrencyException)
                {
                    if (!PointExists(pointVM.PointID))
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

        // GET: Points/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Points == null)
            {
                return NotFound();
            }

            var point = await _context.Points
                .FirstOrDefaultAsync(m => m.PointID == id);
            if (point == null)
            {
                return NotFound();
            }



            return View(point);
        }

        // POST: Points/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Point pt)
        {
            int id = pt.PointID;
            if (_context.Points == null)
            {
                return Problem("Entity set 'DailyMeetingDbContext.Points'  is null.");
            }
            var point = await _context.Points.FindAsync(id);
            if (point != null)
            {
                _context.Points.Remove(point);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PointExists(int id)
        {
          return _context.Points.Any(e => e.PointID == id);
        }
    }
}
