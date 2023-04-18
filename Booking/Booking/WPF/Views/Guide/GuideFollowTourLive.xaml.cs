using Booking.Domain.ServiceInterfaces;
using Booking.Model;
using Booking.Model.Images;
using Booking.Observer;
using Booking.Repository;
using Booking.Service;
using Booking.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlTypes;
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
    public partial class GuideFollowTourLive : Window , IObserver
    {
        public ObservableCollection<Tour> Tours { get; set; }
        public ITourService TourService { get; set; }
        public Tour SelectedTour { get; set; }

        int Pomid { get; set; }


        public GuideFollowTourLive()
        {
            InitializeComponent();
            this.DataContext = this;

            TourService = InjectorService.CreateInstance<ITourService>();

            TourService.Subscribe(this);

            Pomid = -1;
            Tours = new ObservableCollection<Tour>(TourService.GetTodayTours());
        }

        private void StartTour(object sender, RoutedEventArgs e)
        {

            int startedTours = 0;
            startedTours = CheckNumberOfStartedTours(startedTours);

            if (startedTours < 1)
            {
                StartSelectedTour();
            }
            else
            {
                MessageBox.Show("You can start maximum 1 tour in same time!");
            }
        }

        private void StartSelectedTour()
        {
            if (SelectedTour != null)
            {
                GuideKeyPointsCheck guideKeyPointsCheck = new GuideKeyPointsCheck(SelectedTour.Id);
                Pomid = SelectedTour.Id;
                if (SelectedTour.IsEnded)
                {
                    MessageBox.Show("You cant start ended tour!");
                }
                else
                {
                    SelectedTour.IsStarted = true;
                    TourService.UpdateTour(SelectedTour);
                    MessageBox.Show(SelectedTour.Name.ToString() + " is started!");
                    TourService.NotifyObservers();

                    guideKeyPointsCheck.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("In order to start the tour, you first need to select it!");
            }
        }

        private int CheckNumberOfStartedTours(int startedTours)
        {
            foreach (var tour in Tours)
            {
                if (tour.IsStarted == true)
                {
                    startedTours++;
                    Pomid = tour.Id;
                }

            }

            return startedTours;
        }

        public void Update()
        {  
            Tours.Clear();
            foreach(Tour t in TourService.GetTodayTours())
            {
                Tours.Add(t);
            }
        }

        private void EndTour(object sender, RoutedEventArgs e)
        {
            if (SelectedTour != null && SelectedTour.IsStarted == true)
            {
                SelectedTour.IsStarted = false;
                SelectedTour.IsEnded = true;
                TourService.UpdateTour(SelectedTour);
                MessageBox.Show(SelectedTour.Name.ToString() + " is ended!");
                TourService.NotifyObservers();

            }
            else
            {
                MessageBox.Show("In order to end the tour, you first need to select started tour!");
            }
        }

        private void ShowOnGoingTour(object sender, RoutedEventArgs e)
        {
            int pomid1 = -1;
                foreach (var t in Tours)
                {
                if (t.IsStarted == true)
                    pomid1 = t.Id;
                else Pomid = -1;
                }

            Pomid = pomid1;

            Tour tour = TourService.GetById(Pomid);
                if (tour != null)
                {
                    GuideKeyPointsCheck guideKeyPointsCheck = new GuideKeyPointsCheck(tour.Id);
                    guideKeyPointsCheck.ShowDialog();
                } 
                else
                {
                    MessageBox.Show("You don't have ongoing tour!");
                }         
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
