using Booking.Domain.RepositoryInterfaces;
using Booking.Domain.ServiceInterfaces;
using Booking.Model.Images;
using Booking.Observer;
using Booking.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Application.UseCases
{
    public class TourImageService : ITourImageService
    {
        private readonly ITourImageRepository _tourImageRepository;
        private readonly List<IObserver> _observers;
        public TourImageService()
        {
            _tourImageRepository = InjectorRepository.CreateInstance<ITourImageRepository>();
            _observers = new List<IObserver>();
        }

        public TourImage RemoveTourImage(TourImage image) 
        {
            TourImage tourImage = _tourImageRepository.GetByUrl(image.Url);//GetById(image.Id); 
            if(tourImage == null) return null;

            _tourImageRepository.Delete(tourImage);
            NotifyObservers();
            return tourImage;
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
