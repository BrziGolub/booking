using Booking.Conversion;
using Booking.Serializer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Model
{
    public class AccommodationReservation : ISerializable
    {

        public int Id { get; set; }
        
        public Accommodation Accommodation { get; set; }

        public DateTime ArrivalDay { get; set; }

        public DateTime DepartureDay { get; set; }

        public User Guest { get; set; }

        public AccommodationReservation()
        {
            Accommodation = new Accommodation();
            Guest = new User();
        }

        public AccommodationReservation(Accommodation accommodation, DateTime arrivalDay, DateTime departureDay)
        {
            this.Accommodation = accommodation;
            this.ArrivalDay = arrivalDay;
            this.DepartureDay = departureDay;
           // this.Guest = guest;
        }


        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            Accommodation.Id = int.Parse(values[1]);
            ArrivalDay = DateConversion.StringToDate(values[2]);
            DepartureDay = DateConversion.StringToDate(values[3]);
            Guest.Id = int.Parse(values[4]);
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                Accommodation.Id.ToString(),
                ArrivalDay.ToString("dd/MM/yyyy"),
                DepartureDay.ToString("dd/MM/yyyy"),
                Guest.Id.ToString()
        };
            return csvValues;
        }
    }
}
