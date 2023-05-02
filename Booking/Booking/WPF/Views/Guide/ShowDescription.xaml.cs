using Booking.Domain.ServiceInterfaces;
using Booking.Model;
using Booking.Service;
using Booking.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Booking.WPF.Views.Guide
{
    /// <summary>
    /// Interaction logic for ShowDescription.xaml
    /// </summary>
    public partial class ShowDescription : Window
    {
        public ITourService _tourService { get; set; }
        public ShowDescription(int idTour)
        {
            InitializeComponent();
            _tourService = InjectorService.CreateInstance<ITourService>();
            Tour tour = _tourService.GetById(idTour);
            tbDescription.Text = tour.Description;
        }
    }
}
