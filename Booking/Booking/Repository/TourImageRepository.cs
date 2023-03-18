using Booking.Model;
using Booking.Model.Images;
using Booking.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Repository
{
    public class TourImageRepository
    {
        private const string FilePath = "../../Resources/Data/tourImages.csv";

        private readonly Serializer<TourImage> _serializer;

        public List<TourImage> _tourImages;

        public TourImageRepository()
        {
            _serializer = new Serializer<TourImage>();
            _tourImages = Load();
        }

        public List<TourImage> Load()
        {
            return _serializer.FromCSV(FilePath);
        }
        public void Save(List<TourImage> tourImages)
        {
            _serializer.ToCSV(FilePath, tourImages);
        }
    }
}
