using Booking.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.ServiceInterfaces
{
    public interface ITourReservationService : IService<TourReservation>
    {
        void Load();
        int GenerateId();
        void CreateTourReservation(Tour tour, int visitors);
        int CheckAvailability(int id);
    }
}
