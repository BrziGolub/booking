using Booking.Model;
using Booking.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Repository
{
    public class AccommodationReservationRequestsRepostiory
    {

        private const string FilePath = "../../Resources/Data/accommodationsReservationsRequests.csv";

        private readonly Serializer<AccommodationReservationRequests> _serializer;

        public List<AccommodationReservationRequests> _accommodationsReservationsRequests;

        public AccommodationReservationRequestsRepostiory()
        {
            _serializer = new Serializer<AccommodationReservationRequests>();
            _accommodationsReservationsRequests = Load();
        }

        public List<AccommodationReservationRequests> Load()
        {
            return _serializer.FromCSV(FilePath);
        }

        public void Save(List<AccommodationReservationRequests> requests)
        {
            _serializer.ToCSV(FilePath, requests);
        }
    }
}
