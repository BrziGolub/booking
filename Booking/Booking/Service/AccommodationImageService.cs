using Booking.Domain.ServiceInterfaces;
using Booking.Model.Images;
using Booking.Observer;
using Booking.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Booking.Service
{
    public class AccommodationImageService : IAccommodationImageService
    {

        private readonly AccommodationImagesRepository _repository;

        private List<AccommodationImage> _images;

		public AccommodationImageService()
        {
            _repository = new AccommodationImagesRepository();
            _images = new List<AccommodationImage>();
            Load();
        }

        public void Load()
        {
            _images = _repository.Load();
        }

        public List<AccommodationImage> GetAll()
        {
            return _images;
        }
        public int GenerateId()
        {
            int maxId = 0;
            foreach (AccommodationImage image in _images)
            {
                if (image.Id > maxId)
                {
                    maxId = image.Id;
                }
            }
            return maxId + 1;
        }

        public void Create(AccommodationImage image)
        {
            image.Id = GenerateId();
            _images.Add(image);
        }


        public void Save()
        {
            _repository.Save(_images);
        }

        public AccommodationImage GetById(int id)
        {
            return _images.Find(image => image.Id == id);
        }

    }
}
