using Booking.Model;
using Booking.Model.DAO;
using Booking.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Controller
{
    public class AccommodationGradeController
    {
        private readonly AccommodationGradeDAO accommodationGradeDAO;

        public AccommodationGradeController(AccommodationGradeDAO accommodationGrade)
        {
            accommodationGradeDAO = accommodationGrade;
        }

        public AccommodationGrade FindById(int id)
        {
            return accommodationGradeDAO.FindById(id);
        }
        public void Subscribe(IObserver observer)
        {
            accommodationGradeDAO.Subscribe(observer);
        }
        public void Create(AccommodationGrade accommodationGrade)
        {
            accommodationGradeDAO.Create(accommodationGrade);
        }
        public int NextId()
        {
            return accommodationGradeDAO.NextId();
        }
        public List<AccommodationGrade> FindAll()
        {
            return accommodationGradeDAO.FindAllGrades();
        }
        public bool IsReservationGraded(int accommodationReservationId)
        {
            return accommodationGradeDAO.IsReservationGraded(accommodationReservationId);
        }
    }
}
