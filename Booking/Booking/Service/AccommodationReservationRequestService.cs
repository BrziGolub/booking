using Booking.Model;
using Booking.Model.Enums;
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
    public class AccommodationReservationRequestService
    {
        private readonly AccommodationReservationRequestsRepostiory _repository;
        private List<AccommodationReservationRequests> _requests;

        public AccommodationReservationRequestService()
        {
            _repository = new AccommodationReservationRequestsRepostiory();
            _requests = new List<AccommodationReservationRequests>();
          
            Load();
        }

        public void Load()
        {
            _requests = _repository.Load();
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

        public void Create(AccommodationReservation selectedResrevation, DateTime NewArrivalDay,DateTime NewDepartureDay)
        {
            AccommodationReservationRequests newRequest = new AccommodationReservationRequests();

            newRequest.Id = NextId();
            newRequest.AccommodationReservation = selectedResrevation;
            newRequest.NewArrivalDay = NewArrivalDay;
            newRequest.NewDeparuteDay = NewDepartureDay;
            newRequest.Status = RequestStatus.PENDNING;
            newRequest.Comment = "Comment";
            _requests.Add(newRequest);
            Save();
        }
    }
}
