using Booking.Controller;
using Booking.Model;
using Booking.Model.DAO;
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

namespace Booking.View
{
    /// <summary>
    /// Interaction logic for GuideKeyPointsCheck.xaml
    /// </summary>
    public partial class GuideKeyPointsCheck : Window, IObserver
    {

        public ObservableCollection<TourKeyPoint> KeyPoints { get; set; }

        public TourService TourService { get; set; }
       // public TourController _tourController { get; set; }
        public TourKeyPoint SelectedTourKeyPoint { get; set; }
        public Tour SelectedTour { get; set; }

        int idt;

        public GuideKeyPointsCheck(int idTour)
        {
            InitializeComponent();
            this.DataContext = this;

			var app = Application.Current as App;

			TourService = app.TourService;
			TourService.Subscribe(this);
            
            SelectedTour = TourService.GetById(idTour);

            idt = idTour;

            

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
        }

        private void Button_Click(object sender, RoutedEventArgs e)
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
    }
}
