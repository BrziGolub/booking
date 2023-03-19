using Booking.Controller;
using Booking.Model;
using Booking.Model.Images;
using Booking.Observer;
using Booking.Repository;
using Booking.Service;
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
    /// <summary>
    /// Interaction logic for GuideFollowTourLive.xaml
    /// </summary>
    public partial class GuideFollowTourLive : Window , IObserver
    {
        public ObservableCollection<Tour> Tours { get; set; }
        
        public TourService TourService { get; set; }

        public TourImageRepository _tourImageRepository { get; set; }

        public Tour SelectedTour { get; set; }
        GuideKeyPointsCheck guideKeyPointsCheck { get; set; }
        int pomid { get; set; }


        public GuideFollowTourLive()
        {
            InitializeComponent();
            this.DataContext = this;

            TourService = new TourService();
            TourService.Subscribe(this);

            pomid = -1;
            Tours = new ObservableCollection<Tour>(TourService.GetTodayTours());

        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
             int startedTours = 0;
            foreach (var tour in Tours)
            {
                if (tour.IsStarted == true)
                {
                    startedTours++;
                    pomid = tour.Id;
                }

            }

            if (startedTours < 1)
            {
                if (SelectedTour != null )
                {
                    Tour pomTour = SelectedTour;
                    GuideKeyPointsCheck guideKeyPointsCheck = new GuideKeyPointsCheck(SelectedTour.Id);
                    pomid = SelectedTour.Id;
                    SelectedTour.IsStarted = true;

                    MessageBox.Show(SelectedTour.Name.ToString() + " is started!");
                    TourService.UpdateTour(SelectedTour);

                    guideKeyPointsCheck.ShowDialog();

                    TourService.UpdateTour(pomTour);

                    /*if (SelectedTour != null)
                        pomid = SelectedTour.Id;
                      else
                        pomid = -1;
                    */

                }
                else
                {
                    MessageBox.Show("In order to start the tour, you first need to select it!");
                }
            }
            else 
            {
                MessageBox.Show("You can start maximum 1 tour in same time!");
            }
        }

        public void Update()
        {  
            Tours.Clear();
            foreach(Tour t in TourService.GetTodayTours())
            {
                Tours.Add(t);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (SelectedTour != null && SelectedTour.IsStarted == true)
            {
                SelectedTour.IsStarted = false;
                MessageBox.Show(SelectedTour.Name.ToString() + " is ended!");
                TourService.UpdateTour(SelectedTour);
            }
            else
            {
                MessageBox.Show("In order to end the tour, you first need to select started tour!");
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            int pomid1 = -1;
                foreach (var t in Tours)
                {
                if (t.isStarted == true)
                    pomid1 = t.Id;
                else pomid = -1;
                }

            pomid = pomid1;

            Tour tour = TourService.GetById(pomid);
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
    }
}
