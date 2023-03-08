using Booking.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Model.DAO
{
    public class AccommodationDAO
    {
        //Add observer?

        private readonly AccommodationRepository _accommodationRepository;

        private List<Accommodation> accommodations;

        public AccommodationDAO()
        {
            _accommodationRepository = new AccommodationRepository();
            Load();
        }

        public void Load()
        {
            accommodations = _accommodationRepository.Load();
        }

        public Accommodation GetByID(int id)
        {
            return accommodations.Find(accommodation => accommodation.Id == id);
        }

        public List<Accommodation> GetAll()
        {
            return accommodations;
        }

    }
}
