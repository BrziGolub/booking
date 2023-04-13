using Booking.Model;
using Booking.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.ServiceInterfaces
{
    public interface ITourKeyPointsService : ISubject, IService<TourKeyPoint>
    {
        List<TourKeyPoint> GetKeyPointsByTourId(int id);
        TourKeyPoint UpdateKeyPoint(TourKeyPoint tourKeyPoint);
    }
}
