using Booking.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

		public List<string> GetAllStates()
		{
			List<string> states = new List<string>() { "All" };
			foreach (Location location in locations) 
			{
				if(!states.Contains(location.State))
					states.Add(location.State);
			}
			return states;
		}

		public ObservableCollection<string> GetAllCitiesByState(ObservableCollection<string> observe, string state) 
		{
			observe.Clear();
			observe.Add("All");
			foreach (Location location in locations) 
			{
				if(location.State == state)
					observe.Add(location.City);
			}
			return observe;
		}
	}
}
