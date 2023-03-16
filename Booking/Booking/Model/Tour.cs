using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Booking.Serializer;
using Booking.Conversion;

namespace Booking.Model
{
	public class Tour : ISerializable
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public Location Location { get; set; }

		public string Description { get; set; }

		public string Language { get; set; } 

		public int MaxVisitors { get; set; }

		public List<Location> Destinations { get; set; }

		public DateTime StartTime { get; set; } 

		public double Duration { get; set; } 

		public List<string> pictures { get; set; }	//izmeni

		public Tour() 
		{
			Location = new Location();
            Destinations = new List<Location>();
			pictures = new List<string>();
		}

		public Tour( string name, Location location, string desc, string lang, int maxVisitors, DateTime dt, double duration)
		{
			Name = name;
			Location = location;
			Description = desc;
			Language = lang;
            MaxVisitors = maxVisitors;
            StartTime = dt;
			Duration = duration;
            Destinations = new List<Location>();
			pictures = new List<string>();
		}

		public string[] ToCSV()
		{
			string[] csvValues = { Id.ToString(),
				Name, Location.Id.ToString(),
				Description,
				Language,
				MaxVisitors.ToString(),
				StartTime.ToString(),
				Duration.ToString()
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
			StartTime = DateTime.Parse(values[6]);
			Duration = Convert.ToDouble(values[7]);
		}
	}
}
