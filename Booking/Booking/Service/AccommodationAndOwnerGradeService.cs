using Booking.Model;
using Booking.Model.Images;
using Booking.Observer;
using Booking.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Service
{
    public class AccommodationAndOwnerGradeService 
    {
        private readonly AccommodationAndOwnerGradeRepository _repository;

        private List<AccommodationAndOwnerGrade> _accommodationAndOwnerGrades;
<<<<<<< Updated upstream

        private readonly AccommodationGradeRepository _gradeRepository;
        private List<AccommodationGrade> _accommodationGrades;
        private AccommodationReservationService _accommodationReservationService;
        private readonly List<IObserver> _observers;

=======
        //private AccommodationResevationRepository _accommodationReservationRepository;
        private AccommodationReservationService _accommodationReservationService;

        private GuestsAccommodationImagesService _guestsImagesService;
>>>>>>> Stashed changes

        public AccommodationAndOwnerGradeService()
        {
            _observers = new List<IObserver>();
            _repository = new AccommodationAndOwnerGradeRepository();
<<<<<<< Updated upstream
            _accommodationAndOwnerGrades = new List<AccommodationAndOwnerGrade>();
            _accommodationReservationService = new AccommodationReservationService();
            _gradeRepository = new AccommodationGradeRepository();
            _accommodationGrades = new List<AccommodationGrade>();
=======
            _guestsImagesService = new GuestsAccommodationImagesService(); //ovo iz app
            _accommodationReservationService = new AccommodationReservationService();
            _accommodationAndOwnerGrades = new List<AccommodationAndOwnerGrade>(); 
           // _accommodationReservationRepository = new AccommodationResevationRepository();
            
>>>>>>> Stashed changes
            Load(); 
        }

        public void Load()
        {
            _accommodationAndOwnerGrades = _repository.Load();
            _accommodationGrades = _gradeRepository.Load();
            BindGradesToReservations();
            BindGuestsImagesToGrades();
        }

        public int NextId()
        {
            if (_accommodationAndOwnerGrades.Count == 0)
            {
                return 1;
            }
            else
            {
                return _accommodationAndOwnerGrades.Max(t => t.Id) + 1;
            }
        }
        public void SaveGrade(AccommodationAndOwnerGrade grade)
        {
            grade.Id = NextId();
            _accommodationAndOwnerGrades.Add(grade);
            _repository.Save(_accommodationAndOwnerGrades);
        }
        public List<AccommodationAndOwnerGrade> GetAllGrades()
        {
            return _accommodationAndOwnerGrades;
        }
        public AccommodationAndOwnerGrade GetById(int id)
        {
            return _accommodationAndOwnerGrades.Find(v => v.Id == id);
        }
<<<<<<< Updated upstream
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
        public List<AccommodationAndOwnerGrade> GetSeeableGrades() 
        {
            List<AccommodationAndOwnerGrade> _seeableGrades = new List<AccommodationAndOwnerGrade>();
            foreach (var g in _accommodationAndOwnerGrades) 
            {
                if (g.AccommodationReservation.Accommodation.Owner.Id == AccommodationService.SignedOwnerId) 
                {
                    foreach (var go in _accommodationGrades) 
                    {
                        if (go.Accommodation.Id == g.AccommodationReservation.Id) 
                        {
                            _seeableGrades.Add(g);
                        }
                    }
                }
            }
            return _seeableGrades;
        }
=======

        //proveri ovo
        public void BindGuestsImagesToGrades()
        {
            foreach (AccommodationAndOwnerGrade grade in _accommodationAndOwnerGrades)
            {
                foreach (var image in _guestsImagesService.GetAll())
                {
                    if (image.Accomodation.Id == grade.Id)
                    {
                        grade.Images.Add(image);
                    }
                }

            }
        }
        //proveri ovo 
>>>>>>> Stashed changes
        public void BindGradesToReservations()
        { 

            foreach (AccommodationAndOwnerGrade accommodationGrade in _accommodationAndOwnerGrades)
            {
                foreach (AccommodationReservation accommodationReservation in _accommodationReservationService.GetAll())
                {
                    if (accommodationReservation.Id == accommodationGrade.AccommodationReservation.Id)
                    {
                        accommodationGrade.AccommodationReservation = accommodationReservation;
                    }
                }
            }
        }
    }
}
