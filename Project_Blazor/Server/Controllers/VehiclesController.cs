using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_Blazor.Shared.DTO;
using Project_Blazor.Shared.Models;

namespace Project_Blazor.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehiclesController : ControllerBase
    {
        private readonly VehicleDbContext _context;
        private readonly IWebHostEnvironment env;
        public VehiclesController(VehicleDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            this.env = env;
        }

        // GET: api/Vehicles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vehicle>>> GetVehicles()
        {
          if (_context.Vehicles == null)
          {
              return NotFound();
          }
            return await _context.Vehicles.ToListAsync();
        }

        // GET: api/Vehicles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Vehicle>> GetVehicle(int id)
        {
          if (_context.Vehicles == null)
          {
              return NotFound();
          }
            var Vehicle = await _context.Vehicles.FindAsync(id);

            if (Vehicle == null)
            {
                return NotFound();
            }

            return Vehicle;
        }

        // PUT: api/Vehicles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVehicle(int id, Vehicle Vehicle)
        {
            if (id != Vehicle.VehicleID)
            {
                return BadRequest();
            }

            _context.Entry(Vehicle).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VehicleExists(id))
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
        
        // POST: api/Vehicles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Vehicle>> PostVehicle(Vehicle Vehicle)
        {
          if (_context.Vehicles == null)
          {
              return Problem("Entity set 'VehicleDbContext.Vehicles'  is null.");
          }
            _context.Vehicles.Add(Vehicle);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVehicle", new { id = Vehicle.VehicleID }, Vehicle);
        }
        
        [HttpPost("Upload")]
        public async Task<ImageUploadResponse> Upload(IFormFile file)
        {
            var ext = Path.GetExtension(file.FileName);
            var randomFileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName());
            var storedFileName = randomFileName + ext;
            using FileStream fs = new FileStream(Path.Combine(env.WebRootPath, "Uploads", storedFileName), FileMode.Create);
            await file.CopyToAsync(fs);
            fs.Close();
            return new ImageUploadResponse { FileName = file.FileName, StoredFileName = storedFileName };
        }
        // DELETE: api/Vehicles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicle(int id)
        {
            if (_context.Vehicles == null)
            {
                return NotFound();
            }
            var Vehicle = await _context.Vehicles.FindAsync(id);
            if (_context.RentCars.Any(x => x.VehicleID == id)) return BadRequest("Entity has child data");
            if (Vehicle == null)
            {
                return NotFound();
            }

            _context.Vehicles.Remove(Vehicle);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VehicleExists(int id)
        {
            return (_context.Vehicles?.Any(e => e.VehicleID == id)).GetValueOrDefault();
        }
    }
}
