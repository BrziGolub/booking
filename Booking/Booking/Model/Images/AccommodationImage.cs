using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Model.Images
{
    public class AccommodationImage
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public Accommodation Accomodation { get; set; } //moze id 

        public AccommodationImage()
        {
            Accomodation = new Accommodation();
        }



    }
}
