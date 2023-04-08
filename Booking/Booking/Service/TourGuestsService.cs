using Booking.Model;
using Booking.Observer;
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
        private List<IObserver> _observers;

        public TourGuestsService() 
        {
            _repository = new TourGuestsRepository();
            _tourGuests = new List<TourGuests>();            
            _observers = new List<IObserver>();

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

        public TourGuests AddTourGuests(TourGuests tourGuests)
        {
            Load();
            _tourGuests.Add(tourGuests);  
            
            NotifyObservers();
            Save();
            
            return tourGuests;
        }

        public void Create(TourGuests tourGuests)
        {
            AddTourGuests(tourGuests);
            
        }

        public List<TourGuests> GetAll() 
        {
        return _tourGuests;
        }

        public bool Check() 
        {
            //if(_tourGuests.Any(u=> u.User.Id ))

            return true;
        }

        public void NotifyObservers()
        {
            foreach (var observer in _observers)
            {
                observer.Update();
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


    }
}
