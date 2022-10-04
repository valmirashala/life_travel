using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Travelephant.Data;
using Travelephant.Model;

namespace Travelephant.Controllers
{
    public class BusInfoesController : Controller
    {
        private readonly TravelephantContext _context;

        public BusInfoesController(TravelephantContext context)
        {
            _context = context;
        }

        // GET: BusInfoes
        public async Task<IActionResult> Index()
        {
              return View(await _context.BusInfo.ToListAsync());
        }

        // GET: BusInfoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.BusInfo == null)
            {
                return NotFound();
            }

            var busInfo = await _context.BusInfo
                .FirstOrDefaultAsync(m => m.BusId == id);
            if (busInfo == null)
            {
                return NotFound();
            }

            return View(busInfo);
        }

        // GET: BusInfoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BusInfoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BusId,Name,Departure,Destination,DepartureTime,ArrivalTime,TotalSeat,AvailableSeat,Price")] BusInfo busInfo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(busInfo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(busInfo);
        }

        // GET: BusInfoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.BusInfo == null)
            {
                return NotFound();
            }

            var busInfo = await _context.BusInfo.FindAsync(id);
            if (busInfo == null)
            {
                return NotFound();
            }
            return View(busInfo);
        }

        // POST: BusInfoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BusId,Name,Departure,Destination,DepartureTime,ArrivalTime,TotalSeat,AvailableSeat,Price")] BusInfo busInfo)
        {
            if (id != busInfo.BusId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(busInfo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BusInfoExists(busInfo.BusId))
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
            return View(busInfo);
        }

        // GET: BusInfoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.BusInfo == null)
            {
                return NotFound();
            }

            var busInfo = await _context.BusInfo
                .FirstOrDefaultAsync(m => m.BusId == id);
            if (busInfo == null)
            {
                return NotFound();
            }

            return View(busInfo);
        }

        // POST: BusInfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.BusInfo == null)
            {
                return Problem("Entity set 'TravelephantContext.BusInfo'  is null.");
            }
            var busInfo = await _context.BusInfo.FindAsync(id);
            if (busInfo != null)
            {
                _context.BusInfo.Remove(busInfo);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BusInfoExists(int id)
        {
          return _context.BusInfo.Any(e => e.BusId == id);
        }
    }
}
