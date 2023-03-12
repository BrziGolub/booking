using Booking.Observer;
using Booking.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Booking.Model;

namespace Booking.Model.DAO
{
	public class LocationDAO : ISubject
	{
		private readonly LocationRepository repository;
		private  List<Location> locations;
        private readonly List<IObserver> observers;

        public LocationDAO()
		{
			repository = new LocationRepository();
			locations = repository.Load();
			observers = new List<IObserver>();			
		}

        /*
        public void Load()
        {
            locations = repository.Load();
        }
        */

        public List<Location> GetAll()
		{
			return locations;
		}

		public Location FindById(int id)
		{
			return locations.Find(v => v.Id == id);
		}

		public Location addLocation(Location location) 
		{
		    location.Id = NextId();
			locations.Add(location);
			repository.Save(locations);
			NotifyObservers();
			return location;
		}

        public int NextId()
        {
            if (locations.Count == 0)
            {
                return 1;
            }
            else
            {
                return locations.Max(l => l.Id) + 1;
            }
        }

        public List<Location> getAllLocations()
        {
            return locations;
        }
        public Location GetLocationById(int id)
        {
            return locations.Find(l => l.Id == id);
        }

        public void Subscribe(IObserver observer)
		{
            observers.Add(observer);
        }
        public void Unsubscribe(IObserver observer)
        {
			observers.Remove(observer);
        }
        public void NotifyObservers()
        {
            foreach (var observer in observers)
            {
                observer.Update();
            }
        }

    }
}
