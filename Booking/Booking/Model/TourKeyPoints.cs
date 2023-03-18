using Booking.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;


namespace Booking.Model
{
    public class TourKeyPoints : ISerializable
    {
        public int Id { get; set; }
        public Tour Tour { get; set; }
        public Location Location { get; set; }
        public bool Achieved { get; set; }
        public TourKeyPoints() 
        {
            Tour = new Tour();
            Location = new Location();
            Achieved = false;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            Tour.Id = int.Parse(values[1]);
            Location.Id = Convert.ToInt32(values[2]);
            Achieved = Convert.ToBoolean(values[3]);
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                Tour.Id.ToString(),   
                Location.Id.ToString(),
                Achieved.ToString()
            };
            return csvValues;
       
        }
    }
}
