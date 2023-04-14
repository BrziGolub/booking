using Booking.Domain.RepositoryInterfaces;
using Booking.Domain.ServiceInterfaces;
using Booking.Model;
using Booking.Model.Images;
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
    public class AccommodationAndOwnerGradeService : IAccommodationAndOwnerGradeService
    {
        private readonly IAccommodationAndOwnerGradeRepository _repository;
        private readonly IAccommodationResevationRepository _reservationRepository;
        private readonly IAccommodationRepository _accommodationRepository;
        private readonly IAccommodationGradeRepository _gradeRepository;
        private readonly IUserRepository _userRepository;
        private readonly List<IObserver> _observers;
        //private List<AccommodationAndOwnerGrade> _accommodationAndOwnerGrades;


        //private readonly IAccommodationGradeRepository _gradeRepository;

        //private AccommodationReservationService _accommodationReservationService;
        //private GuestsAccommodationImagesService _guestsImagesService;


		public AccommodationAndOwnerGradeService()
        {
            _observers = new List<IObserver>();
            _repository = InjectorRepository.CreateInstance<IAccommodationAndOwnerGradeRepository>();
            _reservationRepository = InjectorRepository.CreateInstance<IAccommodationResevationRepository>();
            _accommodationRepository = InjectorRepository.CreateInstance<IAccommodationRepository>();
            _userRepository = InjectorRepository.CreateInstance<IUserRepository>();
            _gradeRepository = InjectorRepository.CreateInstance<IAccommodationGradeRepository>();
            //_accommodationReservationService = new AccommodationReservationService();
            //_guestsImagesService = new GuestsAccommodationImagesService(); //ovo iz app

 
        }

        /*public void Load()
        {
            _accommodationAndOwnerGrades = _repository.Load();
            _accommodationGrades = _gradeRepository.Load();
            BindGuestsImagesToGrades();
        }*/

        /*public List<AccommodationAndOwnerGrade> GetAllGrades()
        {
            return _accommodationAndOwnerGrades;
        }*/
        public int NextId()
        {
            return _repository.NextId();
        }

        public void SaveGrade(AccommodationAndOwnerGrade grade) 
        {
            _repository.Add(grade);
            //FALI DODAVANJE SLIKA U CSV
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
        public List<AccommodationAndOwnerGrade> GetSeeableGrades() 
        {
            List<AccommodationAndOwnerGrade> _seeableGrades = new List<AccommodationAndOwnerGrade>();
            foreach (var g in _repository.GetAll()) 
            {
                g.AccommodationReservation = _reservationRepository.GetById(g.AccommodationReservation.Id);
                g.AccommodationReservation.Accommodation = _accommodationRepository.GetById(g.AccommodationReservation.Accommodation.Id);
                g.AccommodationReservation.Guest = _userRepository.GetById(g.AccommodationReservation.Guest.Id);
                //dodati uvezivanje slika al prvo kristina mora da napravi model za te slike.
                if (g.AccommodationReservation.Accommodation.Owner.Id == AccommodationService.SignedOwnerId) 
                {
                    foreach (var go in _gradeRepository.GetAll()) 
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
        public void CheckSuper(AccommodationReservation selectedReservation) 
        {
            List<User> AllUsers = new List<User>();
            AllUsers = _userRepository.GetAll();
            int Counter = 0;
            double GradeSum = 0; 
            selectedReservation.Accommodation = _accommodationRepository.GetById(selectedReservation.Accommodation.Id);
            selectedReservation.Accommodation.Owner = _userRepository.GetById(selectedReservation.Accommodation.Owner.Id);
            foreach (var user in AllUsers)
            {
                if (user.Id == selectedReservation.Accommodation.Owner.Id) 
                {
                    foreach (var ocena in _repository.GetAll()) 
                    {
                        ocena.AccommodationReservation = _reservationRepository.GetById(ocena.AccommodationReservation.Id);
                        ocena.AccommodationReservation.Accommodation = _accommodationRepository.GetById(ocena.AccommodationReservation.Accommodation.Id);

                        if (ocena.AccommodationReservation.Accommodation.Owner.Id == user.Id) 
                        {
                            Counter++;
                            GradeSum = GradeSum + ((Convert.ToDouble(ocena.OwnersCourtesy) + Convert.ToDouble(ocena.Cleaness)) / 2);
                        }
                    }
                    if (Counter >= 50 && (GradeSum / Counter) > 4.5)
                    {
                        user.Super = 1;
                    }
                    else 
                    {
                        user.Super = 0;
                    }
                }

            }
            _userRepository.Save(AllUsers);
        }
        public int GetNumberOfGrades() 
        {
            int Counter = 0;
            foreach (var ocena in _repository.GetAll())
            {
                ocena.AccommodationReservation = _reservationRepository.GetById(ocena.AccommodationReservation.Id);
                ocena.AccommodationReservation.Accommodation = _accommodationRepository.GetById(ocena.AccommodationReservation.Accommodation.Id);

                if (ocena.AccommodationReservation.Accommodation.Owner.Id == AccommodationService.SignedOwnerId)
                {
                    Counter++;
                }
            }
            return Counter;
        }
        public double GetAverageGrade() 
        {
            int Counter = 0;
            double GradeSum = 0;
            foreach (var ocena in _repository.GetAll())
            {
                ocena.AccommodationReservation = _reservationRepository.GetById(ocena.AccommodationReservation.Id);
                ocena.AccommodationReservation.Accommodation = _accommodationRepository.GetById(ocena.AccommodationReservation.Accommodation.Id);

                if (ocena.AccommodationReservation.Accommodation.Owner.Id == AccommodationService.SignedOwnerId)
                {
                    Counter++;
                    GradeSum = GradeSum + ((Convert.ToDouble(ocena.OwnersCourtesy) + Convert.ToDouble(ocena.Cleaness)) / 2);
                }
            }
            return GradeSum/Counter;
        }

        //proveri ovo
        /*
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
        }*/


        public List<AccommodationAndOwnerGrade> GetAll()
        {
            return _repository.GetAll();
        }
    }
}
