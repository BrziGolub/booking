using Booking.Model;
using Booking.Model.DAO;
using Booking.Model.Images;
using Booking.Observer;
using Booking.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Booking.Service
{
	public class TourService
	{
		private readonly TourRepository _repository;
		private List<Tour> _tours;

		private LocationService _locationService;
        private List<Location> _locations;
        private LocationRepository _locationRepository;
        //-------------------------------------------------------
        private readonly List<IObserver> _observers;
        //-------------------------------------------------------
        private List<TourImage> _tourImages;
        private TourImagesRepository _tourImagesRepository;
        //-------------------------------------------------------
        private List<TourKeyPoints> _tourKeyPoints;
        private TourKeyPointsRepository _tourKeyPointsRepository;
        //-------------------------------------------------------

        public TourService()
		{
			_repository = new TourRepository();
			_tours = new List<Tour>();
			_locations = new List<Location>();
            _observers = new List<IObserver>();
            //--------------------------------------------
            _locationService = new LocationService();
            _locationRepository = new LocationRepository();
            _locations = new List<Location>();
            //--------------------------------------------
            _tourKeyPointsRepository = new TourKeyPointsRepository();
            _tourKeyPoints = new List<TourKeyPoints>();
            //--------------------------------------------
            _tourImagesRepository = new TourImagesRepository();
            _tourImages = new List<TourImage>();
            //--------------------------------------------

            Load();
		}

		public void Load()
		{
			_tours = _repository.Load();
			_tourImages = _tourImagesRepository.Load();
            _tourKeyPoints = _tourKeyPointsRepository.Load();
            _locations = _locationRepository.Load();

			LoadLocationsForKeyPoints();
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

        public void LoadLocationsForKeyPoints()
        {
            _locationService.Load();

            foreach (TourKeyPoints keypoint in _tourKeyPoints)
            {
                keypoint.Location = _locationService.GetById(keypoint.Location.Id);
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

        public List<Tour> GetTodayTours()
        {
            List<Tour> _todayTours = new List<Tour>();
            foreach (var tour in _tours)
            {
                if (tour.StartTime == DateTime.Today)
                {
                    _todayTours.Add(tour);
                }
            }
            return _todayTours;
        }

        public TourKeyPoints GetKeyPointById(int id)
        {
            return _tourKeyPoints.Find(t => t.Id == id);
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
            oldTour.isStarted = tour.isStarted;

            _repository.Save(_tours);
            NotifyObservers();

            return oldTour;
        }

        public TourKeyPoints UpdateKeyPoint(TourKeyPoints tourKeyPoint)
        {
            TourKeyPoints oldTourKeyPoint = GetKeyPointById(tourKeyPoint.Id);

            oldTourKeyPoint.Tour = tourKeyPoint.Tour;
            oldTourKeyPoint.Location = tourKeyPoint.Location;
            oldTourKeyPoint.Achieved = tourKeyPoint.Achieved;

            _tourKeyPointsRepository.Save(_tourKeyPoints);
            NotifyObservers();

            return oldTourKeyPoint;
        }

        /*public List<TourKeyPoints> UpdateKeyPoints(List<TourKeyPoints> tourKeyPoints)
        {
            List<TourKeyPoints> keyPoints = GetAllKeyPoints();
            foreach (var tour in tourKeyPoints)
            {
                UpdateKeyPoint(tour); 
            }
            return keyPoints;
        }*/

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

        public int ImageNextId()
        {
            if (_tourImages.Count == 0)
            {
                return 1;
            }
            else
            {
                return _tourImages.Max(t => t.Id) + 1;
            }
        }

        public int KeyPointsNextId()
        {
            if (_tourKeyPoints.Count == 0)
            {
                return 1;
            }
            else
            {
                return _tourKeyPoints.Max(t => t.Id) + 1;
            }
        }

        public List<TourKeyPoints> GetSelectedTourKeyPoints(int idTour)
        {
            List<TourKeyPoints> _selectedKeyPoints = new List<TourKeyPoints>();

            foreach (var keypoint in _tourKeyPoints)
            {
                if (keypoint.Tour.Id == idTour)
                {
                    _selectedKeyPoints.Add(keypoint);
                }
            }
            return _selectedKeyPoints;
        }

        public List<TourKeyPoints> GetAllKeyPoints()
        {
            List<TourKeyPoints> _keyPoints = new List<TourKeyPoints>();

            foreach (var keypoint in _tourKeyPoints)
            {
                _keyPoints.Add(keypoint);
            }
            return _keyPoints;
        }

        public Tour addTour(Tour tour)
        {
            tour.Id = NextId();
            foreach (var destination in tour.Destinations)
            {
                destination.Id = KeyPointsNextId();
                destination.Tour = tour;
                _tourKeyPoints.Add(destination);
            }

            foreach (var picture in tour.Images)
            {
                picture.Id = ImageNextId();
                picture.Tour = tour;
                _tourImages.Add(picture);
            }
            _tours.Add(tour);
            _repository.Save(_tours);
            _tourImagesRepository.Save(_tourImages);
            _tourKeyPointsRepository.Save(_tourKeyPoints);

            NotifyObservers();
            return tour;
        }

        public void Create(Tour tour)
        {
            addTour(tour);
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
