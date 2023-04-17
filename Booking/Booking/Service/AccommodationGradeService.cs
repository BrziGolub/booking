using Booking.Observer;
using Booking.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Booking.Model;
using Booking.Domain.ServiceInterfaces;
using Booking.Util;
using Booking.Domain.RepositoryInterfaces;

namespace Booking.Service
{
    public class AccommodationGradeService : IAccommodationGradeService
    {
        private readonly List<IObserver> _observers;
        private readonly IAccommodationGradeRepository _repository;
        private IAccommodationResevationRepository _accommodationReservationRepository;


		public AccommodationGradeService()
        {
            _repository = InjectorRepository.CreateInstance<IAccommodationGradeRepository>();
            _observers = new List<IObserver>();
            _accommodationReservationRepository = InjectorRepository.CreateInstance<IAccommodationResevationRepository>();
        }
        public List<AccommodationGrade> GetAll()
        {
            return _repository.GetAll();
        }
        public AccommodationGrade Create(AccommodationGrade accommodationGrade)
        {
            _repository.Add(accommodationGrade);
            NotifyObservers();
            return accommodationGrade;
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
        /*
        public void BindGradesToReservations()
        {
            foreach (AccommodationGrade accommodationGrade in _accommodationGrades)
            {
                foreach (AccommodationReservation accommodationReservation in _accommodationReservationRepository.Load())
                {
                    if (accommodationReservation.Id == accommodationGrade.Accommodation.Id)
                    {
                        accommodationGrade.Accommodation = accommodationReservation;
                    }
                }
            }

        }*/
        /*
        public bool IsReservationGraded(int accommodationReservationId)
        {
            foreach (var grade in _repository.GetAll())
            {
                if (grade.Accommodation.Id == accommodationReservationId)
                {
                    return true;
                }
            }
            return false;
        }*/
        public bool IsReservationGraded(int accommodationReservationId)
        {
            return _repository.IsReservationGraded(accommodationReservationId);
        }
    }
}
