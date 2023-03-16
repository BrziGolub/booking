using Booking.Model;
using Booking.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Service
{
	public class LocationService
	{
		private readonly LocationRepository _repository;
		private List<Location> _locations;

		public LocationService()
		{
			_repository = new LocationRepository();
			_locations = new List<Location>();
			Load();
		}

		public void Load()
		{
			_locations = _repository.Load();
		}

		public void Save()
		{
			_repository.Save(_locations);
		}

		public Location GetById(int id)
		{
			return _locations.Find(v => v.Id == id);
		}

		public List<Location> GetAll()
		{
			return _locations;
		}

		public List<string> GetAllStates()
		{
			List<string> states = new List<string>() { "All" };
			foreach (Location location in _locations)
			{
				if (!states.Contains(location.State))
					states.Add(location.State);
			}
			return states;
		}

		public ObservableCollection<string> GetAllCitiesByState(ObservableCollection<string> observe, string state)
		{
			observe.Clear();
			observe.Add("All");
			foreach (Location location in _locations)
			{
				if (location.State == state)
					observe.Add(location.City);
			}
			return observe;
		}
	}
}
