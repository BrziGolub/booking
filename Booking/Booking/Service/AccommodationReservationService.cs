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
        private readonly IUserRepository _userRepository;
        private readonly IAccommodationReservationRequestService _accommodationReservationRequestRepository;
        private readonly IAccommodationGradeService _accommodationGradeRepository;

        // public AccommodationGradeService AccommodationGradeService { get; set; }
        // public AccommodationReservationRequestService AccommodationReservationRequestService { get; set; }
        // public UserService UserService { get; set; }
        // public AccommodationService AccommodationService { get; set; }
        public static int SignedFirstGuestId;

		public AccommodationReservationService()
        {
            _observers = new List<IObserver>();
            _repository = InjectorRepository.CreateInstance<IAccommodationResevationRepository>();
            _accommodationRepository = InjectorRepository.CreateInstance<IAccommodationRepository>();
            _userRepository = InjectorRepository.CreateInstance<IUserRepository>();
            _accommodationReservationRequestRepository = InjectorRepository.CreateInstance<IAccommodationReservationRequestService>();
            _accommodationGradeRepository = InjectorRepository.CreateInstance<IAccommodationGradeService>();

            //var app = Application.Current as App;

            //AccommodationGradeService = app.AccommodationGradeService;
            //AccommodationService = app.AccommodationService;
            //UserService = app.UserService;

            //AccommodationReservationRequestService = app.AccommodationReservationRequestService;
        }

        public List<AccommodationReservation> GetGeustsReservatonst()
        {
            List<AccommodationReservation> _guestsReservations = new List<AccommodationReservation>();

            foreach(var reservation in _repository.GetAll())
            {
                if(reservation.Guest.Id == SignedFirstGuestId)
                {
                    reservation.Accommodation = _accommodationRepository.GetById(reservation.Accommodation.Id);
                    reservation.Accommodation.Owner = _userRepository.GetById(reservation.Accommodation.Owner.Id);
                    _guestsReservations.Add(reservation);
                }
            }
            return _guestsReservations;
        }
        /*public List<AccommodationReservation> GetAll()
        {
            return _reservations;
        }*/
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
            selectedReservation.Accommodation = _accommodationRepository.GetById(selectedReservation.Accommodation.Id);
            if (DateTime.Now < selectedReservation.ArrivalDay.AddDays(-selectedReservation.Accommodation.CancelationPeriod))
            {
                return false;
            }
            return true;
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
                        //unsuccessful reservation
                        return false;
                    }
                }
            }
            AccommodationReservation newReservation = new AccommodationReservation(selectedAccommodation, arrivalDay, departureDay);

            //successful reservation
            SaveReservation(newReservation);
            return true;
        }
        public List<DateTime> SetReservedDates(DateTime arrivalDay, DateTime departureDay, Accommodation selected)
        {
            //this is list od reserved days
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
            //throw out duplicates
            List<DateTime> uniqueReservedDatesList = reservedDates.Distinct().ToList();

            //sort in ascending order
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

            //find date before

            DateTime endDate = departureDay;
            bool endFlag = true;

            while (endFlag)
            {
                bool isEndValid = false;

                for (int i = 1; i <= difference; i++)
                {
                    foreach (var ReservedDay in reservedDates)
                    {
                        DateTime final_moment = endDate;

                        if (final_moment.AddDays(-i) == ReservedDay || final_moment.AddDays(-i) == DateTime.Now)
                        {
                            isEndValid = true;
                            if (endDate.AddDays(-i) == DateTime.Now)
                            {
                                endFlag = false;
                                break;
                            }
                        }
                    }
                }

                if (isEndValid == false)
                {
                    rangeOfDates.Add((endDate.AddDays(-difference), endDate));
                    endFlag = false;

                }
                else
                {
                    endDate = endDate.AddDays(-1);
                }
            }

            //find date after

            endFlag = true;
            DateTime startDate = arrivalDay;

            while (endFlag)
            {
                bool isEndValid = false;

                for (int i = 1; i <= difference; i++)
                {
                    foreach (var ReservedDay in reservedDates)
                    {
                        DateTime initial_moment = startDate;
                        if (initial_moment.AddDays(i) == ReservedDay)
                        {
                            isEndValid = true;
                        }
                    }
                }

                if (isEndValid == false)
                {
                    rangeOfDates.Add((startDate, startDate.AddDays(difference)));
                    endFlag = false;

                }
                else
                {
                    startDate = startDate.AddDays(1);
                }
            }

            return rangeOfDates;
        }
        public List<(DateTime, DateTime)> SuggestOtherDates(DateTime arrivalDay, DateTime departureDay, Accommodation selectedAccommodation)
        {
            int difference = (departureDay - arrivalDay).Days;

            List<DateTime> reservedDates = new List<DateTime>();
            reservedDates = SetReservedDates(arrivalDay, departureDay, selectedAccommodation);

            //return the list of ranges
            return GetDates(reservedDates, difference, departureDay, arrivalDay);
        }

        /*public void BindReservationToAccommodation()
        {
            _accommodationService.Load();
            foreach (AccommodationReservation reservation in _reservations)
            {
                Accommodation accommodation = _accommodationService.GetById(reservation.Accommodation.Id);
                reservation.Accommodation = accommodation;
            }
        }*/
        /*public void BindReservationToGuest()
        {
            UserService.Load();
            foreach (AccommodationReservation reservation in _reservations)
            {
                User user = UserService.GetById(reservation.Guest.Id);
                reservation.Guest = user;
            }
        }*/


        /*public void BindReservationToAccommodation()

        {
            AccommodationService.Load();
            foreach (AccommodationReservation accommodationReservation in _reservations)
            {
                foreach (Accommodation accommodation in _accommodationRepository.Load())
                {
                    if (accommodation.Id == accommodationReservation.Accommodation.Id)
                    {
                        accommodationReservation.Accommodation = accommodation;
                    }
                }
            }
        }
        */


    }
}
