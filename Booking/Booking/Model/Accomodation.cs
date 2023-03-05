using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Booking.Serializer;

namespace Booking.Model
{
	internal class Accomodation : ISerializable
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public Location Location { get; set; }

		public string Type { get; set; } // enum

		public int MaxNumberOfGuests { get; set; } // rename

		public int MinNumberOfDays { get; set; } // rename

		public int CancelationPeriod { get; set; } // rename

		public List<string> Pictures { get; set; }

		public Accomodation() 
		{
			Pictures = new List<string>();
		}

		public Accomodation(string name, Location location, string type, int maxNum, int minNum, int cncl) 
		{ 
			Name = name;
			Location = location;
			Type = type;
			MaxNumberOfGuests = maxNum;
			MinNumberOfDays = minNum;
			CancelationPeriod = cncl;
			Pictures = new List<string>();
		}

		public string[] ToCSV()
		{
			string[] csvValues = { Id.ToString(), Name, Location.Id.ToString(), Type, MaxNumberOfGuests.ToString(), MinNumberOfDays.ToString(), CancelationPeriod.ToString() };
			return csvValues;
		}

		public void FromCSV(string[] values)
		{
			Id = Convert.ToInt32(values[0]);
			Name = values[1];
			Location.Id = Convert.ToInt32(values[2]);
			Type = values[3];
			MaxNumberOfGuests = Convert.ToInt32(values[4]);
			MinNumberOfDays = Convert.ToInt32(values[5]);
			CancelationPeriod = Convert.ToInt32(values[6]);
		}
	}
}
