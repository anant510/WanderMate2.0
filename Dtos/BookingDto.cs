using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace usingLinq.Dtos
{
    public class BookingDto
    {
         public int? BookingId { get; set; }
    
        public DateTime? BookingDate { get; set; }

        public int? UserId { get; set; }

        public int? HotelId { get; set; }
    }
}