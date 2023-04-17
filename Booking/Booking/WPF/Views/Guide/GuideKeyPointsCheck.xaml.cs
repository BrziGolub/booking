using Booking.Domain.ServiceInterfaces;
using Booking.Model;
using Booking.Observer;
using Booking.Repository;
using Booking.Service;
using Booking.Util;
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

namespace Booking.View
{
    public partial class GuideKeyPointsCheck : Window, IObserver
    {

        public ObservableCollection<TourKeyPoint> KeyPoints { get; set; }
        public ObservableCollection<User> Guests { get; set; }

        public ITourService TourService { get; set; }
        public IUserService UserService { get; set; }
        public ITourGuestsService TourGuestsService { get; set; }
        public IVoucherService VoucherService { get; set; }

        public TourKeyPoint SelectedTourKeyPoint { get; set; }
        public Tour SelectedTour { get; set; }
        public User SelectedGuest { get; set; }

        public TourGuests tourGuests = new TourGuests();

       
        public GuideKeyPointsCheck(int idTour)
        {
            InitializeComponent();
            this.DataContext = this;

			TourService = InjectorService.CreateInstance<ITourService>();
            TourService.Subscribe(this);
            
            UserService = InjectorService.CreateInstance<IUserService>();
            UserService.Subscribe(this);

            TourGuestsService = InjectorService.CreateInstance<ITourGuestsService>();
            TourGuestsService.Subscribe(this);
            
            VoucherService = InjectorService.CreateInstance<IVoucherService>();
            VoucherService.Subscribe(this);

            SelectedTour = TourService.GetById(idTour);

            Guests = new ObservableCollection<User>(UserService.GetGuests());
            KeyPoints = new ObservableCollection<TourKeyPoint>(TourService.GetSelectedTourKeyPoints(SelectedTour.Id));

            KeyPoints[0].Achieved = true;
            TourService.UpdateKeyPoint(KeyPoints[0]);

            
        }

        public void Update()
        {
            KeyPoints.Clear();
            foreach (TourKeyPoint keyPoint in TourService.GetSelectedTourKeyPoints(SelectedTour.Id)) 
            {
                KeyPoints.Add(keyPoint);
            }

            Guests.Clear();
            foreach(User user in UserService.GetGuests())
            {
                Guests.Add(user);
            }
        }

        private void AchieveKeypoint(object sender, RoutedEventArgs e)
        {
            if (SelectedTourKeyPoint != null)
            {
                SelectedTourKeyPoint.Achieved = true;
                TourService.UpdateKeyPoint(SelectedTourKeyPoint);
                MessageBox.Show(SelectedTourKeyPoint.Location.State.ToString() + " " + SelectedTourKeyPoint.Location.City.ToString() + " is achieved!");
                TourService.NotifyObservers();

                if (KeyPoints[KeyPoints.Count() - 1].Achieved == true)
                {
                    SelectedTour.IsStarted = false;
                    TourService.UpdateTour(SelectedTour);
                    MessageBox.Show("Tour ended, you achieved last keypoint!");
                    TourService.NotifyObservers();
                    
                    this.Close();
                }
            }
            else 
            {
                MessageBox.Show("You must first mark the keypoint that has been achieved!");
            }
        }

        private MessageBoxResult ConfirmVoucherUse()
        {
            string sMessageBoxText = $"Are guest want to use voucher?";
            string sCaption = "Voucher";

            MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
            MessageBoxImage icnMessageBox = MessageBoxImage.Warning;

            MessageBoxResult result = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);

            return result;
        }
        private void AddGuest(object sender, RoutedEventArgs e)
        {
            List<Voucher> pomVouchers = new List<Voucher>();
            


            tourGuests.Voucher = false;
            if(SelectedGuest != null && SelectedTourKeyPoint != null ) 
            {
                if (TourService.checkTourGuests(SelectedTour.Id, SelectedGuest.Id) == true)
                {
                    
                    tourGuests.Tour.Id = SelectedTour.Id;
                    tourGuests.User.Id = SelectedGuest.Id;
                    tourGuests.TourKeyPoint.Id = SelectedTourKeyPoint.Id;
                   
                    foreach(Voucher v in VoucherService.GetUserVouchers(tourGuests.User.Id))
                    {
                        Voucher pomVoucher = VoucherService.GetById(v.Id);
                        MessageBoxResult result = ConfirmVoucherUse();
                        if (v.IsActive && result == MessageBoxResult.Yes) 
                        {
                            tourGuests.Voucher = true;
                            pomVoucher.IsActive = false;
                            MessageBox.Show("Guest " + SelectedGuest.Username.ToString() + " used voucher");
                            pomVouchers.Add(pomVoucher);
                        }


                    }

                        TourGuestsService.Create(tourGuests);
                        
                        foreach(Voucher v in pomVouchers)
                        {
                        VoucherService.Update(v);
                        }

                    MessageBox.Show("Guest '" + SelectedGuest.Username.ToString() + "' is added to keypoint '" + SelectedTourKeyPoint.Location.State.ToString() + ", " + SelectedTourKeyPoint.Location.City.ToString() + "'");
                }
                else
                {
                    MessageBox.Show("Guest is already added to this tour");
                }
            }
            else
            {
                MessageBox.Show("You must first mark the guest and checkpoint who you want to add and where!");
            }

        }




    }
}
