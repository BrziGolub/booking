using Booking.Domain.RepositoryInterfaces;
using Booking.Domain.ServiceInterfaces;
using Booking.Model;
using Booking.Model.Images;
using Booking.Repository;
using Booking.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Service
{
    public class TourImageService : ITourImageService // nepotrebno
    {
        private readonly ITourImageRepository _tourImageRepository;

		public TourImageService()
        {
			_tourImageRepository = InjectorRepository.CreateInstance<ITourImageRepository>();
        }

        public void Load()
        {
            //_tourImages = _repository.Load();
        }

        public void Save()
        {
            //_repository.Save(_tourImages);
        }

        public List<TourImage> GetImagesByTourId(int id)
        {
            List<TourImage> images = new List<TourImage>();

            foreach(TourImage image in _tourImageRepository.GetAll())
            {
                if(image.Tour.Id == id)
                    images.Add(image);
            }

            return images;
        }
	}
}
