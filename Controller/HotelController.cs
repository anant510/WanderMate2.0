using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using usingLinq.Context;
using usingLinq.Dtos;
using usingLinq.Models;

namespace usingLinq.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class HotelController : ControllerBase  
    {
        private readonly ApplicationDbContext _context;

        public HotelController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Hotel>>> Get()
        {
            var hotel = await _context.Hotels.ToListAsync();
            return Ok(hotel);
        }
  
        [HttpPost]

        public async Task<ActionResult<IEnumerable<Hotel>>> Create([FromBody] HotelDto hotelDto)
        {
          try{
            // Ensure IsDeleted is false
            hotelDto.IsDeleted = false;

            var hotel = new Hotel{
                Name = hotelDto.Name,
                Description = hotelDto.Description,
                ImageUrl = hotelDto.ImageUrl,
            };

           await _context.Hotels.AddAsync(hotel);
           await _context.SaveChangesAsync();
               return CreatedAtAction(nameof(GetById), new { id = hotel.Id }, hotel);
            // return Ok("Created Successfully");
          }catch{
            return BadRequest();
          }
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var hotel = _context.Hotels.Find(id);

            if(hotel == null){
                return NotFound();
            }

            return Ok(hotel);
        }

        [HttpPut("{id}")]

        public async Task<ActionResult<IEnumerable<Hotel>>> Update(int id , Hotel updateHotel)
        {
            try{
                
            var findHotel = await _context.Hotels.FindAsync(id);

            if(findHotel == null)
            {
                return NotFound();
            }

            findHotel.Name = updateHotel.Name;
            findHotel.Description = updateHotel.Description;
            findHotel.ImageUrl = updateHotel.ImageUrl;
           await _context.SaveChangesAsync();

            return Ok("Updated Sucessfully");
            
            }catch{
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]

        public IActionResult Delete(int id)
        {
            var findHotel = _context.Hotels.Find(id);

            if (findHotel == null) {
                return NotFound();
            }

            // _context.Hotels.Remove(findHotel);
            findHotel.IsDeleted = true;
            _context.SaveChanges();

            return Ok("Deleted Sucessfully");

        }

        [HttpGet("search")]
        public IActionResult SearchByName([FromQuery] string name)
        {
            var hotels = _context.Hotels
                .Where(h => h.Name.Contains(name))
                .ToList();

            if (!hotels.Any())
            {
                return NotFound();
            }

            return Ok(hotels);
        }

        
    }
}