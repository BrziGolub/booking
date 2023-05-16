using Booking.Commands;
using Booking.Domain.ServiceInterfaces;
using Booking.Model;
using Booking.Observer;
using Booking.Util;
using Booking.View;
using Booking.WPF.Views.Owner;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using VirtualKeyboard.Wpf;

namespace Booking.WPF.ViewModels.Owner
{
    public class OwnerHomePageViewModel: IObserver
    {
        public IAccommodationReservationService AccommodationReservationService { get; set; }
        public ObservableCollection<AccommodationReservation> Reservations { get; set; }
        public Accommodation SelectedAccommodation { get; set; }
        public ObservableCollection<Accommodation> Accommodations { get; set; }
        public IAccommodationService _accommodationService { get; set; }
        public RelayCommand OpenRegisterAccommodation { get; set; }
        public RelayCommand OpenDateMove { get; set; }
        public RelayCommand OpenGradingGuests { get; set; }
        public RelayCommand OpenViewReviews { get; set; }
        public RelayCommand OwnerLogOut { get; set; }
        public RelayCommand OpenSuperOwner { get; set; }
        public RelayCommand OpenSchedulingRenovations { get; set; }
        public RelayCommand OpenShowRenovations { get; set; }
        public RelayCommand ShowPictures { get; set; }

        private readonly Window _window;
        public OwnerHomePageViewModel(Window window)
        {
            VKeyboard.Listen<TextBox>(e => e.Text);
            VKeyboard.Config(typeof(KeyBoardCustom));
            _window = window;
            _accommodationService = InjectorService.CreateInstance<IAccommodationService>();
            AccommodationReservationService = InjectorService.CreateInstance<IAccommodationReservationService>();
            _accommodationService.Subscribe(this);

            Reservations = new ObservableCollection<AccommodationReservation>(AccommodationReservationService.GetAllUngradedReservations());
            Accommodations = new ObservableCollection<Accommodation>(_accommodationService.GetOwnerAccommodations());
            if (Reservations.Count != 0)
            {
                MessageBox.Show("Number of guests to grade: " + Reservations.Count.ToString());
            }
            SetCommands();
        }
        public void SetCommands()
        {
            OpenRegisterAccommodation = new RelayCommand(RegisterAccomodation);
            OpenDateMove = new RelayCommand(DateMove);
            OpenGradingGuests = new RelayCommand(GradingGuests);
            OpenViewReviews = new RelayCommand(ViewReviews);
            OwnerLogOut = new RelayCommand(LogOut);
            OpenSuperOwner = new RelayCommand(SuperOwner);
            ShowPictures = new RelayCommand(OpenPictures);
            OpenSchedulingRenovations = new RelayCommand(SchedulingRenovation);
            OpenShowRenovations = new RelayCommand(ShowRenovation);
        }


        private void RegisterAccomodation(object param)
        {
            OwnerRegisterAccommodation ownerRegisterAccommodation = new OwnerRegisterAccommodation();
            ownerRegisterAccommodation.Show();
        }
        private void ViewReviews(object param)
        {
            OwnerViewReviews ownerViewReviews = new OwnerViewReviews();
            ownerViewReviews.Show();
        }
        private void ShowRenovation(object param)
        {
            ShowRenovations showRenovations = new ShowRenovations();
            showRenovations.Show();
        }
        private void SchedulingRenovation(object param)
        {
            if (SelectedAccommodation == null)
            {
                MessageBox.Show("Select a accommodation for renovation please");
                return;
            }
            else
            {
                SchedulingRenovations schedulingRenovations = new SchedulingRenovations(SelectedAccommodation);
                schedulingRenovations.Show();
            }
        }

        private void GradingGuests(object param)
        {
            OwnerGradingGuests ownerGradingGuests = new OwnerGradingGuests();
            ownerGradingGuests.Show();
        }
        private void DateMove(object param)
        {
            OwnerDateMove ownerDateMove = new OwnerDateMove();
            ownerDateMove.Show();
        }

        private void SuperOwner(object param)
        {
            SuperOwner superOwner = new SuperOwner();
            superOwner.Show();
        }

        public void Update()
        {
            Accommodations.Clear();

            foreach (Accommodation a in _accommodationService.GetOwnerAccommodations())
            {
                Accommodations.Add(a);
            }
        }
        private void OpenPictures(object param)
        {
            ShowOwnerAccommodationImages showOwnerAccommodationImages = new ShowOwnerAccommodationImages(SelectedAccommodation);
            showOwnerAccommodationImages.Show();
        }
        private void LogOut(object param)
        {
            SignInForm signInForm = new SignInForm();
            signInForm.Show();
            CloseWindow();
        }
        private void CloseWindow()
        {
            _window.Close();
        }
    }
}
