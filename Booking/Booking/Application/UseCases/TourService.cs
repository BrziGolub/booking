using Booking.Domain.RepositoryInterfaces;
using Booking.Domain.ServiceInterfaces;
using Booking.Model;
using Booking.Observer;
using Booking.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Booking.Service
{
	public class TourService : ITourService
	{
		private readonly ITourRepository _tourRepository;
		private readonly ILocationRepository _locationRepository;
		private readonly ITourImageRepository _tourImagesRepository;
		private readonly ITourKeyPointRepository _tourKeyPointsRepository;
		private readonly ITourGuestsRepository _tourGuestsRepository;
		private readonly IUserRepository _userRepository;
		private readonly IVoucherRepository _voucherRepository;

		private readonly List<IObserver> _observers;

		public Voucher voucher;

		public static int SignedGuideId;

		public TourService()
		{
			_tourRepository = InjectorRepository.CreateInstance<ITourRepository>();
			_locationRepository = InjectorRepository.CreateInstance<ILocationRepository>();
			_tourImagesRepository = InjectorRepository.CreateInstance<ITourImageRepository>();
			_tourKeyPointsRepository = InjectorRepository.CreateInstance<ITourKeyPointRepository>();
			_tourGuestsRepository = InjectorRepository.CreateInstance<ITourGuestsRepository>();
			_userRepository = InjectorRepository.CreateInstance<IUserRepository>();
			_voucherRepository = InjectorRepository.CreateInstance<IVoucherRepository>();

			_observers = new List<IObserver>();

			voucher = new Voucher();
		}

        public Tour GetById(int id)
        {
            return _tourRepository.GetById(id);
        }
        public Tour GetByName(string name)
		{
			return _tourRepository.GetByName(name);
		}

		public List<Tour> GetMostVisitedTourGenerally()
		{
			List<Tour> lista = new List<Tour>();

			Tour mostVisitedTour = null;
			int mostVisitedTourID = 0;
			int mostVisits = 0;

			foreach (TourGuests tg in _tourGuestsRepository.GetAll())
			{
				Tour pomTour = _tourRepository.GetById(tg.Tour.Id);

				if (pomTour.GuideId == SignedGuideId && pomTour.IsStarted == false)
				{
					int tourVisits = _tourGuestsRepository.GetAll().Count(m => m.Tour.Id == tg.Tour.Id);

					if (tourVisits > mostVisits)
					{
						mostVisitedTourID = tg.Tour.Id;
						mostVisits = tourVisits;
					}
				}
			}

			mostVisitedTour = _tourRepository.GetById(mostVisitedTourID);
			mostVisitedTour.GuestsAsTour = mostVisits;

			if (mostVisitedTour.GuideId == SignedGuideId && mostVisits > 0)
			{
				mostVisitedTour.Location = _locationRepository.GetById(mostVisitedTour.Location.Id);
				lista.Add(mostVisitedTour);
				return lista;
			}
			else
			{
				return new List<Tour>();
			}
		}

		public List<Tour> GetMostVisitedTourThisYear()
		{
			List<Tour> lista = new List<Tour>();

			Tour mostVisitedTour = null;
			int mostVisitedTourID = 0;
			int mostVisits = 0;

			foreach (TourGuests tg in _tourGuestsRepository.GetAll())
			{
				Tour pomTour = _tourRepository.GetById(tg.Tour.Id);
				if (pomTour.StartTime.Year == DateTime.Now.Year && pomTour.GuideId == SignedGuideId && pomTour.IsStarted == false)
				{
					int tourVisits = _tourGuestsRepository.GetAll().Count(m => m.Tour.Id == tg.Tour.Id);
                    

                    if (tourVisits > mostVisits)
					{
                        
                        mostVisitedTourID = tg.Tour.Id;
						mostVisits = tourVisits;
					}
				}

			}

			mostVisitedTour = _tourRepository.GetById(mostVisitedTourID);
			mostVisitedTour.GuestsAsTour = mostVisits;

            if (mostVisitedTour.GuideId == SignedGuideId && mostVisits > 0)
			{
                mostVisitedTour.Location = _locationRepository.GetById(mostVisitedTour.Location.Id);
                lista.Add(mostVisitedTour);
				return lista;
			}
			else
			{
				return new List<Tour>();
			}

		}


		public List<Tour> GetGuideTours()
		{
			List<Tour> _guideTours = new List<Tour>();

			foreach (var tour in _tourRepository.GetAll())
			{ 
				if (tour.GuideId == SignedGuideId)
				{
					tour.Location = _locationRepository.GetById(tour.Location.Id);
						foreach (var p in _tourImagesRepository.GetAll())
						{
							if (p.Tour.Id == tour.Id)
							{
								tour.Images.Add(p);
							}
						}
						_guideTours.Add(tour);
				}
			}

			return _guideTours;
		}

		public ObservableCollection<Tour> Search(ObservableCollection<Tour> observe, string state, string city, string duration, string language, string visitors)
		{
			observe.Clear();

			foreach (Tour tour in _tourRepository.GetAll())
			{
				//FALI UVEZIVANJE SLIKA I LOKACIJE
				bool isStateSearched = tour.Location.State.Equals(state) || state.Equals("All");
				bool isCitySearche = tour.Location.City.Equals(city) || city.Equals("All");
				bool isLanguageSearched = tour.Language.ToLower().Contains(language.ToLower()) || language.Equals("");

				bool isDurationEmpty = duration.Equals("");
				bool isVisitorsEmpty = visitors.Equals("");

				if (isStateSearched && isCitySearche && isLanguageSearched)
				{
					if (isDurationEmpty && isVisitorsEmpty)
					{
						observe.Add(tour);
					}
					else if (isDurationEmpty && !isVisitorsEmpty)
					{
						if (tour.MaxVisitors >= Convert.ToInt32(visitors))
						{
							observe.Add(tour);
						}
					}
					else if (!isDurationEmpty && isVisitorsEmpty)
					{
						if (tour.Duration == Convert.ToDouble(duration))
						{
							observe.Add(tour);
						}
					}
					else if (tour.MaxVisitors >= Convert.ToInt32(visitors) && tour.Duration == Convert.ToDouble(duration))
					{
						observe.Add(tour);
					}
				}
			}

			return observe;
		}

		public ObservableCollection<Tour> CancelSearch(ObservableCollection<Tour> observe)
		{
			observe.Clear();

			foreach (Tour tour in _tourRepository.GetAll())
			{
				observe.Add(tour);
			}

			return observe;
		}

		public Tour UpdateTour(Tour tour)
		{
			Tour oldTour = _tourRepository.GetById(tour.Id);
			if (oldTour == null) return null;

			oldTour.Name = tour.Name;
			oldTour.Location.Id = tour.Location.Id;
			oldTour.Description = tour.Description;
			oldTour.Language = tour.Language;
			oldTour.MaxVisitors = tour.MaxVisitors;
			oldTour.StartTime = tour.StartTime;
			oldTour.Duration = tour.Duration;
			oldTour.IsStarted = tour.IsStarted;
			oldTour.IsEnded = tour.IsEnded;

			return _tourRepository.Update(tour);
		}

		public TourKeyPoint UpdateKeyPoint(TourKeyPoint tourKeyPoint)
		{	
			TourKeyPoint oldTourKeyPoint = _tourKeyPointsRepository.GetById(tourKeyPoint.Id);

			oldTourKeyPoint.Tour = tourKeyPoint.Tour;
			oldTourKeyPoint.Location = tourKeyPoint.Location;
			oldTourKeyPoint.Achieved = tourKeyPoint.Achieved;

			return _tourKeyPointsRepository.Update(tourKeyPoint);
		}

		public Tour AddTour(Tour tour)
		{
			tour.Id = _tourRepository.NextId();
			foreach (var destination in tour.Destinations)
			{
				destination.Tour = tour;
				_tourKeyPointsRepository.Add(destination);
			}

			foreach (var picture in tour.Images)
			{
				picture.Tour = tour;
				_tourImagesRepository.Add(picture);
			}

			tour.GuideId = SignedGuideId;

			_tourRepository.Add(tour);
			NotifyObservers();

			return tour;
		}

		public void Create(Tour tour)
		{
			AddTour(tour);
		}

		public List<Tour> GetTodayTours()
		{
			List<Tour> _todayTours = new List<Tour>();
			foreach (var tour in _tourRepository.GetAll())
			{
				if (tour.StartTime == DateTime.Today && tour.GuideId == SignedGuideId)
				{
					tour.Location = _locationRepository.GetById(tour.Id);
						foreach (var p in _tourImagesRepository.GetAll())
						{
							if (p.Tour.Id == tour.Id)
							{
								tour.Images.Add(p);
							}
						}
                    _todayTours.Add(tour);
				}
			}
			return _todayTours;
		}

		public List<TourKeyPoint> GetSelectedTourKeyPoints(int idTour)
		{
			List<TourKeyPoint> _selectedKeyPoints = new List<TourKeyPoint>();

			foreach (var keypoint in _tourKeyPointsRepository.GetAll())
			{
				if (keypoint.Tour.Id == idTour)
				{
					keypoint.Location = _locationRepository.GetById(keypoint.Location.Id);

					_selectedKeyPoints.Add(keypoint);
				}
			}
			return _selectedKeyPoints;
		}

		public Tour removeTour(int idTour)
		{
			Tour tour = _tourRepository.GetById(idTour);
			if (tour == null) return null;

			if (tour.IsCancelable())
            {
                CheckVoucher(idTour);
                _tourKeyPointsRepository.DeleteByTourId(idTour);
                _tourImagesRepository.DeleteByTourId(idTour);
                _tourGuestsRepository.DeleteByTourId(idTour);
                _tourRepository.Delete(tour);
                NotifyObservers();
                return tour;
            }
            else
			{
				MessageBox.Show("You can cancel the tour no later than 48 hours before the start!");
				return null;
			}
		}

        private void CheckVoucher(int idTour)
        {
            foreach (TourGuests tg in _tourGuestsRepository.GetAll())
            {
                User pomGuest = _userRepository.GetById(tg.User.Id);

                if (tg.Tour.Id == idTour)
                {
                    voucher.User.Id = pomGuest.Id;
                    voucher.ValidTime = DateTime.Now.AddYears(1);
                    voucher.IsActive = true;
                    _voucherRepository.Add(voucher);
                }
            }
        }

        public bool checkTourGuests(int tourId, int userId) 
		{
			if (_tourGuestsRepository.GetAll().Any(u => u.Tour.Id == tourId && u.User.Id == userId)) 
			{
				return false;
			}
			else
			{
				return true;
			}
		}

		public int numberOfZeroToEighteenGuests(int selectedTourID)
		{
			int sum = 0;

			foreach (var g in _tourGuestsRepository.GetAll())
			{
				User pomGuest = _userRepository.GetById(g.User.Id);
				Tour pomTour = _tourRepository.GetById(g.Tour.Id);

				if (g.Tour.Id == selectedTourID && pomGuest.Years > 0 && pomGuest.Years < 18 && pomTour.IsStarted == false)
				{
					sum += 1;
				}
			}
			return sum;
		}

		public int numberOfEighteenToFiftyGuests(int selectedTourID)
		{
			int sum = 0;

			foreach (var g in _tourGuestsRepository.GetAll())
			{

				User pomGuest = _userRepository.GetById(g.User.Id);
                Tour pomTour = _tourRepository.GetById(g.Tour.Id);

                if (g.Tour.Id == selectedTourID && pomGuest.Years > 17 && pomGuest.Years < 50 && pomTour.IsStarted == false) // >18 i <50 ???
				{
					sum += 1;
				}
			}
			return sum;
		}

		public int numberOfFiftyPlusGuests(int selectedTourID)
		{
			int sum = 0;

			foreach (var g in _tourGuestsRepository.GetAll())
			{
				User pomGuest = _userRepository.GetById(g.User.Id);
                Tour pomTour = _tourRepository.GetById(g.Tour.Id);

                if (g.Tour.Id == selectedTourID && pomGuest.Years >= 50 && pomTour.IsStarted == false)
				{
					sum += 1;
				}
			}
			return sum;
		}

		public int numberWithVouchersGuests(int selectedTourID)
		{
			int sum = 0;

            foreach (var g in _tourGuestsRepository.GetAll())
			{
                User pomGuest = _userRepository.GetById(g.User.Id);
                Tour pomTour = _tourRepository.GetById(g.Tour.Id);

                if (g.Voucher == true && g.Tour.Id == selectedTourID && pomTour.IsStarted == false)
                {
                    sum += 1;
                }
            }
			return sum;
		}

        public int numberWithOutVouchersGuests(int selectedTourID)
        {
            int sum = 0;

            foreach (var g in _tourGuestsRepository.GetAll())
            {
                User pomGuest = _userRepository.GetById(g.User.Id);
                Tour pomTour = _tourRepository.GetById(g.Tour.Id);

                if (g.Voucher == false && g.Tour.Id == selectedTourID && pomTour.IsStarted == false)
                {
                    sum += 1;
                }
            }
            return sum;
        }
        public void NotifyObservers()
		{
			foreach (var observer in _observers)
			{
				observer.Update();
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

		public List<Tour> GetValidTours()
		{
			List<Tour> list = _tourRepository.GetValidTours();
			foreach (Tour tour in list)
			{
				tour.Location = _locationRepository.GetById(tour.Location.Id);
				tour.Destinations = _tourKeyPointsRepository.GetKeyPointsByTourId(tour.Id);
				tour.Images = _tourImagesRepository.GetTourImagesByTourId(tour.Id);
				foreach (TourKeyPoint keyPoint in tour.Destinations)
				{
					keyPoint.Location = _locationRepository.GetById(keyPoint.Location.Id);
				}
			}
			return list;
		}
	}
}
