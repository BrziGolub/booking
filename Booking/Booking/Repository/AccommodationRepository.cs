using Booking.Model;
using Booking.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Repository
{
    public class AccommodationRepository
    {
        private const string FilePath = "../../Resources/Data/accommodation.csv";

        private readonly Serializer<Accommodation> _serializer;

        public List<Accommodation> _accommodations;

        public AccommodationRepository()
        {
            _serializer = new Serializer<Accommodation>();
            _accommodations = Load();
        }

        public List<Accommodation> Load()
        {
            return _serializer.FromCSV(FilePath);
        }

        public void Save(List<Accommodation> accommodation)
        {
            _serializer.ToCSV(FilePath, accommodation);
        }
    }
}
