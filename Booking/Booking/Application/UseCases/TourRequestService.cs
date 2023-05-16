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
using System.Security.AccessControl;

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

		public int GetNumberOfRequestsByUserId(int id, string year)
		{
			return ((year.Equals("All")) ? GetRequestsByUserId(id).Count() : GetRequestsByUserId(id).Where(tr => tr.CreatedDate.Year.ToString().Equals(year)).Count()) - GetNumberOfAcceptedRequestsByUserId(id, year);
		}

		public int GetNumberOfAcceptedRequestsByUserId(int id, string year)
		{
			return (year.Equals("All")) ? GetRequestsByUserId(id).Where(tr => tr.Status.Equals("Accepted")).Count() : GetRequestsByUserId(id).Where(tr => tr.Status.Equals("Accepted")).Where(tr => tr.CreatedDate.Year.ToString().Equals(year)).Count();
		}

		public List<string> GetLanguagesByUserId(int id, string year)
		{
			List<string> result = new List<string>();
			if (year.Equals("All"))
			{
				foreach (var tr in GetRequestsByUserId(id))
				{
					if (!result.Contains(tr.Language))
					{
						result.Add(tr.Language);
					}
				}
			}
			else
			{
				foreach (var tr in GetRequestsByUserId(id))
				{
					if (!result.Contains(tr.Language) && tr.CreatedDate.Year.ToString().Equals(year))
					{
						result.Add(tr.Language);
					}
				}
			}
			return result;
		}

		public int GetNumberOfRequestsByLang(int id, string lang, string year)
		{
			return (year.Equals("All")) ? GetRequestsByUserId(id).Where(tr => tr.Language.Equals(lang)).Count() : GetRequestsByUserId(id).Where(tr => tr.CreatedDate.Year.ToString().Equals(year)).Where(tr => tr.Language.Equals(lang)).Count();
		}

		public List<string> GetYearsByUserId(int id)
		{
			List<string> result = new List<string>() { "All" };
			foreach (var tr in GetAll())
			{
				if (!result.Contains(tr.CreatedDate.Year.ToString()))
				{
					result.Add(tr.CreatedDate.Year.ToString());
				}
			}
			return result;
		}

		public List<string> GetStatesByUserId(int id, string year)
		{
			List<string> result = new List<string>();
			if (year.Equals("All"))
			{
				foreach (var tr in GetRequestsByUserId(id))
				{
					if (!result.Contains(tr.Location.State))
					{
						result.Add(tr.Location.State);
					}
				}
			}
			else
			{
				foreach (var tr in GetRequestsByUserId(id))
				{
					if (!result.Contains(tr.Location.State) && tr.CreatedDate.Year.ToString().Equals(year))
					{
						result.Add(tr.Location.State);
					}
				}
			}
			return result;
		}

		public int GetNumberOfRequestsByState(int id, string state, string year)
		{
			return (year.Equals("All")) ? GetRequestsByUserId(id).Where(tr => tr.Location.State.Equals(state)).Count() : GetRequestsByUserId(id).Where(tr => tr.CreatedDate.Year.ToString().Equals(year)).Where(tr => tr.Location.State.Equals(state)).Count();
		}

		public List<string> GetCitiesByUserId(int id, string year)
		{
			List<string> result = new List<string>();
			if (year.Equals("All"))
			{
				foreach (var tr in GetRequestsByUserId(id))
				{
					if (!result.Contains(tr.Location.City))
					{
						result.Add(tr.Location.City);
					}
				}
			}
			else
			{
				foreach (var tr in GetRequestsByUserId(id))
				{
					if (!result.Contains(tr.Location.City) && tr.CreatedDate.Year.ToString().Equals(year))
					{
						result.Add(tr.Location.City);
					}
				}
			}
			return result;
		}

		public int GetNumberOfRequestsByCity(int id, string city, string year)
		{
			return (year.Equals("All")) ? GetRequestsByUserId(id).Where(tr => tr.Location.City.Equals(city)).Count() : GetRequestsByUserId(id).Where(tr => tr.CreatedDate.Year.ToString().Equals(year)).Where(tr => tr.Location.City.Equals(city)).Count();
		}
	}
}
