using Booking.Commands;
using Booking.Domain.ServiceInterfaces;
using Booking.Model;
using Booking.Observer;
using Booking.Service;
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
        public IUserService _userService { get; set; }
        public RelayCommand OpenRegisterAccommodation { get; set; }
        public RelayCommand OpenSiteProposals { get; set; }
        public RelayCommand OpenDateMove { get; set; }
        public RelayCommand OpenGradingGuests { get; set; }
        public RelayCommand OpenViewReviews { get; set; }
        public RelayCommand OwnerLogOut { get; set; }
        public RelayCommand OpenSuperOwner { get; set; }
        public RelayCommand OpenAccommodationStatistics { get; set; }
        public RelayCommand OpenSchedulingRenovations { get; set; }
        public RelayCommand OpenShowRenovations { get; set; }
        public RelayCommand OpenForums { get; set; }
        public RelayCommand Wizard { get; set; }
        public RelayCommand ShowPictures { get; set; }

        private readonly Window _window;
        public OwnerHomePageViewModel(Window window)
        {
            VKeyboard.Listen<TextBox>(e => e.Text);
            VKeyboard.Config(typeof(KeyBoardCustom));
            _window = window;
            _accommodationService = InjectorService.CreateInstance<IAccommodationService>();
            _userService = InjectorService.CreateInstance<IUserService>();
            AccommodationReservationService = InjectorService.CreateInstance<IAccommodationReservationService>();
            _accommodationService.Subscribe(this);
            User user1 = new User();
            user1 = _userService.GetById(AccommodationService.SignedOwnerId);
            Reservations = new ObservableCollection<AccommodationReservation>(AccommodationReservationService.GetAllUngradedReservations());
            Accommodations = new ObservableCollection<Accommodation>(_accommodationService.GetOwnerAccommodations());
            if (Reservations.Count != 0)
            {
                System.Windows.MessageBox.Show("Number of guests to grade: " + Reservations.Count.ToString(), "Information", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
            }
            if (user1.Wizard == 0) 
            {
                user1.Wizard = 1;
                _userService.SaveWizard(user1);
                Wizard wizard = new Wizard(0);
                wizard.Show();
            }
            SetCommands();
        }
        public void SetCommands()
        {
            OpenRegisterAccommodation = new RelayCommand(RegisterAccomodation);
            OpenSiteProposals = new RelayCommand(SiteProposal);
            OpenDateMove = new RelayCommand(DateMove);
            OpenGradingGuests = new RelayCommand(GradingGuests);
            OpenViewReviews = new RelayCommand(ViewReviews);
            OwnerLogOut = new RelayCommand(LogOut);
            OpenSuperOwner = new RelayCommand(SuperOwner);
            ShowPictures = new RelayCommand(OpenPictures);
            OpenSchedulingRenovations = new RelayCommand(SchedulingRenovation);
            OpenShowRenovations = new RelayCommand(ShowRenovation);
            OpenAccommodationStatistics = new RelayCommand(AccommodationStatistics);
            Wizard = new RelayCommand(OpenWizard);
            OpenForums = new RelayCommand(ForumsOwner);
        }


        private void RegisterAccomodation(object param)
        {
            OwnerRegisterAccommodation ownerRegisterAccommodation = new OwnerRegisterAccommodation();
            ownerRegisterAccommodation.Show();
        }
        private void ForumsOwner(object param)
        {
            OwnerAllForums ownerAllForums = new OwnerAllForums();
            ownerAllForums.Show();
        }
        private void SiteProposal(object param)
        {
            SiteProposals siteProposals = new SiteProposals();
            siteProposals.Show();
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
                System.Windows.MessageBox.Show("Select a accommodation for renovation please", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);                
                return;
            }
            else
            {
                SchedulingRenovations schedulingRenovations = new SchedulingRenovations(SelectedAccommodation);
                schedulingRenovations.Show();
            }
        }
        private void AccommodationStatistics(object param)
        {
            if (SelectedAccommodation == null)
            {
                System.Windows.MessageBox.Show("Select a accommodation please", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);               
                return;
            }
            else
            {
                AccommodationStatisticsByYears accommodationStatisticsByYears = new AccommodationStatisticsByYears(SelectedAccommodation);
                accommodationStatisticsByYears.Show();
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
        private void OpenWizard(object param)
        {
            Wizard wizard = new Wizard(0);
            wizard.Show();
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
