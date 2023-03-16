using Booking.Model;
using Booking.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Repository
{
    public class AccommodationResevationRepository
    {
        private const string FilePath = "../../Resources/Data/accommodationsReservations.csv";

        private readonly Serializer<AccommodationReservation> _serializer;

        public List<AccommodationReservation> _accommodations;

        public AccommodationResevationRepository()
        {
            _serializer = new Serializer<AccommodationReservation>();
            _accommodations = Load();
        }

        public List<AccommodationReservation> Load()
        {
            return _serializer.FromCSV(FilePath);
        }

        public void Save(List<AccommodationReservation> accommodationsReservation)
        {
            _serializer.ToCSV(FilePath, accommodationsReservation);
        }
    }
}
