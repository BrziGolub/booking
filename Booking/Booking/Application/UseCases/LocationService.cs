using Booking.Domain.RepositoryInterfaces;
using Booking.Domain.ServiceInterfaces;
using Booking.Model;
using Booking.Observer;
using Booking.Util;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Booking.Service
{
	public class LocationService : ISubject, ILocationService
	{
		private readonly ILocationRepository _locationRepository;
		private readonly List<IObserver> _observers;

		public LocationService()
		{
			_locationRepository = InjectorRepository.CreateInstance<ILocationRepository>();
			_observers = new List<IObserver>();
		}

		public List<string> GetAllStates()
		{
			List<string> states = new List<string>() { "All" };
			foreach (Location location in _locationRepository.GetAll())
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
			foreach (Location location in _locationRepository.GetAll())
			{
				if (location.State == state)
					observe.Add(location.City);
			}
			return observe;
		}

		public int GetIdByCountryAndCity(string Country, string City)
		{
			foreach (var location in _locationRepository.GetAll())
			{
				if (location.City.Equals(City) && location.State.Equals(Country))
				{
					return location.Id;
				}
			}
			return -1;
		}

		public List<Location> GetAll()
		{
			return _locationRepository.GetAll();
		}

        public Location GetById(int id)
        {
            return _locationRepository.GetById(id);
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
