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

namespace Booking.Service
{
    public class TourGradeService : ITourGradeService
    {
        private readonly List<IObserver> _observers;
        private readonly ITourGradeRepository _repository;
        
        public TourGradeService() 
        { 
        _repository = InjectorRepository.CreateInstance<ITourGradeRepository>();
        _observers = new List<IObserver>();
        }

        public List<TourGrade> GetAll()
        {
            return _repository.GetAll();
        }

        public TourGrade GetById(int id)
        {
            return _repository.GetById(id);
        }

        public List<TourGrade> GetGuideGrades()
        {
            List <TourGrade> lista = new List<TourGrade>();
            foreach (TourGrade tg in _repository.GetAll()) 
            {
            if(tg.Guide.Id == TourService.SignedGuideId)
                {
                    lista.Add(tg);
                }
            }
            return lista;
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
