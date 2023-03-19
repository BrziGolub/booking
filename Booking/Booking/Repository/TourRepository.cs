using Booking.Model;
using Booking.Serializer;
using System.Collections.Generic;

namespace Booking.Repository
{
	public class TourRepository
	{
		private const string FilePath = "../../Resources/Data/tours.csv";

		private readonly Serializer<Tour> _serializer;

		public TourRepository()
		{
			_serializer = new Serializer<Tour>();
		}

		public List<Tour> Load()
		{
			return _serializer.FromCSV(FilePath);
		}

		public void Save(List<Tour> tours)
		{
			_serializer.ToCSV(FilePath, tours);
		}
	}
}
