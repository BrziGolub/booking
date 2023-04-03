using Booking.Observer;
using Booking.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Booking.Model;

namespace Booking.Service
{
    public class AccommodationGradeService : ISubject
    {
        private readonly AccommodationGradeRepository _repository;
        private List<AccommodationGrade> _accommodationGrades;
        private readonly List<IObserver> _observers;
        private readonly AccommodationGradeRepository _accommodationGradeRepository;
        private AccommodationResevationRepository _accommodationReservationRepository;

        public AccommodationGradeService()
        {
            _repository = new AccommodationGradeRepository();
            _accommodationGrades = _repository.Load();
            _observers = new List<IObserver>();
            _accommodationGradeRepository = new AccommodationGradeRepository();
            _accommodationReservationRepository = new AccommodationResevationRepository();
            BindGradesToReservations();
        }
        public List<AccommodationGrade> GetAllGrades()
        {
            return _accommodationGrades;
        }
        public AccommodationGrade GetById(int id)
        {
            return _accommodationGrades.Find(v => v.Id == id);
        }
        public int NextId()
        {
            if (_accommodationGrades.Count == 0)
            {
                return 1;
            }
            else
            {
                return _accommodationGrades.Max(t => t.Id) + 1;
            }
        }

        public AccommodationGrade Create(AccommodationGrade accommodationGrade)
        {
            accommodationGrade.Id = NextId();
            _accommodationGrades.Add(accommodationGrade);
            _accommodationGradeRepository.Save(_accommodationGrades);
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

        }
        public bool IsReservationGraded(int accommodationReservationId)
        {
            foreach (var grade in _accommodationGrades)
            {
                if (grade.Accommodation.Id == accommodationReservationId)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
