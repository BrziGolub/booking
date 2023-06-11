using Booking.Domain.Model;
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
        private readonly IAccommodationRenovationRepository _accommodationRenovationRepository;
        
        //dodala 
        private readonly IAccommodationResevationRepository _accommodationReservationRepository;
        public static int SignedOwnerId;

        public AccommodationService()
        {
            _observers = new List<IObserver>();
            _accommodationRepository = InjectorRepository.CreateInstance<IAccommodationRepository>();
            _locationRepository = InjectorRepository.CreateInstance<ILocationRepository>();
            _userRepository = InjectorRepository.CreateInstance<IUserRepository>();
            _accommodationRenovationRepository = InjectorRepository.CreateInstance<IAccommodationRenovationRepository>();

            _accommodationImagesRepository = InjectorRepository.CreateInstance<IAccommodationImagesRepository>();
            _accommodationReservationRepository = InjectorRepository.CreateInstance<IAccommodationResevationRepository>();
        }
        public int GetSignedInOwner()
        {
            return SignedOwnerId;
        }

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
        public List<Accommodation> GetAllSuper()
        {
            List<Accommodation> accommodationList = new List<Accommodation>();
            accommodationList = _accommodationRepository.GetAll();
            List<AccommodationRenovation> renovationList = new List<AccommodationRenovation>();
            renovationList = _accommodationRenovationRepository.GetAll();
            foreach (var r in renovationList)
            {
                r.Accommodation = _accommodationRepository.GetById(r.Accommodation.Id);
            }
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
                if (a.Owner.Super == 1) 
                {
                    a.Name = a.Name + "*";
                }
                foreach (var r in renovationList) 
                {
                    if (!a.Name.Contains("(renovated)") && a.Id == r.Accommodation.Id && r.EndDay.AddYears(1) > DateTime.Now && r.EndDay < DateTime.Now)
                    {
                        a.Name = a.Name + "(renovated)";
                    }
                }
            }
            return accommodationList;
        }
        public bool IsAccommodationTypeValid(Accommodation accommodation, List<String> accommodationTypes)
        {
            bool info = false;

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
        public bool IsCapasityValid(string numberOfGuests, Accommodation accommodation)
        {
            return string.IsNullOrEmpty(numberOfGuests) || int.Parse(numberOfGuests) <= accommodation.Capacity;
        }

        public bool IsCityValid(string city, Accommodation accommodation)
        {
            return string.IsNullOrEmpty(city) || accommodation.Location.City.ToLower().Contains(city.ToLower());
        }

        public bool IsStateValid(string state, Accommodation accommodation)
        {
            return string.IsNullOrEmpty(state) || state.Equals("All") || accommodation.Location.State.ToLower().Contains(state.ToLower());
        }

        public bool IsNameValid(string name, Accommodation accommodation)
        {
            return string.IsNullOrEmpty(name) || accommodation.Name.ToLower().Contains(name.ToLower());
        }

        public bool IsMinNumberOfDaysValid(string minNumDaysOfReservation, Accommodation accommodation)
        {
            return string.IsNullOrEmpty(minNumDaysOfReservation) || int.Parse(minNumDaysOfReservation) >= accommodation.MinNumberOfDays;
        }

        public void Search(ObservableCollection<Accommodation> observe, string name, string city, string state, List<string> accommodationTypes, string numberOfGuests, string minNumDaysOfReservation)
        {
            observe.Clear();

            foreach (Accommodation accommodation in _accommodationRepository.GetAll())
            {
                //binding
                accommodation.Location = _locationRepository.GetById(accommodation.Location.Id);

                foreach (var p in _accommodationImagesRepository.GetAll())
                {
                    if (p.Accomodation.Id == accommodation.Id)
                    {
                        accommodation.Images.Add(p);
                    }
                }

                if (IsNameValid(name, accommodation) && IsStateValid(state, accommodation) && IsAccommodationTypeValid(accommodation, accommodationTypes) && IsCityValid(city, accommodation) && IsCapasityValid(numberOfGuests, accommodation) && IsMinNumberOfDaysValid(minNumDaysOfReservation, accommodation))
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
                //binding
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
        
        public Accommodation AddAccommodation(Accommodation accommodation)
        {

            accommodation.Owner.Id = SignedOwnerId;
            _accommodationRepository.Add(accommodation);
            foreach (var picture in accommodation.Images)
            {
                picture.Accomodation = accommodation;
                _accommodationImagesRepository.Add(picture);
            }
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


        //Quick serach:

      public List<AccommodationReservation> GetReservationsForAccommodation(Accommodation accommodation)
        {
            List<AccommodationReservation> reservations = new List<AccommodationReservation>();
            List<AccommodationReservation> _guestsReservations = new List<AccommodationReservation>();

            foreach (var reservation in _accommodationReservationRepository.GetAll())
            {
                if (reservation.Guest.Id == AccommodationReservationService.SignedFirstGuestId)
                {
                    reservation.Accommodation = _accommodationRepository.GetById(reservation.Accommodation.Id);
                    reservation.Accommodation.Location = _locationRepository.GetById(reservation.Accommodation.Location.Id);
                    reservation.Accommodation.Owner = _userRepository.GetById(reservation.Accommodation.Owner.Id);
                    reservation.Guest = _userRepository.GetById(AccommodationReservationService.SignedFirstGuestId);
                    foreach (var p in _accommodationImagesRepository.GetAll())
                    {
                        if (p.Accomodation.Id == reservation.Accommodation.Id)
                        {
                            reservation.Accommodation.Images.Add(p);
                        }
                    }
                    _guestsReservations.Add(reservation);
                }
            }

            foreach (AccommodationReservation reservation in _guestsReservations) 
            {

                if (reservation.Accommodation.Id == accommodation.Id)
                {
                    reservations.Add(reservation);
                }
            }
            return reservations;
        }

        public List<AccommodationReservation> FindAcceptableReservations(Accommodation accommodation)
        {
            List<AccommodationReservation> allReservations = new List<AccommodationReservation>(GetReservationsForAccommodation(accommodation));
            List<AccommodationReservation> sortedReservations = allReservations.OrderBy(r => r.ArrivalDay).ToList();
            List<AccommodationReservation> filteredReservations = sortedReservations.Where(r => r.ArrivalDay > DateTime.Today).ToList();
            return filteredReservations;
        }

        public List<(DateTime, DateTime)> FindAvailableDatesQuick(Accommodation accommodation, int daysToStay)
        {
            List<AccommodationReservation> allReservations = FindAcceptableReservations(accommodation);
            List<(DateTime, DateTime)> freeDateRanges = new List<(DateTime, DateTime)>();
            int count = 0;

            for (int i = 0; i < allReservations.Count - 1; i++)
            {
                DateTime currentEndDate = allReservations[i].DepartureDay;
                DateTime nextStartDate = allReservations[i + 1].ArrivalDay;

                if ((nextStartDate - currentEndDate).Days >= daysToStay)
                {
                    freeDateRanges.Add((currentEndDate.AddDays(1), currentEndDate.AddDays(daysToStay)));
                    count++;

                    if (count == 3)
                        break;
                }
            }

            if (allReservations.Count == 0)
            {
                DateTime today = DateTime.Now.AddHours(0).AddMinutes(0).AddSeconds(0);
                freeDateRanges.Add((today.AddDays(1), today.AddDays(daysToStay)));
                freeDateRanges.Add((today.AddDays(daysToStay + 1), today.AddDays(daysToStay * 2 + 1)));
                freeDateRanges.Add((today.AddDays(daysToStay * 2 + 2), today.AddDays(daysToStay * 3 + 2)));
            }

            return freeDateRanges;
        }

        public List<(DateTime, DateTime)> FindAvailableDatesQuickRanges(Accommodation accommodation, int daysToStay, DateTime initialDate, DateTime endDate)
        {
            List<AccommodationReservation> allReservations = FindAcceptableReservations(accommodation);

            List<(DateTime, DateTime)> availableRanges = new List<(DateTime, DateTime)>();

            DateTime currentDate = initialDate;
            while (currentDate <= endDate)
            {
                bool isAvailable = IsDateAvailable(currentDate, daysToStay, allReservations);
                if (isAvailable)
                {
                    DateTime rangeEndDate = currentDate.AddDays(daysToStay - 1);
                    if (rangeEndDate <= endDate)
                    {
                        availableRanges.Add((currentDate, rangeEndDate));
                    }
                }

                currentDate = currentDate.AddDays(1);
            }

            return availableRanges;
        }

        public bool IsDateAvailable(DateTime date, int daysToStay, List<AccommodationReservation> reservations)
        {
            foreach (var reservation in reservations)
            {
                if (date >= reservation.ArrivalDay && date <= reservation.DepartureDay)
                {
                    return false;
                }

                for (int i = 1; i < daysToStay; i++)
                {
                    DateTime currentDate = date.AddDays(i);
                    if (currentDate >= reservation.ArrivalDay && currentDate <= reservation.DepartureDay)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public bool CheckGuestsNumber(Accommodation accommodation, int numberOfGuests)
        {
            return accommodation.Capacity >= numberOfGuests;
        }

        public bool AccommodationIsAvailable(Accommodation accommodation, int daysToStay)
        {
            if (FindAvailableDatesQuick(accommodation, daysToStay).Count != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool AccommodationIsAvailableInRange(Accommodation accommodation, int daysToStay, DateTime initialDate, DateTime endDate)
        {
            if (FindAvailableDatesQuickRanges(accommodation, daysToStay, initialDate, endDate).Count != 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
