using Booking.Model.DAO;
using Booking.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Booking.Observer;
using System.Collections.ObjectModel;

namespace Booking.Controller
{
	public class TourController
	{
		private readonly TourDAO tourDAO;

		public TourController()
		{
			tourDAO = new TourDAO();
		}

		public List<Tour> GetAll()
		{
			return tourDAO.GetAll();
		}

		public void Search(ObservableCollection<Tour> observe, string state, string city, string duration, string language, string passengers) 
		{
			tourDAO.Search(observe, state, city, duration, language, passengers);
		}

		public void CancelSearch(ObservableCollection<Tour> observe)
		{
			tourDAO.CancelSearch(observe);
		}

		public List<string> GetAllStates() 
		{ 
			return tourDAO.GetAllStates();
		}

		public ObservableCollection<string> GetAllCitiesByState(ObservableCollection<string> observe, string state)
		{
			return tourDAO.GetAllCitiesByState(observe, state);
		}

		public void Subscribe(IObserver observer)
		{
			tourDAO.Subscribe(observer);
		}
	}
}
