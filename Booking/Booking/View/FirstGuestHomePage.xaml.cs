using Booking.Model;
using Booking.Model.Enums;
using Booking.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Interaction logic for FirstGuestHomePage.xaml
    /// </summary>
    public partial class FirstGuestHomePage : Window
    {
  

        public FirstGuestHomePage()
        {
            InitializeComponent();
            this.DataContext = this;
          
        }
      
        private void MenuItem_Click_HomePage(object sender, RoutedEventArgs e)
        {
            FrameHomePage.Content = new HomePageFirstGuest();
        }

        private void MenuItem_Click_MyReservations(object sender, RoutedEventArgs e)
        {
            FrameHomePage.Content = new FirstGuestAllReservations();
        }

    }
}
