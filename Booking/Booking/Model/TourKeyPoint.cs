using Booking.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;


namespace Booking.Model
{
    public class TourKeyPoint : ISerializable
    {
        public int Id { get; set; }
        public Tour Tour { get; set; }
        public Location Location { get; set; }

        public TourKeyPoint() 
        {
            Tour = new Tour();
            Location = new Location();
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            Tour.Id = int.Parse(values[1]);
            Location.Id = int.Parse(values[2]);
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                Tour.Id.ToString(),   
                Location.Id.ToString(),
            };
            return csvValues;
        }
    }
}
