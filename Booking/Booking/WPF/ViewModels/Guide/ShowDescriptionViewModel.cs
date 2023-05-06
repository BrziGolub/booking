using Booking.Domain.ServiceInterfaces;
using Booking.Model;
using Booking.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.WPF.ViewModels.Guide
{
    public class ShowDescriptionViewModel
    {
        public ITourService _tourService { get; set; }

        public string DescriptionTB { get; set; }
        public ShowDescriptionViewModel(int idTour)
        {
            _tourService = InjectorService.CreateInstance<ITourService>();
            Tour tour = _tourService.GetById(idTour);
            DescriptionTB = tour.Description;
        }
    }
}
