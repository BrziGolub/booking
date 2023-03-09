using Booking.Observer;
using Booking.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Model.DAO
{
    public class AccommodationDAO : ISubject
    {
        private readonly AccommodationRepository _accommodationRepository;

        private List<Accommodation> accommodations;

        private readonly List<IObserver> observers;

        private LocationDAO locationDAO;


        public AccommodationDAO()
        {
            _accommodationRepository = new AccommodationRepository();
            observers = new List<IObserver>();
            locationDAO = new LocationDAO();
            accommodations = new List<Accommodation>();
            Load();
        }

        public void Load()
        {
            accommodations = _accommodationRepository.Load();
            BindLocationToAccommodaton();
        }

        public Accommodation GetByID(int id)
        {
            return accommodations.Find(accommodation => accommodation.Id == id);
        }

        public List<Accommodation> GetAll()
        {
            return accommodations;
        }


        public  void BindLocationToAccommodaton()
        {
            locationDAO.Load();

            foreach(Accommodation accommodation in accommodations)
            {
                Location location = locationDAO.FindById(accommodation.Location.Id);
                accommodation.Location = location;
            }

        }


        public void Subscribe(IObserver observer)
        {
            observers.Add(observer);
        }

        public void Unsubscribe(IObserver observer)
        {
            observers.Remove(observer);
        }

        public void NotifyObservers()
        {
            foreach(var observer in observers)
            {
                observer.Update();
            }
        }
    }
}
