using Booking.Model;
using Booking.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.ServiceInterfaces
{
    public interface ITourGuestsService : IService<TourGuests>
    {
        void Load();
        TourGuests AddTourGuests(TourGuests tourGuests);
        void Create(TourGuests tourGuests);
        void NotifyObservers();
        void Subscribe(IObserver observer);
        void Unsubscribe(IObserver observer);
    }
}
