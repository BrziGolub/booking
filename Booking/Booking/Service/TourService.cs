using Booking.Model;
using Booking.Model.Images;
using Booking.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Booking.Service
{
    public class TourService
    {
        private readonly TourRepository _repository;
        private List<Tour> _tours;

        private LocationService _locationService;
        private TourImageService _tourImageService;
        private TourKeyPointService _tourKeyPointService;

        public TourService()
        {
            _repository = new TourRepository();
            _tours = new List<Tour>();

            _locationService = new LocationService();
            _tourImageService = new TourImageService();
            _tourKeyPointService = new TourKeyPointService();

            Load();
        }

        public void Load()
        {
            _tours = _repository.Load();

            LoadLocations();
            LoadImages();
            LoadKeyPoints();
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
    }
}
