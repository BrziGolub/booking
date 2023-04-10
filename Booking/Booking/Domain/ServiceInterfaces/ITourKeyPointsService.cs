using Booking.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.ServiceInterfaces
{
    public interface ITourKeyPointsService : IService<ITourKeyPointsService>
    {
        void Load();
        int NextId();
        void LoadLocations();
        List<TourKeyPoint> GetKeyPointsByTourId(int id);
        TourKeyPoint UpdateKeyPoint(TourKeyPoint tourKeyPoint);
    }
}
