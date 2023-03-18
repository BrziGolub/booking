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
    /// Interaction logic for GuideKeyPointsCheck.xaml
    /// </summary>
    public partial class GuideKeyPointsCheck : Window, IObserver
    {
       // public ObservableCollection<Location> Locations { get; set; }

        public ObservableCollection<TourKeyPoints> KeyPoints{ get; set; }
        //public LocationController _locationController { get; set; }
        
        public TourController _tourController { get; set; }
        public TourKeyPoints SelectedTourKeyPoint { get; set; }
        
       // public Tour SelectedTour { get; set; }

        public GuideKeyPointsCheck(int idTour)
        {
            InitializeComponent();
            this.DataContext = this;

            var app = Application.Current as App;

           // _locationController = app.LocationController;
           // _locationController.Subscribe(this);

            _tourController = app.TourController;
            _tourController.Subscribe(this);

            //   SelectedTour = _tourController.GetTourById(idTour);

           // Locations = new ObservableCollection<Location>(_tourController.GetKeyPoints(SelectedTour.Id));
            KeyPoints = new ObservableCollection<TourKeyPoints>(_tourController.GetAllKeyPoints());
            
            foreach (var keyPoint in KeyPoints) 
            {
                MessageBox.Show(keyPoint.Location.City.ToString());
            }
            
        }

        public void Update()
        {
            KeyPoints.Clear();
            foreach(TourKeyPoints l in _tourController.GetAllKeyPoints())
            {
                KeyPoints.Add(l); 
            }
            
           /* Locations.Clear();
            foreach(Location l in _tourController.GetKeyPoints(SelectedTour.Id))
            {
            Locations.Add(l); 
            }*/

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            foreach (var keyPoint in KeyPoints)
            {
                MessageBox.Show(keyPoint.Location.City.ToString());
            }

        }
    }
}
