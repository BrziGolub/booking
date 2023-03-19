using Booking.Model;
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

namespace Booking.View
{
    /// <summary>
    /// Interaction logic for ShowTourDestinations.xaml
    /// </summary>
    public partial class ShowTourDestinations : Window
    {
        public ShowTourDestinations(List<TourKeyPoint> tourKeyPoints)
        {
            InitializeComponent();
            DataContext = this;

            DataGridDestinations.ItemsSource = tourKeyPoints;
        }

        private void ButtonClose(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
