using Booking.Model;
using Booking.Model.Images;
using Booking.Observer;
using Booking.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Input;

namespace Booking.Service
{
    public class TourService
    {
        private readonly TourRepository _repository;
        private List<Tour> _tours;

		private readonly List<IObserver> _observers;

		private LocationService _locationService;
        private TourImageService _tourImageService;
        private TourKeyPointService _tourKeyPointService;
		private TourGuestsService _tourGuestsService;
		private UserService _userService;

		private List<Location> _locations;
		private List<TourImage> _tourImages;
		private List<TourKeyPoint> _tourKeyPoints;
		private List<TourGuests> _tourGuests;
		private List<User> _users;

		private LocationRepository _locationRepository;
		private TourImageRepository _tourImagesRepository;
		private TourKeyPointRepository _tourKeyPointsRepository;
		private TourGuestsRepository _tourGuestsRepository;
		private UserRepository _userRepository;
		
		public static int SignedGuideId;

		public TourService()
        {
            _repository = new TourRepository();
            _tours = new List<Tour>();

			_observers = new List<IObserver>();

			_locationService = new LocationService();
            _tourImageService = new TourImageService();
            _tourKeyPointService = new TourKeyPointService();
			_tourGuestsService = new TourGuestsService();
			_userService = new UserService();

            _locations = new List<Location>();
            _tourImages = new List<TourImage>();
            _tourKeyPoints = new List<TourKeyPoint>();
			_tourGuests = new List<TourGuests>();
			_users = new List<User>();

			_locationRepository = new LocationRepository();
			_tourImagesRepository = new TourImageRepository();
			_tourKeyPointsRepository = new TourKeyPointRepository();
			_tourGuestsRepository = new TourGuestsRepository();
            _userRepository = new UserRepository();

            Load();
        }

        public void Load()
        {
            _tours = _repository.Load();
			_tourImages = _tourImagesRepository.Load();
			_tourKeyPoints = _tourKeyPointsRepository.Load();
			_locations = _locationRepository.Load();
			_tourGuests = _tourGuestsRepository.Load();
			_users = _userRepository.Load();

			LoadLocations();
            LoadImages();
            LoadKeyPoints();

			LoadLocationsForKeyPoints();
		}

        public void Save()
        {
            _repository.Save(_tours);
        }

        public Tour GetById(int id)
        {
            return _tours.Find(v => v.Id == id);
        }

		public Tour GetByName(string name)
		{
			return _tours.Find(t => t.Name == name);
		}

        public List<Tour> GetAll()
        {
            return _tours;
        }

        public List<Tour> GetMostVisitedTourGenerally()
        {
            List<Tour> lista = new List<Tour>();

            Tour mostVisitedTour = null;
            int mostVisitedTourID = 0;
            int mostVisits = 0;			

				foreach(TourGuests tg in _tourGuests)
				{
					Tour pomTour = GetById(tg.Tour.Id);

					if (pomTour.GuideId == SignedGuideId)
					{
						int tourVisits = _tourGuests.Count(m => m.Tour.Id == tg.Tour.Id);

						if (tourVisits > mostVisits)
						{
							mostVisitedTourID = tg.Tour.Id;
							mostVisits = tourVisits;
						}
					}
				}

            mostVisitedTour = GetById(mostVisitedTourID);

            if (mostVisitedTour.GuideId == SignedGuideId && mostVisits > 0)
				{
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

            foreach (TourGuests tg in _tourGuests)
            {
				Tour pomTour = GetById(tg.Tour.Id);
				if(pomTour.StartTime.Year == DateTime.Now.Year && pomTour.GuideId == SignedGuideId)
				{
					int tourVisits = _tourGuests.Count(m => m.Tour.Id == tg.Tour.Id);

					if (tourVisits > mostVisits)
					{
						mostVisitedTourID = tg.Tour.Id;
						mostVisits = tourVisits;
					}
				}
				
            }

			mostVisitedTour = GetById(mostVisitedTourID);

			if (mostVisitedTour.GuideId == SignedGuideId && mostVisits>0)
			{
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

            foreach (var tour in _tours)
            {
                if (tour.GuideId == SignedGuideId)
                {
                    _guideTours.Add(tour);
                }
            }
			
            return _guideTours;
        }

        public void LoadLocations()
        {
            _locationService.Load();

            foreach (Tour tour in _tours)
            {
                tour.Location = _locationService.GetById(tour.Location.Id);
            }
        }

        public void LoadImages()
        {
            _tourImageService.Load();

            foreach (Tour tour in _tours)
            {
                List<TourImage> images = _tourImageService.GetImagesByTourId(tour.Id);
                foreach (TourImage image in images)
                {
                    tour.Images.Add(image);
                }
            }
        }

        public void LoadKeyPoints()
        {
            _tourKeyPointService.Load();

            foreach (Tour tour in _tours)
            {
                List<TourKeyPoint> keyPoints = _tourKeyPointService.GetKeyPointsByTourId(tour.Id);
                foreach (TourKeyPoint keyPoint in keyPoints)
                {
                    tour.Destinations.Add(keyPoint);
                }
            }
        }

        public ObservableCollection<Tour> Search(ObservableCollection<Tour> observe, string state, string city, string duration, string language, string visitors)
        {
            observe.Clear();

            foreach (Tour tour in _tours)
            {
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

		public void LoadLocationsForKeyPoints()
		{
			_locationService.Load();

			foreach (TourKeyPoint keypoint in _tourKeyPoints)
			{
				keypoint.Location = _locationService.GetById(keypoint.Location.Id);
			}
		}

		public Tour UpdateTour(Tour tour)
		{
			Tour oldTour = GetById(tour.Id);
			if (oldTour == null) return null;

			oldTour.Name = tour.Name;
			oldTour.Location.Id = tour.Location.Id;
			oldTour.Description = tour.Description;
			oldTour.Language = tour.Language;
			oldTour.MaxVisitors = tour.MaxVisitors;
			oldTour.StartTime = tour.StartTime;
			oldTour.Duration = tour.Duration;
			oldTour.IsStarted = tour.IsStarted;

			Save();
			NotifyObservers();
			return oldTour;
		}

		public TourKeyPoint UpdateKeyPoint(TourKeyPoint tourKeyPoint)
		{
			TourKeyPoint oldTourKeyPoint = _tourKeyPointService.GetById(tourKeyPoint.Id);

			oldTourKeyPoint.Tour = tourKeyPoint.Tour;
			oldTourKeyPoint.Location = tourKeyPoint.Location;
			oldTourKeyPoint.Achieved = tourKeyPoint.Achieved;

			_tourKeyPointService.Save();
			NotifyObservers();

			return oldTourKeyPoint;
		}

		public int NextId()
		{
			if (_tours.Count == 0)
			{
				return 1;
			}
			else
			{
				return _tours.Max(t => t.Id) + 1;
			}
		}

		public Tour AddTour(Tour tour)
		{
			tour.Id = NextId();
			foreach (var destination in tour.Destinations)
			{
				destination.Id = _tourKeyPointService.NextId();
				destination.Tour = tour;
				_tourKeyPoints.Add(destination);
			}

			foreach (var picture in tour.Images)
			{
				picture.Id = _tourImageService.NextId();
				picture.Tour = tour;
				_tourImages.Add(picture);
			}
			tour.GuideId = SignedGuideId;

			_tours.Add(tour);
            NotifyObservers();
			Save();
			
            _tourImagesRepository.Save(_tourImages);
			_tourKeyPointsRepository.Save(_tourKeyPoints);

			
			return tour;
		}

		public void Create(Tour tour)
		{
			AddTour(tour);
		}

		public List<Tour> GetTodayTours()
		{
			List<Tour> _todayTours = new List<Tour>();
			foreach (var tour in _tours)
			{
				if (tour.StartTime == DateTime.Today && tour.GuideId == SignedGuideId) 
				{
					_todayTours.Add(tour);
				}
			}
			return _todayTours;
		}

		public List<TourKeyPoint> GetSelectedTourKeyPoints(int idTour)
		{
			List<TourKeyPoint> _selectedKeyPoints = new List<TourKeyPoint>();

			foreach (var keypoint in _tourKeyPoints)
			{
				if (keypoint.Tour.Id == idTour)
				{
					_selectedKeyPoints.Add(keypoint);
				}
			}
			return _selectedKeyPoints;
		}



		public Tour removeTour(int idTour) 
		{
			Tour tour = GetById(idTour);
			if (tour == null) return null;

			if (tour.IsCancelable())
			{

				_tourKeyPoints.RemoveAll(TourKeyPoint => TourKeyPoint.Tour.Id == idTour);
				_tourImages.RemoveAll(TourImage => TourImage.Tour.Id == idTour);

                _tours.Remove(tour);
				
				NotifyObservers();
				
                _tourKeyPointsRepository.Save(_tourKeyPoints);
                _tourImagesRepository.Save(_tourImages);
                _repository.Save(_tours);
				
				return tour;
			}
			else
			{
				MessageBox.Show("You can cancel the tour no later than 48 hours before the start!");
				return null;
			}
		}

		public bool checkTourGuests(int tourId, int userId) //, int keyPointId
        {
			if(_tourGuests.Any(u=> u.Tour.Id == tourId && u.User.Id == userId )) // && u.TourKeyPoint.Id == keyPointId
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

			foreach(var g in _tourGuests)
			{		
				User pomGuest = _userService.GetById(g.User.Id);
				
				if(g.Tour.Id == selectedTourID && pomGuest.Years > 0 && pomGuest.Years < 18)
				{
					sum+=1;
				}
			}	
			return sum;
		}

        public int numberOfEighteenToFiftyGuests(int selectedTourID)
        {
            int sum = 0;

            foreach (var g in _tourGuests)
            {

                User pomGuest = _userService.GetById(g.User.Id);

                if (g.Tour.Id == selectedTourID && pomGuest.Years > 17 && pomGuest.Years < 51) // >18 i <50 ???
                {
                    sum += 1;
                }
            }
            return sum;
        }

        public int numberOfFiftyPlusGuests(int selectedTourID)
        {
            int sum = 0;

            foreach (var g in _tourGuests)
            {
                User pomGuest = _userService.GetById(g.User.Id);

                if (g.Tour.Id == selectedTourID && pomGuest.Years > 50)
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
	}
}
