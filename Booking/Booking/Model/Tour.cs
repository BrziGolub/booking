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

        public List<TourKeyPoints> Destinations { get; set; }

        public DateTime StartTime { get; set; }

        public double Duration { get; set; }

        public List<TourImage> Images { get; set; }

<<<<<<< Updated upstream
        public Tour()
        {
            Location = new Location();
            Destinations = new List<TourKeyPoints>();
            Images = new List<TourImage>();
        }
=======
		public bool isStarted { get; set; }
		public Tour()
		{
			Location = new Location();
			Destinations = new List<TourKeyPoints>();
			Images = new List<TourImage>();
			isStarted = false;
		}
>>>>>>> Stashed changes

        public Tour(string name, Location location, string desc, string lang, int maxNum, DateTime dt, double duration)
        {
            Name = name;
            Location = location;
            Description = desc;
            Language = lang;
            MaxGuestsNumber = maxNum;
            StartTime = dt;
            Duration = duration;
            Destinations = new List<TourKeyPoints>();
            Images = new List<TourImage>();
        }


        public string[] ToCSV()
        {
            

            string[] csvValues =
                {
                Id.ToString(),                          //0
                Name,                                   //1
                Location.Id.ToString(),                 //2
                Description,                            //3
                Language,                               //4
<<<<<<< Updated upstream
                MaxGuestsNumber.ToString(),             //5
                DateConversion.DateToString(StartTime), //6
                Duration.ToString()                     //7
            };
            return csvValues;
        }
        public void FromCSV(string[] values)
        {

            Id = Convert.ToInt32(values[0]);
            Name = values[1];
            Location.Id = Convert.ToInt32(values[2]);
            Description = values[3];
            Language = values[4];
            MaxGuestsNumber = Convert.ToInt32(values[5]);

            StartTime = DateConversion.StringToDate(values[6]);
            Duration = Convert.ToDouble(values[7]);

        }
    }
=======
                MaxVisitors.ToString(),                 //5
                //
                StartTime.ToString("dd.MM.yyyy"),       //6
                Duration.ToString() ,                   //7
				isStarted.ToString()					//8
            };
			return csvValues;
		}
		public void FromCSV(string[] values)
		{
			Id = Convert.ToInt32(values[0]);
			Name = values[1];
			Location.Id = Convert.ToInt32(values[2]);
			Description = values[3];
			Language = values[4];
			MaxVisitors = Convert.ToInt32(values[5]);
			//
			StartTime = DateTime.Parse(values[6]);
			Duration = Convert.ToDouble(values[7]);
			isStarted = Convert.ToBoolean(values[8]);
		}
	}
>>>>>>> Stashed changes
}
