using Booking.Domain.RepositoryInterfaces;
using Booking.Domain.ServiceInterfaces;
using Booking.Model;
using Booking.Model.Enums;
using Booking.Model.Images;
using Booking.Observer;
using Booking.Repository;
using Booking.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Booking.Service
{
    public class AccommodationService : ISubject, IAccommodationService
    {
        private readonly List<IObserver> _observers;
        private readonly IAccommodationRepository _accommodationRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAccommodationImagesRepository _accommodationImagesRepository;
        public static int SignedOwnerId;


        //private List<Accommodation> _accommodations;
        //private List<AccommodationImage> _accommodationImages;

        //private LocationService _locationService;
        //private UserService _userService;
        //private readonly ILocationService _locationService;
        //private readonly IUserService _userService;



        public AccommodationService()
        {
            _observers = new List<IObserver>();
            _accommodationRepository = InjectorRepository.CreateInstance<IAccommodationRepository>();
            _locationRepository = InjectorRepository.CreateInstance<ILocationRepository>();
            _userRepository = InjectorRepository.CreateInstance<IUserRepository>();


            //var app = Application.Current as App;
            //_locationService = app.LocationService;
            //_userService = new UserService();

            _accommodationImagesRepository = InjectorRepository.CreateInstance<IAccommodationImagesRepository>();
        }
        /*
        public void BindUserToAccommodation()
        {
            _userService.Load();
            foreach(Accommodation accommodation in _accommodations)
            {
                User user = _userService.GetById(accommodation.Owner.Id);
                accommodation.Owner = user;
            }
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

        }*/
        public List<Accommodation> GetAll() 
        {
            List<Accommodation> accommodationList = new List<Accommodation>();
            accommodationList = _accommodationRepository.GetAll();
            foreach (var a in accommodationList) 
            {
                a.Location = _locationRepository.GetById(a.Location.Id);
                a.Owner = _userRepository.GetById(a.Owner.Id);
                foreach (var p in _accommodationImagesRepository.GetAll()) 
                {
                    if (p.Accomodation.Id == a.Id)
                    {
                        a.Images.Add(p);
                    }
                }
            }
            return accommodationList;
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

            foreach (Accommodation accommodation in _accommodationRepository.GetAll())
            {
                accommodation.Location = _locationRepository.GetById(accommodation.Location.Id);
                foreach (var p in _accommodationImagesRepository.GetAll())
                {
                    if (p.Accomodation.Id == accommodation.Id)
                    {
                        accommodation.Images.Add(p);
                    }
                }
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

            foreach (Accommodation accommodation in _accommodationRepository.GetAll())
            {
                accommodation.Location = _locationRepository.GetById(accommodation.Location.Id);
                foreach (var p in _accommodationImagesRepository.GetAll())
                {
                    if (p.Accomodation.Id == accommodation.Id)
                    {
                        accommodation.Images.Add(p);
                    }
                }
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
        
        /*public int ImageNextId()
        {
            if (_accommodationImages.Count == 0)
            {
                return 1;
            }
            else
            {
                return _accommodationImages.Max(t => t.Id) + 1;
            }
        }*/
        public Accommodation AddAccommodation(Accommodation accommodation)
        {
            foreach (var picture in accommodation.Images)
            {
                picture.Accomodation = accommodation;
                _accommodationImagesRepository.Add(picture);
            }

            accommodation.Owner.Id = SignedOwnerId;
            _accommodationRepository.Add(accommodation);
            NotifyObservers();
            return accommodation;
        }

        public List<Accommodation> GetOwnerAccommodations()
        {
            List<Accommodation> _ownerAccommodations = new List<Accommodation>();

            foreach (var accommodation in _accommodationRepository.GetAll())
            {
                if (accommodation.Owner.Id == SignedOwnerId)
                {
                    accommodation.Location = _locationRepository.GetById(accommodation.Location.Id);
                    foreach (var p in _accommodationImagesRepository.GetAll())
                    {
                        if (p.Accomodation.Id == accommodation.Id)
                        {
                            accommodation.Images.Add(p);
                        }
                    }
                    _ownerAccommodations.Add(accommodation);
                }
            }

            return _ownerAccommodations;
        }

    }
}
