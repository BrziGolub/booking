using Booking.Domain.DTO;
using Booking.Domain.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.ServiceInterfaces
{
    public interface ITourRequestService : IService<TourRequest>
    {
        string GetMostPopularLanguageInLastYear();
        int GetMostPopularLocationIdInLastYear();
        List<TourRequest> GetAll();
        void Search(ObservableCollection<TourRequest> observe, string city, string country, string numberOfGuests, string language, DateTime? startDate, DateTime? endDate);
        void ShowAll(ObservableCollection<TourRequest> observe);
        List<YearlyRequests> GetRequestsByYearAndLocation(int locationId);
        List<YearlyRequests> GetRequestsByYearAndLanguage(string language);
        List<MonthlyRequests> GetRequestsByMonthAndLocation(int locationId, int year);
        List<MonthlyRequests> GetRequestsByMonthAndLanguage(string language, int year);
        TourRequest AddTourRequest(TourRequest tourRequest);
        List<TourRequest> GetRequestsByUserId(int id);
	}
}
