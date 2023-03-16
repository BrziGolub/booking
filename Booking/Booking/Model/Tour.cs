using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Booking.Serializer;
using Booking.Conversion;
using System.Windows;
using Booking.Model.Images;

namespace Booking.Model
{
    public class Tour : ISerializable
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Location Location { get; set; }

        public string Description { get; set; }

        public string Language { get; set; }

        public int MaxGuestsNumber { get; set; }

        public List<Location> Destinations { get; set; }

        public List<int> LocationID { get; set; }

        public DateTime StartTime { get; set; }

        public double Duration { get; set; }

        public List<TourImage> Images { get; set; }

        public Tour()
        {
            Location = new Location();
            Destinations = new List<Location>();
            Images = new List<TourImage>();
        }

        public Tour(string name, Location location, string desc, string lang, int maxNum, DateTime dt, double duration)
        {
            Name = name;
            Location = location;
            Description = desc;
            Language = lang;
            MaxGuestsNumber = maxNum;
            StartTime = dt;
            Duration = duration;
            Destinations = new List<Location>();
            Images = new List<TourImage>();
        }


        public string[] ToCSV()
        {
            string pom = "";
            foreach (Location location in Destinations)
            {
                string pom1 = location.Id.ToString();
                pom += pom1 + ",";
            }

            string[] csvValues =
                {
                Id.ToString(),
                Name,
                Location.Id.ToString(),
                Description,
                Language,
                MaxGuestsNumber.ToString(),
                pom,
                DateConversion.DateToString(StartTime),
                Duration.ToString()
            };
            return csvValues;
        }
        public void FromCSV(string[] values)
        {

            List<int> listId = new List<int>();
            foreach(String stringId in values[6].Split(','))
            {
                listId.Add(int.Parse(stringId));
            }
                // treba uraditi fromCSV, treba resiti da ne upisuje , na kraju toCSV-a , u fromu splitujem prvo po zarezu i onda 
            

            Id = Convert.ToInt32(values[0]);
            Name = values[1];
            Location.Id = Convert.ToInt32(values[2]);
            Description = values[3];
            Language = values[4];
            MaxGuestsNumber = Convert.ToInt32(values[5]);
            //Destinations = values[6];
            StartTime = DateConversion.StringToDate(values[7]);
            Duration = Convert.ToDouble(values[8]);
        }
    }
}
