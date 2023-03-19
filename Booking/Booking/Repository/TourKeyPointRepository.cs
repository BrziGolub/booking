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
    public class TourKeyPointRepository
    {
        private const string FilePath = "../../Resources/Data/tourKeyPoints.csv";

        private readonly Serializer<TourKeyPoint> _serializer;

        public List<TourKeyPoint> _tourKeyPoints;

        public TourKeyPointRepository()
        {
            _serializer = new Serializer<TourKeyPoint>();
            _tourKeyPoints = Load();
        }
        public List<TourKeyPoint> Load()
        {
            return _serializer.FromCSV(FilePath);
        }
        public void Save(List<TourKeyPoint> tourKeyPoints)
        {
            _serializer.ToCSV(FilePath, tourKeyPoints);
        }

    }
}
