using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace usingLinq.Dtos
{
    public class HotelDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; }  = string.Empty;

        public List<string> ImageUrl { get; set; } = new List<string>();

        public int? Price {get; set;} 

        public bool FreeCancellation {get; set;}= false;

        public bool ReserveNow {get; set;} = false;

        // public bool IsDeleted { get; set; } = false; // Soft delete flag
    }
}