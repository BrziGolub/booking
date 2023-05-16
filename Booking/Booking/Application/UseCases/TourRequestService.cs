using Booking.Domain.DTO;
using Booking.Domain.Model;
using Booking.Domain.RepositoryInterfaces;
using Booking.Domain.ServiceInterfaces;
using Booking.Repository;
using Booking.Service;
using Booking.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;

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

		public void Search(ObservableCollection<TourRequest> observe, string city, string country, string numberOfGuests, string language, DateTime? startDate, DateTime? endDate)
		{
			observe.Clear();

			foreach (TourRequest tourRequest in _tourRequestRepository.GetAll())
			{
				tourRequest.Location = _locationRepository.GetById(tourRequest.Location.Id);

				if (IsCountryValid(country, tourRequest) && IsCityValid(city, tourRequest) && IsNumberOfGuestsValid(numberOfGuests, tourRequest) && IsLanguageOfGuestsValid(language, tourRequest) && IsTourRequestInDateRange(startDate, endDate, tourRequest))
				{
					observe.Add(tourRequest);
				}


			}

		}

		public bool IsCountryValid(string country, TourRequest tourRequest)
		{
			return string.IsNullOrEmpty(country) || country.Equals("All") || tourRequest.Location.State.ToLower().Contains(country.ToLower());
		}

		public bool IsCityValid(string city, TourRequest tourRequest)
		{
			return string.IsNullOrEmpty(city) || tourRequest.Location.City.ToLower().Contains(city.ToLower());
		}

		public bool IsNumberOfGuestsValid(string number, TourRequest tourRequest)
		{
			if (string.IsNullOrEmpty(number))
			{
				return true;
			}

			int guestsNumber;

			if (int.TryParse(number, out guestsNumber))
			{
				return tourRequest.GuestsNumber == guestsNumber;
			}

			return false;
		}

		public bool IsLanguageOfGuestsValid(string language, TourRequest tourRequest)
		{
			return string.IsNullOrEmpty(language) || tourRequest.Language.ToLower().Contains(language.ToLower());
		}

		private bool IsTourRequestInDateRange(DateTime? startDate, DateTime? endDate, TourRequest tourRequest)
		{
			if (startDate == null && endDate == null)
			{
				return true;
			}

			if (startDate == null && tourRequest.StartTime <= endDate)
			{
				return true;
			}

			if (endDate == null && tourRequest.EndTime >= startDate)
			{
				return true;
			}

			if (tourRequest.StartTime <= endDate && tourRequest.EndTime >= startDate)
			{
				return true;
			}

			return false;
		}

		public void ShowAll(ObservableCollection<TourRequest> observe)
		{
			observe.Clear();
			foreach (TourRequest tourRequest in _tourRequestRepository.GetAll())
			{
				tourRequest.Location = _locationRepository.GetById(tourRequest.Location.Id);
				observe.Add(tourRequest);
			}

		}

		public List<YearlyRequests> GetRequestsByYearAndLocation(int locationId)
		{
			List<YearlyRequests> requestsByYear = new List<YearlyRequests>();

			List<TourRequest> tourRequests = _tourRequestRepository.GetByLocationId(locationId);

			var requestsGroupedByYear = tourRequests.GroupBy(tr => tr.StartTime.Year)
													 .Select(g => new { Year = g.Key, Count = g.Count() });

			foreach (var group in requestsGroupedByYear)
			{
				YearlyRequests yearlyRequests = new YearlyRequests
				{
					Year = group.Year,
					Count = group.Count
				};

				requestsByYear.Add(yearlyRequests);
			}

			return requestsByYear;
		}

		public List<YearlyRequests> GetRequestsByYearAndLanguage(string language)
		{
			List<YearlyRequests> requestsByYear = new List<YearlyRequests>();

			List<TourRequest> tourRequests = _tourRequestRepository.GetByLanguage(language);

			var requestsGroupedByYear = tourRequests.GroupBy(tr => tr.StartTime.Year)
													 .Select(g => new { Year = g.Key, Count = g.Count() });

			foreach (var group in requestsGroupedByYear)
			{
				YearlyRequests yearlyRequests = new YearlyRequests
				{
					Year = group.Year,
					Count = group.Count
				};

				requestsByYear.Add(yearlyRequests);
			}

			return requestsByYear;
		}

		public List<MonthlyRequests> GetRequestsByMonthAndLocation(int locationId, int year)
		{
			List<MonthlyRequests> requestsByMonth = new List<MonthlyRequests>();

			List<TourRequest> tourRequests = _tourRequestRepository.GetByLocationId(locationId);

			var requestsGroupedByMonth = tourRequests.Where(tr => tr.StartTime.Year == year)
													 .GroupBy(tr => tr.StartTime.Month)
													 .Select(g => new { Month = g.Key, Count = g.Count() });

			foreach (var group in requestsGroupedByMonth)
			{
				MonthlyRequests monthlyRequests = new MonthlyRequests
				{
					MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(group.Month),
					Count = group.Count
				};

				requestsByMonth.Add(monthlyRequests);
			}


			return requestsByMonth;
		}

		public List<MonthlyRequests> GetRequestsByMonthAndLanguage(string language, int year)
		{
			List<MonthlyRequests> requestsByMonth = new List<MonthlyRequests>();

			List<TourRequest> tourRequests = _tourRequestRepository.GetByLanguage(language);

			var requestsGroupedByMonth = tourRequests.Where(tr => tr.StartTime.Year == year)
													 .GroupBy(tr => tr.StartTime.Month)
													 .Select(g => new { Month = g.Key, Count = g.Count() });

			foreach (var group in requestsGroupedByMonth)
			{
				MonthlyRequests monthlyRequests = new MonthlyRequests
				{
					MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(group.Month),
					Count = group.Count
				};

				requestsByMonth.Add(monthlyRequests);
			}


			return requestsByMonth;
		}

		public TourRequest AddTourRequest(TourRequest tourRequest)
		{
			tourRequest.User.Id = TourService.SignedGuideId;
			tourRequest.Status = "On hold";

			_tourRequestRepository.Add(tourRequest);

			return tourRequest;
		}

		public List<TourRequest> GetRequestsByUserId(int id)
		{
			List<TourRequest> list = GetAll();
			List<TourRequest> result = new List<TourRequest>();

			var res = list.Where(tr => tr.User.Id == id);
			foreach (var tr in res)
			{
				result.Add(tr);
			}

			return result;
		}
	}
}
