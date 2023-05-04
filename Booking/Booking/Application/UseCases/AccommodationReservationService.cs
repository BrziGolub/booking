using Booking.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Booking.Model;
using Booking.Observer;
using Booking.Domain.ServiceInterfaces;
using Booking.Util;
using Booking.Domain.RepositoryInterfaces;

namespace Booking.Service
{
    public class AccommodationReservationService : ISubject, IAccommodationReservationService
    {
        private readonly List<IObserver> _observers;
        private readonly IAccommodationResevationRepository _repository;
        private readonly IAccommodationRepository _accommodationRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAccommodationReservationRequestsRepostiory _accommodationReservationRequestRepository;
        private readonly IAccommodationGradeRepository _accommodationGradeRepository;
        public static int SignedFirstGuestId;
		public AccommodationReservationService()
        {
            _observers = new List<IObserver>();
            _repository = InjectorRepository.CreateInstance<IAccommodationResevationRepository>();
            _accommodationRepository = InjectorRepository.CreateInstance<IAccommodationRepository>();
            _userRepository = InjectorRepository.CreateInstance<IUserRepository>();
            _locationRepository = InjectorRepository.CreateInstance<ILocationRepository>();
            _accommodationReservationRequestRepository = InjectorRepository.CreateInstance<IAccommodationReservationRequestsRepostiory>();
            _accommodationGradeRepository = InjectorRepository.CreateInstance<IAccommodationGradeRepository>();
        }

        public int GetSignedInFirstGuest()
        {
            return SignedFirstGuestId;
        }
        public AccommodationReservation GetById(int id)
        {
           return _repository.GetById(id);
        }

        public List<AccommodationReservation> GetGeustsReservatonst()
        {
            List<AccommodationReservation> _guestsReservations = new List<AccommodationReservation>();

            //binding
            foreach (var reservation in _repository.GetAll())
            {
                if (reservation.Guest.Id == SignedFirstGuestId)
                {
                    reservation.Accommodation = _accommodationRepository.GetById(reservation.Accommodation.Id);
                    reservation.Accommodation.Location = _locationRepository.GetById(reservation.Accommodation.Location.Id);
                    reservation.Accommodation.Owner = _userRepository.GetById(reservation.Accommodation.Owner.Id);
                    reservation.Guest = _userRepository.GetById(SignedFirstGuestId);
                    _guestsReservations.Add(reservation);
                }
            }
            return _guestsReservations;
        }
        public void SaveReservation(AccommodationReservation reservation)
        {
            _repository.Add(reservation);
        }

        public void Delete(AccommodationReservation selectedReservation)
        {
            _repository.Delete(selectedReservation);
            _accommodationReservationRequestRepository.DeleteRequest(selectedReservation);
            NotifyObservers();
        }

        public bool IsAbleToCancleResrvation(AccommodationReservation selectedReservation)
        {
            DateTime currentDate = DateTime.Now;
            DateTime cancellationDate = selectedReservation.ArrivalDay.AddDays(-selectedReservation.Accommodation.CancelationPeriod);
            return (currentDate <= cancellationDate);
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
        public List<DateTime> MakeListOfReservedDates(DateTime initialDate, DateTime endDate)
        {
            List<DateTime> reservedDates = new List<DateTime>();
            for (DateTime date = initialDate; date <= endDate; date = date.AddDays(1))
            {
                reservedDates.Add(date);
            }
            return reservedDates;
        }
        public bool IsDatesMatche(List<DateTime> reservedDatesEntered, List<DateTime> reservedDates)
        {
            foreach (DateTime date in reservedDates)
            {
                foreach (DateTime dateEntered in reservedDatesEntered)
                {
                    if (dateEntered == date)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        public bool Reserve(DateTime arrivalDay, DateTime departureDay, Accommodation selectedAccommodation)
        {
            List<DateTime> reservedDatesEntered = MakeListOfReservedDates(arrivalDay, departureDay);

            foreach (AccommodationReservation reservation in _repository.GetAll())
            {
                if (reservation.Accommodation.Id == selectedAccommodation.Id)
                {
                    List<DateTime> reservedDates = MakeListOfReservedDates(reservation.ArrivalDay, reservation.DepartureDay);

                    if (IsDatesMatche(reservedDatesEntered, reservedDates) == false)
                    {
                        return false;
                    }
                }
            }
            AccommodationReservation newReservation = new AccommodationReservation(selectedAccommodation, arrivalDay, departureDay, AccommodationReservationService.SignedFirstGuestId);
            SaveReservation(newReservation);
            return true;
        }
        public List<DateTime> SetReservedDates(DateTime arrivalDay, DateTime departureDay, Accommodation selected)
        {
            List<DateTime> reservedDates = new List<DateTime>();

            foreach (AccommodationReservation r in _repository.GetAll())
            {
                if (r.Accommodation.Id == selected.Id)
                {
                    for (DateTime date = r.ArrivalDay; date <= r.DepartureDay; date = date.AddDays(1))
                    {
                        reservedDates.Add(date);
                    }
                }
            }
            List<DateTime> uniqueReservedDatesList = reservedDates.Distinct().ToList();
            uniqueReservedDatesList.Sort();
            return uniqueReservedDatesList;
        }
        public bool IsReservationAvailableToGrade(AccommodationReservation accommodationReservation)
        {
            return accommodationReservation.DepartureDay <= DateTime.Now && accommodationReservation.DepartureDay.AddDays(5) >= DateTime.Now;

        }
        public List<AccommodationReservation> GetAllUngradedReservations()
        {
            List<AccommodationReservation> reservationList = new List<AccommodationReservation>();
            foreach (var reservation in _repository.GetAll())
            {
                if (IsReservationAvailableToGrade(reservation) == false)
                {
                    continue;
                }
                bool flag = _accommodationGradeRepository.IsReservationGraded(reservation.Id);
                reservation.Accommodation = _accommodationRepository.GetById(reservation.Accommodation.Id);
                reservation.Accommodation.Owner = _userRepository.GetById(reservation.Accommodation.Owner.Id);
                if (!flag && reservation.Accommodation.Owner.Id == AccommodationService.SignedOwnerId)
                {
                    reservationList.Add(reservation);
                }
            }
            return reservationList;
        }
        public List<(DateTime, DateTime)> GetDates(List<DateTime> reservedDates, int difference, DateTime departureDay, DateTime arrivalDay)
        {
            List<(DateTime, DateTime)> rangeOfDates = new List<(DateTime, DateTime)>();

            DateTime endDate = FindDateAfter(reservedDates, difference, departureDay);
            rangeOfDates.Add((endDate.AddDays(-difference), endDate));

            DateTime startDate = FindDateBefore(reservedDates, difference, arrivalDay);
            rangeOfDates.Add((startDate, startDate.AddDays(difference)));
            return rangeOfDates;
        }

        private DateTime FindDateAfter(List<DateTime> reservedDates, int difference, DateTime departureDay)
        {
            DateTime endDate = departureDay;
            while (true)
            {
                bool isEndValid = IsDateValid(reservedDates, endDate, difference);

                if (!isEndValid)
                {
                    return endDate;
                }
                else
                {
                    endDate = endDate.AddDays(-1);
                }
            }
        }
        private DateTime FindDateBefore(List<DateTime> reservedDates, int difference, DateTime arrivalDay)
        {
            DateTime startDate = arrivalDay;
            while (true)
            {
                bool isStartValid = IsDateValid(reservedDates, startDate, difference);

                if (!isStartValid)
                {
                    return startDate;
                }
                else
                {
                    startDate = startDate.AddDays(1);
                }
            }
        }
        private bool IsDateValid(List<DateTime> reservedDates, DateTime date, int difference)
        {
            for (int i = 1; i <= difference; i++)
            {
                DateTime checkDate = date.AddDays(-i);

                if (checkDate == DateTime.Now)
                {
                    return true;
                }

                if (reservedDates.Contains(checkDate))
                {
                    return true;
                }
            }
            return false;
        }
        public List<(DateTime, DateTime)> SuggestOtherDates(DateTime arrivalDay, DateTime departureDay, Accommodation selectedAccommodation)
        {
            int difference = (departureDay - arrivalDay).Days;

            List<DateTime> reservedDates = new List<DateTime>();
            reservedDates = SetReservedDates(arrivalDay, departureDay, selectedAccommodation);

            return GetDates(reservedDates, difference, departureDay, arrivalDay);
        }
    }
}
