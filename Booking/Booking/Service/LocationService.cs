using Booking.Model;
using Booking.Observer;
using Booking.Repository;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Booking.Service
{
	public class LocationService : ISubject
	{
		private readonly LocationRepository _repository;
		private List<Location> _locations;
		private readonly List<IObserver> _observers;

		public LocationService()
		{
			_repository = new LocationRepository();
			_locations = new List<Location>();
			_observers = new List<IObserver>();
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

		public int GetIdByCountryAndCity(string Country, string City)
		{
			foreach (var location in _locations)
			{
				if (location.City.Equals(City) && location.State.Equals(Country))
				{
					return location.Id;
				}
			}
			return -1;
		}

		public Location GetByCountryAndCity(string Name)
		{
			string[] pom = Name.Split(',');
			pom[1] = pom[1].Trim();
			foreach (Location loc in _locations)
			{

				if (loc.City == pom[1])
				{
					return loc;
				}
			}
			return null;
		}

		public Location AddLocation(Location location)
		{
			location.Id = NextId();
			_locations.Add(location);
			Save();
			return location;
		}

		public int NextId()
		{
			if (_locations.Count == 0)
			{
				return 1;
			}
			else
			{
				return _locations.Max(l => l.Id) + 1;
			}
		}
		public void Subscribe(IObserver observer)
		{
			_observers.Add(observer);
		}
		public void Unsubscribe(IObserver observer)
		{
			_observers.Remove(observer);
		}
		public void NotifyObservers()
		{
			foreach (var observer in _observers)
			{
				observer.Update();
			}
		}
	}
}
