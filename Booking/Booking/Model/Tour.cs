using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Booking.Serializer;

namespace Booking.Model
{
	internal class Tour : ISerializable
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public Location Location { get; set; }

		public string Description { get; set; }

		public string Language { get; set; } // enum

		public int MaxNumberOfGuests { get; set; }

		public List<string> destinations { get; set; }

		public DateTime DateTime { get; set; } // rename

		public int Duration { get; set; } // rename

		public List<string> pictures { get; set; }

		public Tour() 
		{
			destinations = new List<string>();
			pictures = new List<string>();
		}

		public Tour( string name, Location location, string desc, string lang, int maxNum, DateTime dt, int duration)
		{
			Name = name;
			Location = location;
			Description = desc;
			Language = lang;
			MaxNumberOfGuests = maxNum;
			DateTime = dt;
			Duration = duration;
			destinations = new List<string>();
			pictures = new List<string>();
		}

		public string[] ToCSV()
		{
			string[] csvValues = { Id.ToString(), Name, Location.Id.ToString(), Description, Language, MaxNumberOfGuests.ToString(), DateTime.ToString(), Duration.ToString()};
			return csvValues;
		}

		public void FromCSV(string[] values)
		{
			Id = Convert.ToInt32(values[0]);
			Name = values[1];
			Location.Id = Convert.ToInt32(values[2]);
			Description = values[3];
			Language = values[4];
			MaxNumberOfGuests = Convert.ToInt32(values[5]);
			//DateTime = 
			Duration = Convert.ToInt32(values[7]);
		}
	}
}
