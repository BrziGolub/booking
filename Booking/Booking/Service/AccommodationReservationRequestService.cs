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
    public class AccommodationReservationRequestService : ISubject, IAccommodationReservationRequestService
    {
        private readonly AccommodationReservationRequestsRepostiory _repository;
        private List<AccommodationReservationRequests> _requests;

        //private AccommodationReservationService _reservationService;
        private readonly IAccommodationReservationService _reservationService;

        private readonly List<IObserver> _observers;

		public AccommodationReservationRequestService()
        {
            //var app = Application.Current as App;
            //_reservationService = app.AccommodationReservationService;
            _reservationService = InjectorService.CreateInstance<IAccommodationReservationService>();
            
            _repository = new AccommodationReservationRequestsRepostiory();
            _requests = new List<AccommodationReservationRequests>();
            _observers = new List<IObserver>();
          
            Load();
        }

        public void Load()
        {
            _requests = _repository.Load();
            BindReservationToRequest();
        }

        public void Save()
        {
            _repository.Save(_requests);
        }

        public AccommodationReservationRequests GetById(int id)
        {
            return _requests.Find(v => v.Id == id);
        }

        public List<AccommodationReservationRequests> GetAll()
        {
            return _requests;
        }

        public int NextId()
        {
            if (_requests.Count == 0)
            {
                return 1;
            }
            else
            {
                return _requests.Max(l => l.Id) + 1;
            }
        }

        public void DeleteRequest(AccommodationReservation selectedReservation)
        {
             /*foreach(var request in _repository.Load())
             {
                 if(request.AccommodationReservation.Id == selectedReservation.Id)
                 {
                     _requests.Remove(request);

                 }
             }*/
           _requests.RemoveAll(request => request.AccommodationReservation.Id == selectedReservation.Id);

            Save();
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

        public void BindReservationToRequest()
        {
            _reservationService.Load();
            foreach(var request in _requests)
            {
                AccommodationReservation reservation = _reservationService.GetById(request.AccommodationReservation.Id);
                request.AccommodationReservation = reservation;
            }
        }
      

        public void Create(AccommodationReservation selectedResrevation, DateTime newArrivalDay,DateTime newDepartureDay,String comment)
        {
            AccommodationReservationRequests newRequest = new AccommodationReservationRequests();

            newRequest.Id = NextId();
            newRequest.AccommodationReservation = selectedResrevation;
            newRequest.NewArrivalDay = newArrivalDay;
            newRequest.NewDeparuteDay = newDepartureDay;
            newRequest.Status = RequestStatus.PENDNING;
            newRequest.Comment = comment;
            _requests.Add(newRequest);
            Save();
            NotifyObservers();
        }
    }
}
