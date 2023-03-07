using Booking.Model;
using Booking.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Repository
{
	public class TourRepository
	{
		private const string FilePath = "../../Resources/Data/tours.csv";

		private readonly Serializer<Tour> _serializer;

		private List<Tour> _tours;

		public TourRepository()
		{
			_serializer = new Serializer<Tour>();
			_tours = Load();
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
