using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace usingLinq.Dtos
{
    public class ReviewDto
    {
        public int? Rating { get; set; } 

        public string? ReviewText { get; set; } 

        public int? HotelId { get; set; }
    }
}