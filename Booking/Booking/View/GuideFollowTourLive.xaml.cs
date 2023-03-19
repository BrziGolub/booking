using Booking.Controller;
using Booking.Model;
using Booking.Observer;
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
    /// <summary>
    /// Interaction logic for GuideFollowTourLive.xaml
    /// </summary>
    public partial class GuideFollowTourLive : Window , IObserver
    {
        public ObservableCollection<Tour> Tours { get; set; }
        public TourController _tourController { get; set; }
        public Tour SelectedTour { get; set; }
        
        

        public GuideFollowTourLive()
        {
            InitializeComponent();
            this.DataContext = this;

            var app = Application.Current as App;

            _tourController = app.TourController;
            _tourController.Subscribe(this);

            Tours = new ObservableCollection<Tour>(_tourController.GetTodayTours());



        }

        

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int startedTours = 0;
            foreach (var tour in Tours)
            {
                if (tour.IsStarted == true)
                { startedTours++; }
            }

            if (startedTours < 1)
            {
                if (SelectedTour != null )
                {
                    SelectedTour.IsStarted = true;
                    MessageBox.Show(SelectedTour.Name.ToString() + " is started!");
                    _tourController.UpdateTour(SelectedTour);

                    GuideKeyPointsCheck GuideKeyPointsCheck = new GuideKeyPointsCheck();
                    GuideKeyPointsCheck.ShowDialog();
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
            foreach(Tour t in _tourController.GetTodayTours())
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
                _tourController.UpdateTour(SelectedTour);
            }
            else
            {
                MessageBox.Show("In order to end the tour, you first need to select started tour!");
            }
        }
    }
}
