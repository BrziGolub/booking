using Booking.Model;
using Booking.Observer;
using Booking.WPF.ViewModels.Guest1;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.ServiceInterfaces
{
    public interface IAccommodationService : IService<Accommodation>, ISubject
    {
        bool IsAccommodationTypeValid(Accommodation accommodation, List<String> accommodationTypes);
        bool IsCapasityValid(string numberOfGuests, Accommodation accommodation);
        bool IsCityValid(string city, Accommodation accommodation);
        bool IsStateValid(string state, Accommodation accommodation);
        bool IsNameValid(string name, Accommodation accommodation);
        bool IsMinNumberOfDaysValid(string minNumDaysOfReservation, Accommodation accommodation);
        void Search(ObservableCollection<Accommodation> observe, string name, string city, string state, List<string> accommodationTypes, string numberOfGuests, string minNumDaysOfReservation);
        void ShowAll(ObservableCollection<Accommodation> accommodationsObserve);
        Accommodation AddAccommodation(Accommodation accommodation);
        List<Accommodation> GetOwnerAccommodations();
        List<Accommodation> GetAll();
        List<Accommodation> GetAllSuper();
        int GetSignedInOwner();

        //quick serach
        List<AccommodationReservation> GetReservationsForAccommodation(Accommodation accommodation);
        List<AccommodationReservation> FindAcceptableReservations(Accommodation accommodation);
        List<(DateTime, DateTime)> FindAvailableDatesQuick(Accommodation accommodation, int daysToStay);
        List<(DateTime, DateTime)> FindAvailableDatesQuickRanges(Accommodation accommodation, int daysToStay, DateTime initialDate, DateTime endDate);
        bool IsDateAvailable(DateTime date, int daysToStay, List<AccommodationReservation> reservations);
        bool CheckGuestsNumber(Accommodation accommodation, int numberOfGuests);
        bool AccommodationIsAvailable(Accommodation accommodation, int daysToStay);
        bool AccommodationIsAvailableInRange(Accommodation accommodation, int daysToStay, DateTime initialDate, DateTime endDate);


        List<AccommodationDTO> GetAllDTO();

    }
}
