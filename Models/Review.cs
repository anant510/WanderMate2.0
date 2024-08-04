using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace usingLinq.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }

        public int? Rating { get; set; } 

        public string? ReviewText { get; set; } = String.Empty; 

        // Foreign key to Hotel
          // Navigation property: A review belongs to one hotel
        public int? HotelId { get; set; }
        public Hotel? Hotel { get; set; }
        public int? UserId { get; set; }
        public User? User { get; set; }

    }
}