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
    public class AccommodationAndOwnerGradeService 
    {
        private readonly AccommodationAndOwnerGradeRepository _repository;

        private List<AccommodationAndOwnerGrade> _accommodationAndOwnerGrades;
        private AccommodationResevationRepository _accommodationReservationRepository;
       

        public AccommodationAndOwnerGradeService()
        {
            _repository = new AccommodationAndOwnerGradeRepository();
            _accommodationAndOwnerGrades = new List<AccommodationAndOwnerGrade>(); 
            _accommodationReservationRepository = new AccommodationResevationRepository();
            Load(); 
        }

        public void Load()
        {
            _accommodationAndOwnerGrades = _repository.Load();
            BindGradesToReservations();
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
        public void BindGradesToReservations()
        { 
            //proveri ovo 
            foreach (AccommodationAndOwnerGrade accommodationGrade in _accommodationAndOwnerGrades)
            {
                foreach (AccommodationReservation accommodationReservation in _accommodationReservationRepository.Load())
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
