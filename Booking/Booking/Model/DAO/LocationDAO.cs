using Booking.Observer;
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
        private readonly List<IObserver> observers;

        public LocationDAO()
		{
			repository = new LocationRepository();
			locations = new List<Location>();
			observers = new List<IObserver>();
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
