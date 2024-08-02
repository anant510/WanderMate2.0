using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using usingLinq.Context;
using usingLinq.Dtos;
using usingLinq.Models;

namespace usingLinq.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewController : ControllerBase
    {
         private readonly ApplicationDbContext _context;

        public ReviewController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]

        public IActionResult Get()
        {
            return Ok(_context.Reviews.ToList());
        }

        [HttpPost]

        public async Task<ActionResult<IEnumerable<Review>>> Create([FromBody] ReviewDto reviewDto)
        {
           try{
                var review = new Review{

                Rating = reviewDto.Rating,
                ReviewText = reviewDto.ReviewText,
                HotelId = reviewDto.HotelId
            };

            await _context.Reviews.AddAsync(review);
            await _context.SaveChangesAsync();
            return Ok("Created sucessfully");
           }catch{
            return BadRequest();
           }
        }

        [HttpDelete]

        public IActionResult Delete(int id)
        {
            var review = _context.Reviews.Find(id);

            if (review == null)
            {
                return NotFound();
            }

            _context.Reviews.Remove(review);
            _context.SaveChanges();

            return Ok("Deleted Sucessfully");
        }
    }
}