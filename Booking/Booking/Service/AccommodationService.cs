﻿using Booking.Model;
using Booking.Model.Enums;
using Booking.Model.Images;
using Booking.Observer;
using Booking.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Service
{
    public class AccommodationService : ISubject
    {
        private readonly List<IObserver> _observers;
        private readonly AccommodationRepository _accommodationRepository;
        private List<Accommodation> _accommodations;
        private List<AccommodationImage> _accommodationImages;

        private LocationService _locationService;

        private AccommodationImagesRepository _accommodationImagesRepository;

        public AccommodationService()
        {
            _accommodationRepository = new AccommodationRepository();
            _observers = new List<IObserver>();
            _locationService = new LocationService();
            _accommodationImagesRepository = new AccommodationImagesRepository();
            _accommodations = new List<Accommodation>();
            _accommodationImages = new List<AccommodationImage>();
            Load();
        }
        public void Load()
        {
            _accommodations = _accommodationRepository.Load();
            _accommodationImages = _accommodationImagesRepository.Load();
            BindLocationToAccommodaton();
            BindImagesToAccommodaton();
        }
        public Accommodation GetByID(int id)
        {
            return _accommodations.Find(accommodation => accommodation.Id == id);
        }
        public List<Accommodation> GetAll()
        {
            return _accommodations;
        }
        public void BindLocationToAccommodaton()
        {
            _locationService.Load();

            foreach (Accommodation accommodation in _accommodations)
            {
                Location location = _locationService.GetById(accommodation.Location.Id);
                accommodation.Location = location;
            }

        }
        public void BindImagesToAccommodaton()
        {
            foreach (Accommodation accommodation in _accommodations)
            {
                foreach (AccommodationImage accommodationImage in _accommodationImagesRepository.Load())
                {
                    if (accommodationImage.Accomodation.Id == accommodation.Id)
                    {
                        accommodation.Images.Add(accommodationImage);
                    }
                }
            }

        }
        public Boolean IsEnumTrue(Accommodation accommodation, List<String> accommodationTypes)
        {
            Boolean info = false;

            if (accommodationTypes.Count == 0)
            {
                return true;
            }
            foreach (String type in accommodationTypes)
            {
                if (accommodation.Type.ToString().Contains(type))
                {
                    info = true;
                    break;
                }
            }
            return info;
        }
        public void Search(ObservableCollection<Accommodation> observe, string name, string city, string state, List<string> accommodationTypes, string numberOfGuests, string minNumDaysOfReservation)
        {
            observe.Clear();

            foreach (Accommodation accommodation in _accommodations)
            {
                bool isNameValid = string.IsNullOrEmpty(name) || accommodation.Name.ToLower().Contains(name.ToLower());
                bool isStateValid = string.IsNullOrEmpty(state) || state.Equals("All") || accommodation.Location.State.ToLower().Contains(state.ToLower());
                bool isAccommodationTypeValid = IsEnumTrue(accommodation, accommodationTypes);
                bool isCityValid = string.IsNullOrEmpty(city) || accommodation.Location.City.ToLower().Contains(city.ToLower());
                bool isCapacityValid = string.IsNullOrEmpty(numberOfGuests) || int.Parse(numberOfGuests) <= accommodation.Capacity;
                bool isMinNumberOfDaysValid = string.IsNullOrEmpty(minNumDaysOfReservation) || int.Parse(minNumDaysOfReservation) >= accommodation.MinNumberOfDays;

                if (isNameValid && isStateValid && isAccommodationTypeValid && isCityValid && isCapacityValid && isMinNumberOfDaysValid)
                {
                    observe.Add(accommodation);
                }
            }

        }
        public void ShowAll(ObservableCollection<Accommodation> accommodationsObserve)
        {
            accommodationsObserve.Clear();

            foreach (Accommodation accommodation in _accommodations)
            {
                accommodationsObserve.Add(accommodation);
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

        public void NotifyObservers()
        {
            foreach (var observer in _observers)
            {
                observer.Update();
            }
        }
        public int NextId()
        {
            if (_accommodations.Count == 0)
            {
                return 1;
            }
            else
            {
                return _accommodations.Max(t => t.Id) + 1;
            }
        }
        public int ImageNextId()
        {
            if (_accommodationImages.Count == 0)
            {
                return 1;
            }
            else
            {
                return _accommodationImages.Max(t => t.Id) + 1;
            }
        }
        public Accommodation AddAccommodation(Accommodation accommodation)
        {
            accommodation.Id = NextId();
            foreach (var picture in accommodation.Images)
            {
                picture.Id = ImageNextId();
                picture.Accomodation = accommodation;
                _accommodationImages.Add(picture);
            }
            _accommodations.Add(accommodation);
            _accommodationRepository.Save(_accommodations);
            _accommodationImagesRepository.Save(_accommodationImages);
            NotifyObservers();
            return accommodation;
        }
    }
}