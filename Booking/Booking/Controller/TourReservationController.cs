using Booking.Model;
using Booking.Model.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Booking.Controller
{
	public class TourReservationController
	{
		private readonly TourReservationDAO _tourReservationDAO;

        public TourReservationController()
        {
            _tourReservationDAO = new TourReservationDAO();
        }

        public List<TourReservation> GetAll()
        {
            return _tourReservationDAO.GetAll();
        }

        public void CreateTourReservation(Tour tour, int passengers)
        {
            _tourReservationDAO.CreateTourReservation(tour, passengers);
        }

        public int CheckAvailability(int id)
        {
            return _tourReservationDAO.CheckAvailability(id);
		}
    }
}
