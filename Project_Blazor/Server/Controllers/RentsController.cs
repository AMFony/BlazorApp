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
    public class RentsController : ControllerBase
    {
        private readonly VehicleDbContext _context;

        public RentsController(VehicleDbContext context)
        {
            _context = context;
        }

        // GET: api/Rents
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Rent>>> GetRents()
        {
          if (_context.Rents == null)
          {
              return NotFound();
          }
            return await _context.Rents.ToListAsync();
        }
        [HttpGet("DTO")]
        public async Task<ActionResult<IEnumerable<RentViewDTO>>> GetRentDTOs()
        {
            if (_context.Rents == null)
            {
                return NotFound();
            }
            return await _context.Rents
                .Include(o => o.Customer)
                .Include(o => o.RentCars).ThenInclude(oi => oi.Vehicle)
                .Select(o => 
                    new RentViewDTO
                    {
                        RentID = o.RentID,
                        RentDate = o.RentDate,
                        DeliveryDate = o.DeliveryDate,
                        CustomerName = o.Customer.CustomerName,
                        Status=o.Status,
                        ItemCount = o.RentCars.Count,
                        RentValue = o.RentCars.Sum(x => x.Vehicle.Price * x.Quantity)

                    })
                .ToListAsync();
        }
        // GET: api/Rents/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Rent>> GetRent(int id)
        {
          if (_context.Rents == null)
          {
              return NotFound();
          }
            var Rent = await _context.Rents.FindAsync(id);

            if (Rent == null)
            {
                return NotFound();
            }

            return Rent;
        }
        [HttpGet("DTO/{id}")]
        public async Task<ActionResult<RentViewDTO>> GetRentViewDTO(int id)
        {
            if (_context.Rents == null)
            {
                return NotFound();
            }
            var Rent = await _context.Rents.Include(o => o.Customer)
                .Include(o => o.RentCars).ThenInclude(oi => oi.Vehicle).FirstOrDefaultAsync(o=> o.RentID == id);

            if (Rent == null)
            {
                return NotFound();
            }

            return new RentViewDTO { 
                RentID = Rent.RentID,
                RentDate = Rent.RentDate,
                DeliveryDate = Rent.DeliveryDate,
                CustomerName = Rent.Customer.CustomerName,
                Status = Rent.Status,
                ItemCount = Rent.RentCars.Count,
                RentValue = Rent.RentCars.Sum(x => x.Vehicle.Price * x.Quantity)

            };
        }
        [HttpGet("Items/{id}")]
        public async Task<ActionResult<IEnumerable<RentCarViewDTO>>> GetRentCars(int id)
        {
            if (_context.RentCars == null)
            {
                return NotFound();
            }
            var RentCars = await _context.RentCars.Include(x=> x.Vehicle).Where(oi=> oi.RentID == id).ToListAsync();

            if (RentCars == null)
            {
                return NotFound();
            }

            return RentCars.Select(oi =>new RentCarViewDTO {  RentID=oi.RentID, VehicleName=oi.Vehicle.VehicleName,Price=oi.Vehicle.Price, Quantity=oi.Quantity}).ToList();
        }
        [HttpGet("OI/{id}")]
        public async Task<ActionResult<IEnumerable<RentCar>>> GetRentCarsOf(int id)
        {
            if (_context.RentCars == null)
            {
                return NotFound();
            }
            var RentCars = await _context.RentCars.Where(oi => oi.RentID == id).ToListAsync();

            if (RentCars == null)
            {
                return NotFound();
            }

            return RentCars;
        }
        // PUT: api/Rents/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRent(int id, Rent Rent)
        {
            if (id != Rent.RentID)
            {
                return BadRequest();
            }

            _context.Entry(Rent).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RentExists(id))
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
        [HttpPut("DTO/{id}")]
        public async Task<IActionResult> PutRentWithRentCar(int id, RentEditDTO Rent)
        {
            if (id != Rent.RentID)
            {
                return BadRequest();
            }
            var existing = await _context.Rents.Include(x => x.RentCars).FirstAsync(o => o.RentID == id);
            _context.RentCars.RemoveRange(existing.RentCars);
            existing.RentID= Rent.RentID;
            existing.RentDate= Rent.RentDate;
            existing.DeliveryDate= Rent.DeliveryDate;
            existing.CustomerID= Rent.CustomerID;
            existing.Status= Rent.Status;
            foreach (var item in Rent.RentCars)
            {
                _context.RentCars.Add(new RentCar { RentID = Rent.RentID, VehicleID = item.VehicleID, Quantity = item.Quantity });
            }
           

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.InnerException?.Message);

            }

            return NoContent();
        }
        // POST: api/Rents
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Rent>> PostRent(Rent Rent)
        {
          if (_context.Rents == null)
          {
              return Problem("Entity set 'VehicleDbContext.Rents'  is null.");
          }
            _context.Rents.Add(Rent);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRent", new { id = Rent.RentID }, Rent);
        }

        [HttpPost("DTO")]
        public async Task<ActionResult<Rent>> PostRentDTO(RentDTO dto)
        {
            if (_context.Rents == null)
            {
                return Problem("Entity set 'VehicleDbContext.Rents'  is null.");
            }
            var Rent = new Rent {  CustomerID= dto.CustomerID, RentDate=dto.RentDate, DeliveryDate=dto.DeliveryDate, Status=dto.Status };
            foreach(var oi in dto.RentCars)
            {
                Rent.RentCars.Add(new RentCar { VehicleID = oi.VehicleID, Quantity = oi.Quantity });
            }
            _context.Rents.Add(Rent);
            await _context.SaveChangesAsync();

            return Rent;
        }
        // DELETE: api/Rents/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRent(int id)
        {
            if (_context.Rents == null)
            {
                return NotFound();
            }
            var Rent = await _context.Rents.FindAsync(id);
            if (Rent == null)
            {
                return NotFound();
            }

            _context.Rents.Remove(Rent);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RentExists(int id)
        {
            return (_context.Rents?.Any(e => e.RentID == id)).GetValueOrDefault();
        }
    }
}
