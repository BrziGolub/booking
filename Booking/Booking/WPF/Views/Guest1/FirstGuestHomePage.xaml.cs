using Booking.Model;
using Booking.Model.Enums;
using Booking.Service;
using Booking.WPF.Views.Guest1;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Booking.View
{
    public partial class FirstGuestHomePage : Window
    {
        public FirstGuestHomePage()
        {
            InitializeComponent();
            this.DataContext = this;
            FrameHomePage.Content = new HomePageFirstGuest(this.FrameHomePage.NavigationService);
        }

        private void MenuItem_Click_HomePage(object sender, RoutedEventArgs e)
        {
            FrameHomePage.Content = new HomePageFirstGuest(this.FrameHomePage.NavigationService);
        }

        private void MenuItem_Click_MyReservations(object sender, RoutedEventArgs e)
        {
            FrameHomePage.Content = new FirstGuestAllReservations(this.FrameHomePage.NavigationService);
           
        }

        private void MenuItem_Click_ResheduleRequests(object sender, RoutedEventArgs e)
        {
            FrameHomePage.Content = new FisrtGuestAllRequests(this.FrameHomePage.NavigationService);
        }

        private void MenuItem_Click_Reviews(object sender, RoutedEventArgs e)
        {
            FrameHomePage.Content = new FirstGuestReviews();
        }

        private void MenuItem_Click_LogOut(object sender, RoutedEventArgs e)
        {
            SignInForm signInForm = new SignInForm();
            signInForm.Show();
            this.Close();
        }

        private void MenuItem_Click_MyProfile(object sender, RoutedEventArgs e)
        {
            FrameHomePage.Content = new FirstGuestProfile();
        }
    }
}
