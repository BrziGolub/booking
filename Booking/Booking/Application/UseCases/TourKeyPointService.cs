using Booking.Domain.RepositoryInterfaces;
using Booking.Domain.ServiceInterfaces;
using Booking.Model;
using Booking.Observer;
using Booking.Repository;
using Booking.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Booking.Service
{
	public class TourKeyPointService : ITourKeyPointsService // nepotrebno
	{
		private readonly ITourKeyPointRepository _tourKeyPointRepository;
        private readonly List<IObserver> _observers;

        public TourKeyPointService()
		{
			_tourKeyPointRepository = InjectorRepository.CreateInstance<ITourKeyPointRepository>();
			_observers = new List<IObserver>();
		}

		public List<TourKeyPoint> GetKeyPointsByTourId(int id)
		{
			List<TourKeyPoint> keyPoints = new List<TourKeyPoint>();

			foreach (TourKeyPoint keypoint in _tourKeyPointRepository.GetAll())
			{
				if (keypoint.Tour.Id == id)
					keyPoints.Add(keypoint);
			}

			return keyPoints;
		}

		public TourKeyPoint UpdateKeyPoint(TourKeyPoint tourKeyPoint)
		{
			/*TourKeyPoint oldTourKeyPoint = GetById(tourKeyPoint.Id);

			oldTourKeyPoint.Tour = tourKeyPoint.Tour;
			oldTourKeyPoint.Location = tourKeyPoint.Location;
			oldTourKeyPoint.Achieved = tourKeyPoint.Achieved;*/

			//Save(); // repository save
			return _tourKeyPointRepository.Update(tourKeyPoint);
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
