using Booking.Model;
using Booking.Model.DAO;
using Booking.Observer;
using Booking.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Booking.Service
{
	public class TourService
	{
		private readonly TourRepository _repository;
		private List<Tour> _tours;

		private LocationService _locationService;

		public TourService()
		{
			_repository = new TourRepository();
			_tours = new List<Tour>();

			_locationService = new LocationService();

			Load();
		}

		public void Load()
		{
			_tours = _repository.Load();

			LoadLocations();
		}

		public void Save()
		{
			_repository.Save(_tours);
		}

		public Tour GetById(int id)
		{
			return _tours.Find(v => v.Id == id);
		}

		public List<Tour> GetAll()
		{
			return _tours;
		}

		public void LoadLocations()
		{
			_locationService.Load();

			foreach (Tour tour in _tours)
			{
				tour.Location = _locationService.GetById(tour.Id);
			}
		}

		public ObservableCollection<Tour> Search(ObservableCollection<Tour> observe, string state, string city, string duration, string language, string passengers)
		{
			observe.Clear();

			foreach (Tour tour in _tours)
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

			return observe;
		}

		public ObservableCollection<Tour> CancelSearch(ObservableCollection<Tour> observe)
		{
			observe.Clear();

			foreach (Tour tour in _tours)
			{
				observe.Add(tour);
			}

			return observe;
		}

		public List<string> GetAllStates()
		{
			return _locationService.GetAllStates();
		}

		public ObservableCollection<string> GetAllCitiesByState(ObservableCollection<string> observe, string state)
		{
			return _locationService.GetAllCitiesByState(observe, state);
		}
	}
}
