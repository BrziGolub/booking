using Booking.Model;
using Booking.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.ServiceInterfaces
{
    public interface IAccommodationAndOwnerGradeService : IService<AccommodationAndOwnerGrade>
    {
        void Load();
        int NextId();
        void Subscribe(IObserver observer);
        void Unsubscribe(IObserver observer);
        void NotifyObservers();
        List<AccommodationAndOwnerGrade> GetSeeableGrades();
        void BindGuestsImagesToGrades();
        void BindGradesToReservations();
        void SaveGrade(AccommodationAndOwnerGrade grade);
    }
}
