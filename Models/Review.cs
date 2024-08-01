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

        public string ReviewText { get; set; } = String.Empty; 

        // Foreign key to Hotel
        public int? HotelId { get; set; }

        // Navigation property: A review belongs to one hotel
        public Hotel? Hotel { get; set; }
    }
}