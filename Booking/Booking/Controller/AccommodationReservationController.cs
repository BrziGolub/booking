using Booking.Model;
using Booking.Model.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Controller
{
    public class AccommodationReservationController
    {
        private readonly AccommodationReservationDAO accommodationReservationDAO;

        public AccommodationReservationController()
        {
            accommodationReservationDAO = new AccommodationReservationDAO();
        }

        public List<AccommodationReservation> GetAll()
        {
            return accommodationReservationDAO.GetAll();
        }

        public Boolean Reserve(DateTime ArrivalDay,DateTime DepartureDay,Accommodation SelectedAccommodation)
        {
            return accommodationReservationDAO.Reserve(ArrivalDay, DepartureDay, SelectedAccommodation);
        }

        public List<(DateTime, DateTime)> SuggestOtherDates(DateTime ArrivalDay, DateTime DepartureDay, Accommodation SelectedAccommodation)
        {
            return accommodationReservationDAO.SuggestOtherDates(ArrivalDay, DepartureDay, SelectedAccommodation);
        }

        public void SaveReservation(AccommodationReservation newReservation)
        {
            accommodationReservationDAO.SaveReservation(newReservation);
        }

    }
}
