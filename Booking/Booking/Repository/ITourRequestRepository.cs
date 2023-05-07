using Booking.Domain.Model;
using Booking.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Repository
{
    public interface ITourRequestRepository : IRepository<TourRequest>
    {
        string GetMostPopularLanguageInLastYear();
        int GetMostPopularLocationIdInLastYear();
    }
}
