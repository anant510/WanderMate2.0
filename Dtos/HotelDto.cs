using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace usingLinq.Dtos
{
    public class HotelDto
    {
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; }  = string.Empty;

        public string ImageUrl { get; set; } = string.Empty;

        public bool IsDeleted { get; set; } = false; // Soft delete flag
    }
}