using Booking.Conversion;
using Booking.Model.Enums;
using Booking.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Model
{
    public class AccommodationReservationRequests : ISerializable
    {
        public int Id { get; set; }
        public AccommodationReservation AccommodationReservation { get; set; }
        public DateTime NewArrivalDay { get; set; }
        public DateTime NewDeparuteDay { get; set; }
        public RequestStatus Status { get; set; }
        public String Comment { get; set; }
            
        public AccommodationReservationRequests(AccommodationReservation accommodationReservation, DateTime newArrivalDay, DateTime newDeparuteDay, RequestStatus status, string comment)
        {
            AccommodationReservation = accommodationReservation;
            NewArrivalDay = newArrivalDay;
            NewDeparuteDay = newDeparuteDay;
            Status = status;
            Comment = comment;
        }

        public AccommodationReservationRequests()
        {
            AccommodationReservation = new AccommodationReservation();
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            AccommodationReservation.Id = int.Parse(values[1]);
            NewArrivalDay = DateConversion.StringToDate(values[2]);
            NewDeparuteDay = DateConversion.StringToDate(values[3]);
            RequestStatus status;

            if (Enum.TryParse<RequestStatus>(values[3], out status))
            {
                Status = status;
            }
            else
            {
                Status = RequestStatus.PENDNING;
                System.Console.WriteLine("Doslo je do greske prilikom ucitavanja tipa smestaja");
            }
            Comment = values[4];
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                AccommodationReservation.Id.ToString(),
                NewArrivalDay.ToString("dd/MM/yyyy"),
                NewDeparuteDay.ToString("dd/MM/yyyy"),
                Status.ToString(),
                Comment
        };
            return csvValues;
        }
    }
}
