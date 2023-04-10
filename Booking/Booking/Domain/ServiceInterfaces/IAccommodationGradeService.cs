using Booking.Model;
using Booking.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.ServiceInterfaces
{
    public interface IAccommodationGradeService : IService<AccommodationGrade>
    {
        int NextId();
        AccommodationGrade Create(AccommodationGrade accommodationGrade);
        void Subscribe(IObserver observer);
        void Unsubscribe(IObserver observer);
        void NotifyObservers();
        void BindGradesToReservations();
        bool IsReservationGraded(int accommodationReservationId);
    }
}
