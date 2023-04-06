using Booking.Model;
using Booking.Observer;
using Booking.Repository;
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

namespace Booking.View
{
    public partial class GuideKeyPointsCheck : Window, IObserver
    {

        public ObservableCollection<TourKeyPoint> KeyPoints { get; set; }

        public ObservableCollection<User> Guests { get; set; }

        public TourService TourService { get; set; }
        public UserService UserService{ get; set; } 
        public TourGuestsService TourGuestsService { get; set; }
      
        public TourKeyPoint SelectedTourKeyPoint { get; set; }
        public Tour SelectedTour { get; set; }
        public User SelectedGuest { get; set; }

        public TourGuests tourGuests;

       
        public GuideKeyPointsCheck(int idTour)
        {
            InitializeComponent();
            this.DataContext = this;

			var app = Application.Current as App;

			TourService = app.TourService;
			TourService.Subscribe(this);
            
            UserService = app.UserService;
            UserService.Subscribe(this);

            TourGuestsService = app.TourGuestsService;
            TourGuestsService.Subscribe(this);
            tourGuests = new TourGuests();


            SelectedTour = TourService.GetById(idTour);

            Guests = new ObservableCollection<User>(UserService.GetGuests());
            KeyPoints = new ObservableCollection<TourKeyPoint>(TourService.GetSelectedTourKeyPoints(SelectedTour.Id));
           

            KeyPoints[0].Achieved = true;
            
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
                MessageBox.Show(SelectedTourKeyPoint.Location.State.ToString() + " " + SelectedTourKeyPoint.Location.City.ToString() + " is achieved!");
                TourService.UpdateKeyPoint(SelectedTourKeyPoint);

                if (KeyPoints[KeyPoints.Count() - 1].Achieved == true)
                {
                    SelectedTour.IsStarted = false;
                    MessageBox.Show("Tour ended, you achieved last keypoint!"); 
                    this.Close();
                }
            }
            else 
            {
                MessageBox.Show("You must first mark the keypoint that has been achieved!");
            }
        }

        private void AddGuest(object sender, RoutedEventArgs e)
        {
            if(SelectedGuest != null && SelectedTourKeyPoint != null) 
            {

                tourGuests.Tour.Id = SelectedTour.Id;
                tourGuests.User.Id = SelectedGuest.Id;
                tourGuests.TourKeyPoint.Id = SelectedTourKeyPoint.Id;

                TourGuestsService.Create(tourGuests);

                MessageBox.Show("added");
                //MessageBox.Show("Guest '" + SelectedGuest.Username.ToString() + "' is added to keypoint '" + SelectedTourKeyPoint.Location.State.ToString() + ", " + SelectedTourKeyPoint.Location.City.ToString() + "'");
            }
            else
            {
                MessageBox.Show("You must first mark the guest and checkpoint who you want to add and where!");
            }

            //treba da ih ponistim
                //SelectedTourKeyPoint = null;
                //SelectedGuest = null;
        }

    }
}
