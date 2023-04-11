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
        TourGuests AddTourGuests(TourGuests tourGuests);
        void Create(TourGuests tourGuests);
    }
}
