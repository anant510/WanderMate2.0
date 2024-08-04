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
    public class ReviewController : ControllerBase
    {
         private readonly ApplicationDbContext _context;

        public ReviewController(ApplicationDbContext context)
        {
            _context = context;
        }

         [HttpGet]  
        public async Task<ActionResult<IEnumerable<Review>>> GetReviews()
        {
            return await _context.Reviews
                                 .Include(r => r.Hotel)
                                 .Include(r => r.User)
                                 .ToListAsync();
        }

        //   [HttpGet]  
        // public async Task<ActionResult<IEnumerable<Review>>> GetReviews()
        // {
        //     return await _context.Reviews
        //                          .ToListAsync();
        // }

        [HttpGet("{id}")]
        public async Task<ActionResult<Review>> GetReview(int id)
        {
            var review = await _context.Reviews
                                       .Include(r => r.Hotel)
                                       .Include(r => r.User)
                                       .FirstOrDefaultAsync(r => r.Id == id);

            if (review == null)
            {
                return NotFound();
            }

            return review;
        }

        [HttpPost]

        public async Task<ActionResult<IEnumerable<Review>>> Create([FromBody] ReviewDto reviewDto)
        {
           try{
                var review = new Review{

                Rating = reviewDto.Rating,
                ReviewText = reviewDto.ReviewText,
                HotelId = reviewDto.HotelId,
                UserId = reviewDto.UserId
            };

            await _context.Reviews.AddAsync(review);
            await _context.SaveChangesAsync();
            return Ok("Created sucessfully");
           }catch{
            return BadRequest();
           }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutReview(int id, Review review)
        {
            if (id != review.Id)
            {
                return BadRequest();
            }

            _context.Entry(review).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReviewExists(id))
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

        private bool ReviewExists(int id)
        {
            return _context.Reviews.Any(e => e.Id == id);
        }
    }
}