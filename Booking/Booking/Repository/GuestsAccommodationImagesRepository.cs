using Booking.Model.Images;
using Booking.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Repository
{
    public class GuestsAccommodationImagesRepository
    {
        private const string FilePath = "../../Resources/Data/guestsAccommodationImages.csv";

        private readonly Serializer<AccommodationImage> _serializer;

        public List<AccommodationImage> _accommodationsImages;

        public GuestsAccommodationImagesRepository()
        {
            _serializer = new Serializer<AccommodationImage>();
            _accommodationsImages = Load();
        }

        public List<AccommodationImage> Load()
        {
            return _serializer.FromCSV(FilePath);
        }

        public void Save(List<AccommodationImage> accommodationsImages)
        {
            _serializer.ToCSV(FilePath, accommodationsImages);
        }

    }
}
