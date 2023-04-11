using Booking.Domain.RepositoryInterfaces;
using Booking.Domain.ServiceInterfaces;
using Booking.Model;
using Booking.Observer;
using Booking.Repository;
using Booking.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Service
{
    public class TourGuestsService : ITourGuestsService
    {
        private readonly ITourGuestsRepository _repository;
        private List<IObserver> _observers;

		public TourGuestsService() 
        {
            _repository = InjectorRepository.CreateInstance<ITourGuestsRepository>();
            _observers = new List<IObserver>();
        }

        public TourGuests AddTourGuests(TourGuests tourGuests)
        {           
            return _repository.Add(tourGuests);
        }

        public void Create(TourGuests tourGuests)
        {
            AddTourGuests(tourGuests);
			NotifyObservers();
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
