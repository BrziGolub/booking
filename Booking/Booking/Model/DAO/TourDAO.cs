using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation.Peers;
using Booking.Model.Images;
using Booking.Observer;
using Booking.Repository;

namespace Booking.Model.DAO
{
	public class TourDAO : ISubject
	{
		private readonly List<IObserver> observers;
		private readonly TourRepository repository;
		private List<Tour> tours;
		private List<TourImage> tourImages;
		private TourImagesRepository _tourImagesRepository;

		private LocationDAO locationDAO;

		public TourDAO()
		{
			repository = new TourRepository();
			observers = new List<IObserver>();
			tours = repository.Load();
			locationDAO = new LocationDAO();
			_tourImagesRepository = new TourImagesRepository();
			tourImages = new List<TourImage>();
			Load();
		}

		
		public void Load()
		{
			tours = repository.Load();
			tourImages = _tourImagesRepository.Load();
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

		public void Search(ObservableCollection<Tour> observe, string state, string city, string duration, string language, string passengers)
		{
			observe.Clear();

			foreach (Tour tour in tours)
			{
				if ((tour.Location.State.ToLower().Contains(state.ToLower()) || state.Equals("")) && (tour.Location.City.ToLower().Contains(city.ToLower()) || city.Equals("")) && (tour.Language.ToLower().Contains(language.ToLower()) || language.Equals("")))
				{
					if (duration.Equals("") && passengers.Equals(""))
					{
						observe.Add(tour);
					}
					else if (duration.Equals("") && !passengers.Equals(""))
					{
						if (tour.MaxGuestsNumber >= Convert.ToInt32(passengers))
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
						if (tour.MaxGuestsNumber >= Convert.ToInt32(passengers) && tour.Duration == Convert.ToDouble(duration))
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

       public int ImageNextId()
        {
            if (tourImages.Count == 0)
            {
                return 1;
            }
            else
            {
                return tourImages.Max(t => t.Id) + 1;
            }
        }
        public Tour addTour(Tour tour)
		{
			tour.Id = NextId();
			foreach(var picture in tour.Images)
			{
				picture.Id = ImageNextId();
				picture.Tour = tour; 
				tourImages.Add(picture);
			}
			tours.Add(tour);
			repository.Save(tours);
			_tourImagesRepository.Save(tourImages);
			NotifyObservers();
			return tour;
		}

	}
}
