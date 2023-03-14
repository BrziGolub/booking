using Booking.Model.Enums;
using Booking.Model.Images;
using Booking.Observer;
using Booking.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Model.DAO
{
    public class AccommodationDAO : ISubject
    {
        //Add observer?

        private readonly List<IObserver> observers;
        private readonly AccommodationRepository _accommodationRepository;
        private List<Accommodation> accommodations;

        private LocationDAO locationDAO;

        private AccommodationImagesRepository _accommodationImagesRepository;

        public AccommodationDAO()
        {
            _accommodationRepository = new AccommodationRepository();
            observers = new List<IObserver>();
            locationDAO = new LocationDAO();
            _accommodationImagesRepository = new AccommodationImagesRepository();
            accommodations = new List<Accommodation>();
            Load();
        }

        public void Load()
        {
            accommodations = _accommodationRepository.Load();
            BindLocationToAccommodaton();
            BindImagesToAccommodaton();
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

        public void BindImagesToAccommodaton()
        {
            foreach (Accommodation accommodation in accommodations)
            {
                foreach (AccommodationImage accommodationImage in _accommodationImagesRepository.Load())
                {
                    if (accommodationImage.Accomodation.Id == accommodation.Id)
                    {
                        accommodation.Images.Add(accommodationImage);
                    }
                }
            }

        }

        public Boolean isEnumTrue(Accommodation accommodation,List<String> accommodationTypes)
        {
            Boolean info = false;

            if (accommodationTypes.Count == 0)
            {
                return true;
            }
            foreach(String type in accommodationTypes)
            {
                if (accommodation.Type.ToString().Contains(type)){
                    info = true;
                    break;
                }
            }
            return info;
        }

        public void Search(ObservableCollection<Accommodation> observe, string name, string city, string state, List<string> accommodationTypes, string numberOfGuests, string minNumDaysOfReservation)
        {
            observe.Clear();

            foreach (Accommodation accommodation in accommodations)
            {
                if ((accommodation.Name.ToLower().Contains(name.ToLower()) || name.Equals(""))
                    && (accommodation.Location.State.ToLower().Contains(state.ToLower()) || state.Equals(""))
                        && (isEnumTrue(accommodation, accommodationTypes))
                           && (accommodation.Location.City.ToLower().Contains(city.ToLower()) || city.Equals("")) 
                              && (numberOfGuests.Equals("") || int.Parse(numberOfGuests) <= accommodation.Capacity))
                {
                    if (minNumDaysOfReservation.Equals("") || int.Parse(minNumDaysOfReservation) >= accommodation.MinNumberOfDays)
                    {
                        observe.Add(accommodation);
                    }
                }
            }

        }

        public void ShowAll(ObservableCollection<Accommodation> accommodationsObserve)
        {
            accommodationsObserve.Clear();

            foreach(Accommodation accommodation in accommodations)
            {
                accommodationsObserve.Add(accommodation);
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
