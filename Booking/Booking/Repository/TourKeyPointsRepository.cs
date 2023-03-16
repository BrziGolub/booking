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
    public class TourKeyPointsRepository
    {
        private const string FilePath = "../../Resources/Data/tourKeyPoints.csv";

        private readonly Serializer<TourKeyPoints> _serializer;

        public List<TourKeyPoints> _tourKeyPoints;

        public TourKeyPointsRepository()
        {
            _serializer = new Serializer<TourKeyPoints>();
            _tourKeyPoints = Load();
        }
        public List<TourKeyPoints> Load()
        {
            return _serializer.FromCSV(FilePath);
        }
        public void Save(List<TourKeyPoints> tourKeyPoints)
        {
            _serializer.ToCSV(FilePath, tourKeyPoints);
        }

    }
}
