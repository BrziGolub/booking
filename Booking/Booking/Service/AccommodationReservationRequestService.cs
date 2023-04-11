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

        //private AccommodationReservationService _reservationService;
        //private readonly IAccommodationReservationService _reservationService;
        //private List<AccommodationReservationRequests> _requests;


        public AccommodationReservationRequestService()
        {
            //var app = Application.Current as App;
            //_reservationService = app.AccommodationReservationService;
            //_reservationService = InjectorService.CreateInstance<IAccommodationReservationService>();
            
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

        /*public void BindReservationToRequest()
        {
            _reservationService.Load();
            foreach(var request in _requests)
            {
                AccommodationReservation reservation = _reservationService.GetById(request.AccommodationReservation.Id);
                request.AccommodationReservation = reservation;
            }
        }*/
      

        public void Create(AccommodationReservation selectedResrevation, DateTime newArrivalDay,DateTime newDepartureDay,String comment)
        {
            _repository.Add(selectedResrevation, newArrivalDay, newDepartureDay, comment);
            NotifyObservers();
        }
    }
}
