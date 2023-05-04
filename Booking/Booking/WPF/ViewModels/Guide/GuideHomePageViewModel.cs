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
using Booking.View.Guide;
using Booking.View;
using System.ComponentModel;
using System.Windows.Navigation;
using Booking.Commands;
using Booking.WPF.Views.Guide;

namespace Booking.WPF.ViewModels.Guide
{
    public class GuideHomePageViewModel : IObserver, INotifyPropertyChanged
    {
        public ObservableCollection<Tour> Tours { get; set; }
        public ITourService _tourService { get; set; }

        public Tour SelectedTour { get; set; }
        public User user { get; set; }

        public static string _username;

        public string Username
        {
            get => _username;
            set
            {
                if (_username != value)
                {
                    _username = value;
                    OnPropertyChanged(nameof(Username));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public RelayCommand OpenCreateTour { get; set; }
        public RelayCommand OpenStatisticsAboutTour { get; set; }
        public RelayCommand OpenFollowTourLive { get; set; }
        public RelayCommand OpenViewReviews { get; set; }
        public RelayCommand LogOut { get; set; }
        public RelayCommand CancelTour { get; set; }
        public RelayCommand ShowDescriptionText { get; set; }

        private readonly Window _window;

        public GuideHomePageViewModel(Window window) 
        {
            _window = window;
            _tourService = InjectorService.CreateInstance<ITourService>();

            _tourService.Subscribe(this);

            Tours = new ObservableCollection<Tour>(_tourService.GetGuideTours());

            SetCommands();
        }

        private void CloseWindow()
        { 
            _window.Close();
        }
        public void SetCommands()
        {
            OpenCreateTour = new RelayCommand(ButtonCreateTour);
            OpenStatisticsAboutTour = new RelayCommand(ButtonStatisticAboutTour);
            OpenFollowTourLive = new RelayCommand(ButtonFollowTourLive);
            OpenViewReviews = new RelayCommand(ButtonViewReviews);
            LogOut = new RelayCommand(ButtonLogOut);
            CancelTour = new RelayCommand(ButtonCancelTour);
            ShowDescriptionText = new RelayCommand(ButtonShowDescriptionText);
            
        }

        private void ButtonCreateTour(object param)
        {
            GuideCreateTour guideCreateTour = new GuideCreateTour();
            guideCreateTour.ShowDialog();
        }

        private void ButtonStatisticAboutTour(object param)
        {
            GuideStatisticAboutTours statistics = new GuideStatisticAboutTours();
            statistics.Show();
        }

        private void ButtonFollowTourLive(object param)
        {
            GuideFollowTourLive guideFollowTourLive = new GuideFollowTourLive();
            if (guideFollowTourLive.Tours.Count > 0)
                guideFollowTourLive.Show();
            else
                MessageBox.Show("Today you don't have tours!");
        }

        private void ButtonViewReviews(object param)
        {
            GuideViewReviews reviews = new GuideViewReviews();
            reviews.Show();
        }
        private void ButtonLogOut(object param)
        {
            SignInForm signInForm = new SignInForm();
            signInForm.Show();
            CloseWindow();     
        }

        private void ButtonCancelTour(object param)
        {
            if (SelectedTour != null)
            {
                MessageBoxResult result = ConfirmTourCancel();

                if (result == MessageBoxResult.Yes)
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

        private void ButtonShowDescriptionText(object param)
        {
            ShowDescription showDescriptionText = new ShowDescription(SelectedTour.Id);
            showDescriptionText.Show();
        }
        public void Update()
        {
            Tours.Clear();

            foreach (Tour t in _tourService.GetGuideTours())
            {
                Tours.Add(t);
            }
        }
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
