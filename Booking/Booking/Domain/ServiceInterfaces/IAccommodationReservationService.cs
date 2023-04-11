using Booking.Model;
using Booking.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.ServiceInterfaces
{
    public interface IAccommodationReservationService : IService<AccommodationReservation>, ISubject
    {
        List<AccommodationReservation> GetGeustsReservatonst();
        void SaveReservation(AccommodationReservation reservation);
        void Delete(AccommodationReservation selectedReservation);
        bool IsAbleToCancleResrvation(AccommodationReservation selectedReservation);
        List<DateTime> MakeListOfReservedDates(DateTime initialDate, DateTime endDate);
        bool IsDatesMatche(List<DateTime> reservedDatesEntered, List<DateTime> reservedDates);
        bool Reserve(DateTime arrivalDay, DateTime departureDay, Accommodation selectedAccommodation);
        List<DateTime> SetReservedDates(DateTime arrivalDay, DateTime departureDay, Accommodation selected);
        bool IsReservationAvailableToGrade(AccommodationReservation accommodationReservation);
        List<AccommodationReservation> GetAllUngradedReservations();
        List<(DateTime, DateTime)> GetDates(List<DateTime> reservedDates, int difference, DateTime departureDay, DateTime arrivalDay);
        List<(DateTime, DateTime)> SuggestOtherDates(DateTime arrivalDay, DateTime departureDay, Accommodation selectedAccommodation);
    }
}
