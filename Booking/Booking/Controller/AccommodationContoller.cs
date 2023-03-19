using Booking.Model;
using Booking.Model.DAO;
using Booking.Model.Enums;
using Booking.Observer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Controller
{
    public class AccommodationContoller
    {

        private readonly AccommodationDAO accommodationDAO;

        public AccommodationContoller()
        {
            accommodationDAO = new AccommodationDAO();
        }

        public AccommodationContoller(AccommodationDAO accommodation)
        {
            accommodationDAO = accommodation;
        }

        public List<Accommodation> GetAll()
        {
            return accommodationDAO.GetAll();
        }

        public void Search(ObservableCollection<Accommodation> accommodations,string SearchName, string SearchCity, string SerachStete, List<string> accommodationTypes, string SerachGuests, string SearchReservationDays)
        {
            accommodationDAO.Search(accommodations, SearchName, SearchCity, SerachStete, accommodationTypes, SerachGuests, SearchReservationDays);
        }

        public void ShowAll(ObservableCollection<Accommodation> accommodations)
        {
            accommodationDAO.ShowAll(accommodations);
        }
        public void Subscribe(IObserver observer)
        {
            accommodationDAO.Subscribe(observer);
        }

        public void Create(Accommodation accommodation)
        {
            accommodationDAO.addAccommodation(accommodation);
        }

        public int NextId() 
        {
            return accommodationDAO.NextId();
        }

    }
}
