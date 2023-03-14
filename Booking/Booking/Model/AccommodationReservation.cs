using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Model
{
    public class AccommodationReservation
    {

        public int Id { get; set; }
        
        public Accommodation Accommodation { get; set; }

        public User User { get; set; }

        public DateTime ArrivalDay { get; set; }

        public DateTime DepartureDay { get; set; } 
    }
}
