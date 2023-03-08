using Booking.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Model.DAO
{
	internal class LocationDAO
	{
		private readonly LocationRepository repository;
		private List<Location> locations;

		public LocationDAO()
		{
			repository = new LocationRepository();
			locations = new List<Location>();
			Load();
		}

		public void Load()
		{
			locations = repository.Load();
		}

		public List<Location> GetAll()
		{
			return locations;
		}

		public Location FindById(int id)
		{
			return locations.Find(v => v.Id == id);
		}
	}
}
