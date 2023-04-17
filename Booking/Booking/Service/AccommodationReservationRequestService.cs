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
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Booking.Service
{
    public class AccommodationReservationRequestService : IAccommodationReservationRequestService
    {
        private readonly List<IObserver> _observers;
        private readonly IAccommodationReservationRequestsRepostiory _repository;
        private readonly IAccommodationResevationRepository _reservationRepository;
        private readonly IAccommodationRepository _accommodationRepository;
        private readonly ILocationRepository _locationRepository;

        public AccommodationReservationRequestService()
        {
           
            _observers = new List<IObserver>();
            _repository = InjectorRepository.CreateInstance<IAccommodationReservationRequestsRepostiory>();
            _reservationRepository = InjectorRepository.CreateInstance<IAccommodationResevationRepository>();
            _accommodationRepository = InjectorRepository.CreateInstance<IAccommodationRepository>();
            _locationRepository = InjectorRepository.CreateInstance<ILocationRepository>();
        }


        public List<AccommodationReservationRequests> GetAll()
        {
            List<AccommodationReservationRequests> accommodationReservationRequestAll = new List<AccommodationReservationRequests>();
            List<AccommodationReservationRequests> accommodationReservationRequestList = new List<AccommodationReservationRequests>();
            accommodationReservationRequestAll = _repository.GetAll();
            foreach (var arr in accommodationReservationRequestAll)
            {
                arr.AccommodationReservation = _reservationRepository.GetById(arr.AccommodationReservation.Id);
                if(arr.AccommodationReservation.Guest.Id == AccommodationReservationService.SignedFirstGuestId) 
                {
                    accommodationReservationRequestList.Add(arr);
                }
            }

            foreach (var arr in accommodationReservationRequestList)
            {
                arr.AccommodationReservation.Accommodation = _accommodationRepository.GetById(arr.AccommodationReservation.Accommodation.Id);
                arr.AccommodationReservation.Accommodation.Location = _locationRepository.GetById(arr.AccommodationReservation.Accommodation.Location.Id);
            }
            return accommodationReservationRequestList;
        }
        public List<AccommodationReservationRequests> GetSeeableDateChanges() 
        {
            List<AccommodationReservationRequests> accommodationReservationRequestAll = new List<AccommodationReservationRequests>();
            List<AccommodationReservationRequests> accommodationReservationRequestList = new List<AccommodationReservationRequests>();
            accommodationReservationRequestAll = _repository.GetAll();
            foreach (var arr in accommodationReservationRequestAll)
            {
                arr.AccommodationReservation = _reservationRepository.GetById(arr.AccommodationReservation.Id);
                arr.AccommodationReservation.Accommodation = _accommodationRepository.GetById(arr.AccommodationReservation.Accommodation.Id);
                if (arr.AccommodationReservation.Accommodation.Owner.Id == AccommodationService.SignedOwnerId && arr.Status.ToString().Equals("PENDNING"))
                {
                    arr.Accessable = CheckDate(arr);
                    accommodationReservationRequestList.Add(arr);
                }
            }
            return accommodationReservationRequestList;
        }
        public String CheckDate(AccommodationReservationRequests request) 
        {
            List<AccommodationReservation> accommodationReservations = new List<AccommodationReservation>();
            accommodationReservations = _reservationRepository.GetAll();
            foreach (var reservation in accommodationReservations) 
            {
                reservation.Accommodation = _accommodationRepository.GetById(reservation.Accommodation.Id);
                for (DateTime date = request.NewArrivalDay; date <= request.NewDeparuteDay; date=date.AddDays(1)) 
                {
                    for (DateTime reservationDate = reservation.ArrivalDay; reservationDate <= reservation.DepartureDay; reservationDate=reservationDate.AddDays(1)) 
                    {
                        if (date.Equals(reservationDate) && reservation.Accommodation.Owner.Id == request.AccommodationReservation.Accommodation.Owner.Id) 
                        {
                            return "Ocupied";
                        }
                    }
                }
            }
            return "Free";
        }

        public void DeleteRequest(AccommodationReservation selectedReservation)
        {
            _repository.DeleteRequest(selectedReservation);
            NotifyObservers();
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

        public void Create(AccommodationReservation selectedResrevation, DateTime newArrivalDay,DateTime newDepartureDay)
        {
            _repository.Add(selectedResrevation, newArrivalDay, newDepartureDay);
            NotifyObservers();
        }
    }
}
