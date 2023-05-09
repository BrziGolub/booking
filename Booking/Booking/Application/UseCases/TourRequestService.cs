using Booking.Domain.Model;
using Booking.Domain.RepositoryInterfaces;
using Booking.Domain.ServiceInterfaces;
using Booking.Model;
using Booking.Repository;
using Booking.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

            foreach (var TourRequest in _tourRequestRepository.GetAll())
            {
                TourRequest.Location = _locationRepository.GetById(TourRequest.Location.Id);
                tourRequests.Add(TourRequest);
            }

            return tourRequests;
        }

        public void Search(ObservableCollection<TourRequest> observe, string city, string country, string numberOfGuests, string language)// fale datumi jos
        {
            observe.Clear();

            foreach (TourRequest tourRequest in _tourRequestRepository.GetAll())
            {
                tourRequest.Location = _locationRepository.GetById(tourRequest.Location.Id);

                if (IsCountryValid(country,tourRequest) && IsCityValid(city,tourRequest)  && IsNumberOfGuestsValid(numberOfGuests,tourRequest) && IsLanguageOfGuestsValid(language,tourRequest))
                {
                    observe.Add(tourRequest);
                }


            }


            //return observe;
        }

        public bool IsCountryValid(string country, TourRequest tourRequest)
        {
            return string.IsNullOrEmpty(country) || country.Equals("All") || tourRequest.Location.State.ToLower().Contains(country.ToLower());
        }

        public bool IsCityValid(string city, TourRequest tourRequest)
        {
            return string.IsNullOrEmpty(city) || tourRequest.Location.State.ToLower().Contains(city.ToLower());
        }

        public bool IsNumberOfGuestsValid(string number, TourRequest tourRequest)
        {
            return string.IsNullOrEmpty(number) || tourRequest.GuestsNumber.Equals(number);
        }

        public bool IsLanguageOfGuestsValid(string language, TourRequest tourRequest)
        {
            return string.IsNullOrEmpty(language) || tourRequest.Language.ToLower().Contains(language.ToLower());
        }
    }
}
