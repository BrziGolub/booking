using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation.Peers;
using Booking.Observer;
using Booking.Repository;

namespace Booking.Model.DAO
{
	public class TourDAO : ISubject
	{
		private readonly List<IObserver> observers;
		private readonly TourRepository repository;
		private List<Tour> tours;

		private LocationDAO locationDAO;

		public TourDAO()
		{
			repository = new TourRepository();
			observers = new List<IObserver>();
			tours = repository.Load();
			locationDAO = new LocationDAO();
			Load();
		}

		
		public void Load()
		{
			tours = repository.Load();

			AppendLocations();
		}
		

		
		public void AppendLocations() 
		{
			locationDAO.Load();

			foreach (Tour tour in tours)
			{
				tour.Location = locationDAO.FindById(tour.Location.Id);
			}
		}
		
		public List<Tour> GetAll()
		{
			return tours;
		}

		public void NotifyObservers()
		{
			foreach (var observer in observers)
			{
				observer.Update();
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

        public int NextId()
        {
            if (tours.Count == 0)
            {
                return 1;
            }
            else
            {
                return tours.Max(t => t.Id) + 1;
            }
        }
        public Tour addTour(Tour tour)
		{
			tour.Id = NextId();
			tours.Add(tour);
			repository.Save(tours);
			NotifyObservers();
			return tour;
		}

	}
}
