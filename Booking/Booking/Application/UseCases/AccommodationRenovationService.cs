using Booking.Domain.Model;
using Booking.Domain.RepositoryInterfaces;
using Booking.Domain.ServiceInterfaces;
using Booking.Observer;
using Booking.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Application.UseCases
{
    public class AccommodationRenovationService : ISubject, IAccommodationRenovationService
    {
        private readonly List<IObserver> _observers;
        private readonly IAccommodationRenovationRepository _repository;
        public AccommodationRenovationService()
        {
            _observers = new List<IObserver>();
            _repository = InjectorRepository.CreateInstance<IAccommodationRenovationRepository>();
        }
        public AccommodationRenovation GetById(int id)
        {
            return _repository.GetById(id);
        }
        public void SaveRenovation(AccommodationRenovation renovation)
        {
            _repository.Add(renovation);
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
    }
}
