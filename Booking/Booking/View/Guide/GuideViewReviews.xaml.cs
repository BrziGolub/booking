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

namespace Booking.View.Guide
{
    /// <summary>
    /// Interaction logic for GuideViewReviews.xaml
    /// </summary>
    public partial class GuideViewReviews : Window
    {
        public GuideViewReviews()
        {
            InitializeComponent();
            this.DataContext = this;


        }

        private void ShowStatistic(object sender, RoutedEventArgs e)
        {

        }

        private void ResetStatistic(object sender, RoutedEventArgs e)
        {

        }

        private void Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
