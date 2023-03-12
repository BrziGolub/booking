using Booking.Model.DAO;
using Booking.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Booking.Observer;

namespace Booking.Controller
{
	public class TourController
	{
		private readonly TourDAO tourDAO;

        public TourController(TourDAO tour)
        {
            tourDAO = tour;
        }

        public List<Tour> GetAll()
		{
			return tourDAO.GetAll();
		}

		public void Subscribe(IObserver observer)
		{
			tourDAO.Subscribe(observer);
		}

		public void Create(Tour tour)
		{
			tourDAO.addTour(tour);
		}
	}
}
