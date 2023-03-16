using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
			tours = new List<Tour>();
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
				tour.Location = locationDAO.FindById(tour.Id);
			}
		}

		public List<Tour> GetAll()
		{
			return tours;
		}

		public Tour FindById(int id)
		{
			return tours.Find(v => v.Id == id);
		}

		public void Search(ObservableCollection<Tour> observe, string state, string city, string duration, string language, string passengers)
		{
			observe.Clear();

			foreach (Tour tour in tours)
			{
				if ((tour.Location.State.Equals(state) || state.Equals("All")) && (tour.Location.City.Equals(city) || city.Equals("All")) && (tour.Language.ToLower().Contains(language.ToLower()) || language.Equals("")))
				{
					if (duration.Equals("") && passengers.Equals(""))
					{
						observe.Add(tour);
					}
					else if (duration.Equals("") && !passengers.Equals(""))
					{
						if (tour.MaxVisitors >= Convert.ToInt32(passengers))
						{
							observe.Add(tour);
						}
					}
					else if (!duration.Equals("") && passengers.Equals(""))
					{
						if (tour.Duration == Convert.ToDouble(duration))
						{
							observe.Add(tour);
						}
					}
					else
					{
						if (tour.MaxVisitors >= Convert.ToInt32(passengers) && tour.Duration == Convert.ToDouble(duration))
						{
							observe.Add(tour);
						}
					}
				}
			}
		}

		public void CancelSearch(ObservableCollection<Tour> observe)
		{
			observe.Clear();

			foreach (Tour tour in tours)
			{
				observe.Add(tour);
			}
		}

		public List<string> GetAllStates()
		{
			return locationDAO.GetAllStates();
		}

		public ObservableCollection<string> GetAllCitiesByState(ObservableCollection<string> observe, string state)
		{
			return locationDAO.GetAllCitiesByState(observe, state);
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
	}
}
