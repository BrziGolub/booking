using Booking.Model;
using Booking.Model.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Controller
{
    public class AccommodationContoller
    {

        private readonly AccommodationDAO accommodationDAO;

        public AccommodationContoller()
        {
            accommodationDAO =  new AccommodationDAO();
        }

        public List<Accommodation> GetAll()
        {
            return accommodationDAO.GetAll();
        }

        //subcribe?
    }
}
