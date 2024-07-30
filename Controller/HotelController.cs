using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using usingLinq.Context;
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
        public IActionResult Get()
        {
            var hotel = _context.Hotels.ToList();
            return Ok(hotel);
        }

        [HttpPost]

        public IActionResult Create([FromBody]Hotel hotel)
        {
            // Ensure IsDeleted is false
            hotel.IsDeleted = false;

            _context.Hotels.Add(hotel);
            _context.SaveChanges();
               return CreatedAtAction(nameof(GetById), new { id = hotel.Id }, hotel);
            // return Ok("Created Successfully");
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

        public IActionResult Update(int id , Hotel updateHotel)
        {
            var findHotel = _context.Hotels.Find(id);

            if(findHotel == null)
            {
                return NotFound();
            }

            findHotel.Name = updateHotel.Name;
            findHotel.Description = updateHotel.Description;
            findHotel.ImageUrl = updateHotel.ImageUrl;
            _context.SaveChanges();

            return Ok("Updated Sucessfully");
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