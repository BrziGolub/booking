using Booking.Domain.ServiceInterfaces;
using Booking.Repository;
using Booking.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Application.UseCases
{
    public class TourRequestService : ITourRequestService
    {
        private readonly ITourRequestRepository _tourRequestRepository;
        public TourRequestService()
        {
            _tourRequestRepository = InjectorRepository.CreateInstance<ITourRequestRepository>();
        }

        public string GetMostPopularLanguageInLastYear()
        {
            return _tourRequestRepository.GetMostPopularLanguageInLastYear();
        }

        public int GetMostPopularLocationIdInLastYear()
        {
            return _tourRequestRepository.GetMostPopularLocationIdInLastYear();
        }

    }
}
