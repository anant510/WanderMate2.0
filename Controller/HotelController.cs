using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using usingLinq.Context;
using usingLinq.Dtos;
using usingLinq.Models;

namespace usingLinq.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles ="Admin")]
    public class HotelController : ControllerBase  
    {
        private readonly ApplicationDbContext _context;

        public HotelController(ApplicationDbContext context)
        {
            _context = context;
        }
        [Authorize(Roles ="User")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Hotel>>> Get()
        {
             var hotels = await _context.Hotels.ToListAsync();
             var hotelDTO = hotels.Select( hotel => new HotelDto {
                Id = hotel.Id,
                Name = hotel.Name,
                Price=hotel.Price,
                Description=hotel.Description,
                ImageUrl =hotel.ImageUrl,
                FreeCancellation =hotel.FreeCancellation,
                ReserveNow = hotel.ReserveNow,
                
            }).ToList();

            return Ok(hotelDTO);
        }


  
        [HttpPost]

        public async Task<ActionResult<IEnumerable<Hotel>>> Create([FromBody] HotelDto hotelDto)
        {
          try{
            // Ensure IsDeleted is false
            // hotelDto.IsDeleted = false;

            var hotel = new Hotel{
                // Id = hotelDto.Id,
                Name = hotelDto.Name,
                Description = hotelDto.Description,
                ImageUrl = hotelDto.ImageUrl,
                Price = hotelDto.Price,
                FreeCancellation = hotelDto.FreeCancellation,
                ReserveNow = hotelDto.ReserveNow,

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
        public async Task<ActionResult> Update(int id, [FromBody] HotelDto hotelDto)
        {
            if (hotelDto == null)
            {
                return BadRequest("Hotel data is null");
            }


            // Find the existing hotel entity by id
            var findHotel = await _context.Hotels.FindAsync(id);

            if (findHotel == null)
            {
                return NotFound("Hotel not found");
            }

            // Update the hotel entity with values from the DTO
            // findHotel.Id = hotelDto.Id;
            findHotel.Name = hotelDto.Name;
            findHotel.Description = hotelDto.Description;
            findHotel.ImageUrl = hotelDto.ImageUrl;
            findHotel.Price = hotelDto.Price; // Use existing price if not provided
            findHotel.FreeCancellation = hotelDto.FreeCancellation;
            findHotel.ReserveNow = hotelDto.ReserveNow;
            //findHotel.IsDeleted = hotelDto.IsDeleted;

            _context.Entry(findHotel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return Ok("Updated Successfully");
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
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