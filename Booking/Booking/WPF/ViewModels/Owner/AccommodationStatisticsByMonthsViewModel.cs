using Booking.Domain.DTO;
using Booking.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Booking.WPF.ViewModels.Owner
{
    public class AccommodationStatisticsByMonthsViewModel
    {
        private readonly Window _window;
        public AccommodationStatisticsByMonthsViewModel(Accommodation SelectedAccommodation, OwnerYearStatistic SelectedYearStatistics, Window window)
        {
            _window = window;
        }
    }
}
