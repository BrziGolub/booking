﻿using Booking.Commands;
using Booking.Domain.Model;
using Booking.Domain.ServiceInterfaces;
using Booking.Model;
using Booking.Service;
using Booking.Util;
using Booking.View;
using Booking.View.Guest2;
using Booking.WPF.Views.Guest2;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace Booking.WPF.ViewModels.Guest2
{
    public class SecondGuestHomePageViewModel : INotifyPropertyChanged
    {
        private readonly Window _window;
        private ITourService _tourService;
        private ILocationService _locationService;
        private ITourReservationService _tourReservationService;
        private ITourGuestsService _tourGuestsService;
        private ITourGradeService _tourGradeService;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Tour> Tours { get; set; }
        public ObservableCollection<TourReservation> ActiveTour { get; set; }

        public List<string> SearchState { get; set; }
        public ObservableCollection<string> SearchCity { get; set; }

        public string SearchDuration { get; set; } = string.Empty;
        public string SearchLanguage { get; set; } = string.Empty;
        public string SearchVisitors { get; set; } = string.Empty;

        public Tour SelectedTour { get; set; }
        public string SelectedState { get; set; }
        public string SelectedCity { get; set; }

        private bool _isEnabled;
        public bool CityComboBoxIsEnabled
        { 
            get => _isEnabled;
            set
            {
                if (_isEnabled != value)
                {
                    _isEnabled = value;
                    OnPropertyChanged();
                }
            } 
        }
        private int _selectedIndex;
        public int CityComboBoxSelectedIndex
        {
            get => _selectedIndex;
            set
            {
                if( _selectedIndex != value)
                {
                    _selectedIndex = value;
                    OnPropertyChanged();
                }
            }
        }

        public RelayCommand Button_Click_Search { get; set; }
        public RelayCommand Button_Click_ShowAll { get; set; }
        public RelayCommand Button_Click_Reserve { get; set; }
        public RelayCommand Button_Click_SignOff { get; set; }
        public RelayCommand Button_Click_ShowImages { get; set; }
        public RelayCommand Button_Click_ShowDestinations { get; set; }
        public RelayCommand Button_Click_ShowVouchers { get; set; }
        public RelayCommand Selection_Changed { get; set; }

        public SecondGuestHomePageViewModel(Window window)
        {
            _window = window;
            _tourService = InjectorService.CreateInstance<ITourService>();
            _locationService = InjectorService.CreateInstance<ILocationService>();
            _tourReservationService = InjectorService.CreateInstance<ITourReservationService>();
            _tourGuestsService = InjectorService.CreateInstance<ITourGuestsService>();
            _tourGradeService = InjectorService.CreateInstance<ITourGradeService>();

            Tours = new ObservableCollection<Tour>(_tourService.GetValidTours());
            ActiveTour = new ObservableCollection<TourReservation> { _tourReservationService.GetActiveTour(TourService.SignedGuideId) };

            SearchState = _locationService.GetAllStates();
            SearchCity = new ObservableCollection<string>();

            SelectedState = "All";
            SelectedCity = "All";

            FillCityComboBox();
            InitializeCommands();
            CheckPresence();
            GradeTour();
        }

        private void InitializeCommands()
        {
            Button_Click_Search = new RelayCommand(ButtonSearch);
            Button_Click_ShowAll = new RelayCommand(ButtonShowAll);
            Button_Click_Reserve = new RelayCommand(ButtonReserveTour);
            Button_Click_SignOff = new RelayCommand(ButtonSignOff);
            Button_Click_ShowImages = new RelayCommand(ButtonShowImages);
            Button_Click_ShowDestinations = new RelayCommand(ButtonShowDestinations);
            Button_Click_ShowVouchers = new RelayCommand(ButtonShowVouchers);
            Selection_Changed = new RelayCommand(ComboBoxStateSelectionChanged);
        }

        public void TourSearch(string state, string city, string duration, string lang, string passengers)
        {
            Tours = _tourService.Search(Tours, state, city, duration, lang, passengers);
        }

        private void ButtonSearch(object param)
        {
            TourSearch(SelectedState, SelectedCity, SearchDuration, SearchLanguage, SearchVisitors);
        }

        public void ShowAll()
        {
            Tours = _tourService.CancelSearch(Tours);
        }

        private void ButtonShowAll(object param)
        {
            ShowAll();
        }

        private bool IsTourSelected()
        {
            if(SelectedTour == null)
            {
                MessageBox.Show("You haven't selected a tour!");
            }
            return SelectedTour != null;
        }

        public void ReserveTourSearch(string state, string city, int id)
        {
            TourSearch(state, city, "", "", "");
            Tours.Remove(Tours.Where(s => s.Id == id).Single());

            if (Tours.Count() == 0)
            {
                MessageBox.Show("All tours at same location are full!");
                ShowAll();
            }
        }

        private void ButtonReserveTour(object param)
        {
            if (IsTourSelected())
            {
                ReserveTour view = new ReserveTour(SelectedTour, this);
                view.Owner = _window;
                view.ShowDialog();
            }
        }

        private void ComboBoxStateSelectionChanged(object param)
        {
            FillCityComboBox();
        }

        private void FillCityComboBox()
        {
            SearchCity = _locationService.GetAllCitiesByState(SearchCity, SelectedState);
            SelectedCity = "All";
            CityComboBoxSelectedIndex = 0;
            if (SelectedState.Equals("All"))
            {
                CityComboBoxIsEnabled = false;
            }
            else
            {
                CityComboBoxIsEnabled = true;
            }
        }

        private void ButtonSignOff(object param)
        {
            SignInForm signInForm = new SignInForm();
            signInForm.Show();
            _window.Close();
        }

        private void ButtonShowImages(object param)
        {
            if (IsTourSelected())
            {
                ShowTourImages view = new ShowTourImages(SelectedTour);
                view.ShowDialog();
            }
            
        }

        private void ButtonShowDestinations(object parma)
        {
            if (IsTourSelected())
            {
                ShowTourDestinations view = new ShowTourDestinations(SelectedTour);
                view.ShowDialog();
            }        }

        private void ButtonShowVouchers(object param)
        {
            ShowVouchers view = new ShowVouchers();
            view.ShowDialog();
        }

        private void CheckPresence()
        {
            TourGuests tourGuest = _tourGuestsService.CheckPresence(TourService.SignedGuideId);
            if (tourGuest != null && !tourGuest.IsPresent)
            {
                MessageBoxResult dialogResult = PresenceOnTourResponse(tourGuest.Tour.Name);
                if (dialogResult == MessageBoxResult.Yes)
                {
                    tourGuest.IsPresent = true;
                    _tourGuestsService.UpdateTourGuest(tourGuest);
                }
            }
        }

        private MessageBoxResult PresenceOnTourResponse(string name)
        {
            string sMessageBoxText = $"Are you present on tour \n{name}?";
            string sCaption = "Presence on tour";

            MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
            MessageBoxImage icnMessageBox = MessageBoxImage.Question;

            MessageBoxResult result = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
            return result;
        }

        private void GradeTour()
        {
            List<TourReservation> tourReservations = _tourReservationService.GetReservationsByGuestId(TourService.SignedGuideId);
            List<TourGrade> tourGrades = _tourGradeService.GetGradesByGuestId(TourService.SignedGuideId);

            foreach (TourReservation tourReservation in tourReservations)
            {
                TourGrade tourGrade = tourGrades.Find(t => t.Tour.Id == tourReservation.Tour.Id);
                if (tourReservation.Tour.IsEnded && tourGrade == null)
                {
                    MessageBoxResult dialogResult = RateTourResponse(tourReservation.Tour.Name);
                    if (dialogResult == MessageBoxResult.Yes)
                    {
                        RateTour view = new RateTour(tourReservation.Tour);
                        view.Show();
                    }
                }
            }
        }

        private MessageBoxResult RateTourResponse(string name)
        {
            string sMessageBoxText = $"Would you like to rate a tour: \n{name}?";
            string sCaption = "Rate a tour";

            MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
            MessageBoxImage icnMessageBox = MessageBoxImage.Question;

            MessageBoxResult result = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
            return result;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
