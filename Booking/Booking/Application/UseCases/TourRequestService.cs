using Booking.Domain.Model;
using Booking.Domain.RepositoryInterfaces;
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
        private readonly ILocationRepository _locationRepository;
        public TourRequestService()
        {
            _tourRequestRepository = InjectorRepository.CreateInstance<ITourRequestRepository>();
            _locationRepository = InjectorRepository.CreateInstance<ILocationRepository>();
        }

        public string GetMostPopularLanguageInLastYear()
        {
            return _tourRequestRepository.GetMostPopularLanguageInLastYear();
        }

        public int GetMostPopularLocationIdInLastYear()
        {
            return _tourRequestRepository.GetMostPopularLocationIdInLastYear();
        }

        public List<TourRequest> GetAll()
        {
            List<TourRequest> tourRequests = new List<TourRequest>();

            foreach(var TourRequest in  _tourRequestRepository.GetAll()) 
            {
                TourRequest.Location = _locationRepository.GetById(TourRequest.Location.Id);
                tourRequests.Add(TourRequest);
            }

            return tourRequests;
        }

    }
}
