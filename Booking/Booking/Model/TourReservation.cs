using Booking.Conversion;
using Booking.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace Booking.Model
{
	public class TourReservation : ISerializable
	{
		public int Id { get; set; }

		public User User { get; set; }

		public Tour Tour { get; set; }

		//public Guide Guide { get; set; }

		public int NumberOfVisitors { get; set; }

        public TourReservation()
        {
			User = new User();
			Tour = new Tour();
        }

        public TourReservation(Tour tour, int passengers)
        {
			User = new User();
			Tour = tour;
			NumberOfVisitors = passengers;
		}

		public string[] ToCSV()
		{
			string[] csvValues = {
				Id.ToString(),
				User.Id.ToString(),
				Tour.Id.ToString(),
				NumberOfVisitors.ToString()
			};
			return csvValues;
		}

		public void FromCSV(string[] values)
		{
			Id = Convert.ToInt32(values[0]);
			User.Id = Convert.ToInt32(values[1]);
			Tour.Id = Convert.ToInt32(values[2]);
			NumberOfVisitors = Convert.ToInt32(values[3]);
		}
	}
}
