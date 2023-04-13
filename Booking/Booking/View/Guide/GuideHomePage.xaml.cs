using Booking.Model;
using Booking.Observer;
using Booking.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Resources;
using Booking.Domain.ServiceInterfaces;
using Booking.Util;

namespace Booking.View
{
    /// <summary>
    /// Interaction logic for GuideHomePage.xaml
    /// </summary>
    public partial class GuideHomePage : Window, IObserver
    {
        public ObservableCollection<Tour> Tours { get; set; }
        public ITourService _tourService { get; set; }

        public Tour SelectedTour { get; set; }
        public User user { get; set; }

        public static string Username;

        public GuideHomePage()
        {
            InitializeComponent();            
            this.DataContext = this;
            _tourService = InjectorService.CreateInstance<ITourService>();
            
            _tourService.Subscribe(this);
      
            Tours = new ObservableCollection<Tour>(_tourService.GetGuideTours());

            usernameTextBlock.Text = Username;
            
        }

        private void OpenCreateTour(object sender, RoutedEventArgs e)
        {
        GuideCreateTour guideCreateTour = new GuideCreateTour();
        guideCreateTour.Show();
            this.Close();
        }

        private void OpenFollowTourLive(object sender, RoutedEventArgs e)
        {
            GuideFollowTourLive guideFollowTourLive = new GuideFollowTourLive();
            guideFollowTourLive.Show();
        }

        public void Update()
        {
            Tours.Clear();

            foreach (Tour t in _tourService.GetGuideTours())
            {
                Tours.Add(t);
            }
        }

        private void CancelTour(object sender, RoutedEventArgs e)
        {
            if(SelectedTour != null)
            {
                MessageBoxResult result = ConfirmTourCancel();

                if(result == MessageBoxResult.Yes)
                {
                    _tourService.removeTour(SelectedTour.Id);
                }   
            }
            else
            {
                MessageBox.Show("You need to select tour if you want to cancel it!");
            }


        }

        private MessageBoxResult ConfirmTourCancel()
        {
            string sMessageBoxText = $"Are you sure to cancel tour\n{SelectedTour.Name}";
            string sCaption = "Confirmation of cancellation";

            MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
            MessageBoxImage icnMessageBox = MessageBoxImage.Warning;

            MessageBoxResult result = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);

            return result;
        }

        private void LogOut(object sender, RoutedEventArgs e)
        {
            SignInForm signInForm = new SignInForm();
            signInForm.Show();
            this.Close();
        }

        private void OpenStatisticsAboutTour(object sender, RoutedEventArgs e)
        {
            GuideStatisticAboutTours statistics = new GuideStatisticAboutTours();
            statistics.Show();
        }
    }
}
