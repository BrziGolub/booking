using Booking.Domain.ServiceInterfaces;
using Booking.Model.Images;
using Booking.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Service
{
    public class GuestsAccommodationImagesService : IGuestsAccommodationImagesService
    {
        private readonly GuestsAccommodationImagesRepository _repository;

        private List<AccommodationImage> _images;

        public GuestsAccommodationImagesService()
        {
            _repository = new GuestsAccommodationImagesRepository();
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
