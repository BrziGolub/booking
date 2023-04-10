using Booking.Model;
using Booking.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.ServiceInterfaces
{
    public interface IAccommodationReservationRequestService : IService<AccommodationReservationRequests>
    {
        void Load();
        int NextId();
        void DeleteRequest(AccommodationReservation selectedReservation);
        void Subscribe(IObserver observer);
        void Unsubscribe(IObserver observer);
        void NotifyObservers();
        void BindReservationToRequest();
        void Create(AccommodationReservation selectedResrevation, DateTime newArrivalDay, DateTime newDepartureDay, String comment);

    }
}
