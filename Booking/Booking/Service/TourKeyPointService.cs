using Booking.Model;
using Booking.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Service
{
    public class TourKeyPointService
    {
        private readonly TourKeyPointRepository _repository;
        private List<TourKeyPoint> _tourKeyPoints;

        private LocationService _locationService;

        public TourKeyPointService()
        {
            _repository = new TourKeyPointRepository();
            _tourKeyPoints = new List<TourKeyPoint>();

            _locationService = new LocationService();
        }

        public void Load()
        {
            _tourKeyPoints = _repository.Load();

            LoadLocations();
        }

        public void Save()
        {
            _repository.Save(_tourKeyPoints);
        }

        public TourKeyPoint GetById(int id)
        {
            return _tourKeyPoints.Find(v => v.Id == id);
        }

        public List<TourKeyPoint> GetAll()
        {
            return _tourKeyPoints;
        }

        public void LoadLocations()
        {
            _locationService.Load();

            foreach (TourKeyPoint keyPoint in _tourKeyPoints)
            {
                keyPoint.Location = _locationService.GetById(keyPoint.Location.Id);
            }
        }

        public List<TourKeyPoint> GetKeyPointsByTourId(int id)
        {
            List<TourKeyPoint> keyPoints = new List<TourKeyPoint>();

            foreach (TourKeyPoint keypoint in _tourKeyPoints)
            {
                if (keypoint.Tour.Id == id)
                    keyPoints.Add(keypoint);
            }

            return keyPoints;
        }

		public int NextId()
		{
			if (_tourKeyPoints.Count == 0)
			{
				return 1;
			}
			else
			{
				return _tourKeyPoints.Max(t => t.Id) + 1;
			}
		}

		public TourKeyPoint UpdateKeyPoint(TourKeyPoint tourKeyPoint)
		{
			TourKeyPoint oldTourKeyPoint = GetById(tourKeyPoint.Id);

			oldTourKeyPoint.Tour = tourKeyPoint.Tour;
			oldTourKeyPoint.Location = tourKeyPoint.Location;
			oldTourKeyPoint.Achieved = tourKeyPoint.Achieved;

			Save();

			return oldTourKeyPoint;
		}
	}
}
