using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_Blazor.Shared.Models;

namespace Project_Blazor.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentCarsController : ControllerBase
    {
        private readonly VehicleDbContext _context;

        public RentCarsController(VehicleDbContext context)
        {
            _context = context;
        }

        // GET: api/RentCars
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RentCar>>> GetRentCars()
        {
          if (_context.RentCars == null)
          {
              return NotFound();
          }
            return await _context.RentCars.ToListAsync();
        }

        // GET: api/RentCars/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RentCar>> GetRentCar(int id)
        {
          if (_context.RentCars == null)
          {
              return NotFound();
          }
            var RentCar = await _context.RentCars.FindAsync(id);

            if (RentCar == null)
            {
                return NotFound();
            }

            return RentCar;
        }

        // PUT: api/RentCars/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRentCar(int id, RentCar RentCar)
        {
            if (id != RentCar.RentID)
            {
                return BadRequest();
            }

            _context.Entry(RentCar).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RentCarExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/RentCars
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RentCar>> PostRentCar(RentCar RentCar)
        {
          if (_context.RentCars == null)
          {
              return Problem("Entity set 'VehicleDbContext.RentCars'  is null.");
          }
            _context.RentCars.Add(RentCar);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (RentCarExists(RentCar.RentID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetRentCar", new { id = RentCar.RentID }, RentCar);
        }

        // DELETE: api/RentCars/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRentCar(int id)
        {
            if (_context.RentCars == null)
            {
                return NotFound();
            }
            var RentCar = await _context.RentCars.FindAsync(id);
            if (RentCar == null)
            {
                return NotFound();
            }

            _context.RentCars.Remove(RentCar);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RentCarExists(int id)
        {
            return (_context.RentCars?.Any(e => e.RentID == id)).GetValueOrDefault();
        }
    }
}
