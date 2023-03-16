using Booking.Model;
using Booking.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Repository
{
	public class TourReservationRepository
	{
		private const string FilePath = "../../Resources/Data/tourReservations.csv";

		private readonly Serializer<TourReservation> _serializer;

		public TourReservationRepository()
		{
			_serializer = new Serializer<TourReservation>();
		}

		public List<TourReservation> Load()
		{
			return _serializer.FromCSV(FilePath);
		}

		public void Save(List<TourReservation> tourReservations)
		{
			_serializer.ToCSV(FilePath, tourReservations);
		}
	}
}
