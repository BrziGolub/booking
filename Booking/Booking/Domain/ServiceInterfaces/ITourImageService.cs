using Booking.Model.Images;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.ServiceInterfaces
{
    public interface ITourImageService : IService<TourImage>
    {
        void Load();
        List<TourImage> GetImagesByTourId(int id);
    }
}
