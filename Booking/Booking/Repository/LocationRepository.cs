using Booking.Model;
using Booking.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Repository
{
	public class LocationRepository
	{
		private const string FilePath = "../../Resources/Data/locations.csv";

		private readonly Serializer<Location> _serializer;

		public LocationRepository()
		{
			_serializer = new Serializer<Location>();
		}

		public List<Location> Load()
		{
			return _serializer.FromCSV(FilePath);
		}

		public void Save(List<Location> locations)
		{
			_serializer.ToCSV(FilePath, locations);
		}
	}
}
