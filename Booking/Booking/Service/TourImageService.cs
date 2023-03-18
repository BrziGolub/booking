using Booking.Model;
using Booking.Model.Images;
using Booking.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Service
{
    public class TourImageService
    {
        private readonly TourImageRepository _repository;
        private List<TourImage> _tourImages;

        public TourImageService()
        {
            _repository = new TourImageRepository();
            _tourImages = new List<TourImage>();

            Load();
        }

        public void Load()
        {
            _tourImages = _repository.Load();
        }

        public void Save()
        {
            _repository.Save(_tourImages);
        }

        public TourImage GetById(int id)
        {
            return _tourImages.Find(v => v.Id == id);
        }

        public List<TourImage> GetAll()
        {
            return _tourImages;
        }

        public List<TourImage> GetImagesByTourId(int id)
        {
            List<TourImage> images = new List<TourImage>();

            foreach(TourImage image in _tourImages)
            {
                if(image.Tour.Id == id)
                    images.Add(image);
            }

            return images;
        }
    }
}
