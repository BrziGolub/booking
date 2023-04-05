using Booking.Model;
using Booking.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Service
{
    public class TourGuestsService
    {
        private readonly TourGuestsRepository _repository;
        private List<TourGuests> _tourGuests;

        public TourGuestsService() 
        {
        _repository = new TourGuestsRepository();
            _tourGuests = new List<TourGuests>();

            Load();
        
        }
        public void Load() 
        {
            _tourGuests = _repository.Load();
        }

        public void Save() 
        {
            _repository.Save(_tourGuests);
        }

        /*public TourGuests UpdateTourGuests(TourGuests tourGuests)
        {
            TourKeyPoint oldTourKeyPoint = _tourKeyPointService.GetById(tourKeyPoint.Id);

            TourGuests tourGuests = new TourGuests();

            tourGuests.Tour.Id = SelectedTour.Id;
            tourGuests.User.Id = SelectedGuest.Id;
            tourGuests.TourKeyPoint.Id = SelectedTourKeyPoint.Id;

            _tourKeyPointService.Save();
            NotifyObservers();

            return oldTourKeyPoint;
        }*/
        public List<TourGuests> GetAll() 
        {
        return _tourGuests;
        }


    }
}
