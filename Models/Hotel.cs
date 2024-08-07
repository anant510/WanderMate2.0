using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace usingLinq.Models
{
    public class Hotel
    {
        
        [Key]
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; }  = string.Empty;

        public List<string> ImageUrl { get; set; } = new List<string>();

        public int? Price {get; set;}

        public bool FreeCancellation {get; set;}= false;

        public bool ReserveNow {get; set;} = false;

         public bool IsDeleted { get; set; } = false; // Soft delete flag

         // Navigation property: A hotel can have many reviews
        public ICollection<Review> Reviews { get; set; } = new List<Review>();

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}