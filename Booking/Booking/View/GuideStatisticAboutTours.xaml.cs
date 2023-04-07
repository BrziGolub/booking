using Booking.Model;
using Booking.Observer;
using Booking.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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
    public partial class GuideStatisticAboutTours : Window, IObserver
    {
        public ObservableCollection<Tour> MostVisitedTourGenerraly { get; set; }
        public ObservableCollection<Tour> MostVisitedTourThisYear { get; set; }
        public TourService TourService { get; set; }
        public User user { get; set; }

        public GuideStatisticAboutTours()
        {
            
            InitializeComponent();
            this.DataContext = this;

            var app = Application.Current as App;

            TourService = app.TourService;
            TourService.Subscribe(this);

            buttonGenerraly.Focus();

            mostVisitedDataGrid.Items.Clear();
            MostVisitedTourGenerraly = new ObservableCollection<Tour>(TourService.GetMostVisitedTourGenerally()); 
           MostVisitedTourThisYear = new ObservableCollection<Tour>(TourService.GetTodayTours()); // GetMostVisitedTourThisYear()
        }


        public void Update()
        {
            MostVisitedTourGenerraly.Clear();
            foreach (Tour t in TourService.GetMostVisitedTourGenerally())
            {
                MostVisitedTourGenerraly.Add(t);
            }

            MostVisitedTourThisYear.Clear();
            foreach (Tour t in TourService.GetTodayTours()) // GetMostVisitedTourThisYear()
            {
                MostVisitedTourThisYear.Add(t);
            }
        }

        private void MostVisitedThisYear(object sender, RoutedEventArgs e)
        {
            buttonGenerraly.Background = Brushes.LightPink;
            buttonThisYear.Background = Brushes.LightGreen;

            mostVisitedDataGrid.ItemsSource = MostVisitedTourThisYear; // GetMostVisitedTourThisYear()

        }

        private void buttonGenerraly_Click(object sender, RoutedEventArgs e)
        {
            buttonGenerraly.Background = Brushes.LightGreen;
            buttonThisYear.Background = Brushes.LightPink;

            mostVisitedDataGrid.ItemsSource = MostVisitedTourGenerraly;
            
            
        }
    }
}
