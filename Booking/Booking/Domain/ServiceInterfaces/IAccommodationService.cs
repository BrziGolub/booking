using Booking.Model;
using Booking.Observer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.ServiceInterfaces
{
    public interface IAccommodationService : IService<Accommodation>
    {
        void Load();
        void BindUserToAccommodation();
        void BindLocationToAccommodaton();
        void BindImagesToAccommodaton();
        Boolean IsEnumTrue(Accommodation accommodation, List<String> accommodationTypes);
        void Search(ObservableCollection<Accommodation> observe, string name, string city, string state, List<string> accommodationTypes, string numberOfGuests, string minNumDaysOfReservation);
        void ShowAll(ObservableCollection<Accommodation> accommodationsObserve);
        void Subscribe(IObserver observer);
        void Unsubscribe(IObserver observer);
        void NotifyObservers();
        int NextId();
        Accommodation AddAccommodation(Accommodation accommodation);
        List<Accommodation> GetOwnerAccommodations();
    }
}
