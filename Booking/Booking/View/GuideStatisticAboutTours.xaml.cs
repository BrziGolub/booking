using Booking.Model;
using Booking.Observer;
using Booking.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
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
        public ObservableCollection<Tour> averageNumberOfGuests { get; set; }
        public TourService TourService { get; set; }
        public User user { get; set; }

        int zeroToEighteen = 0;

        public GuideStatisticAboutTours()
        {
            
            InitializeComponent();
            this.DataContext = this;

            var app = Application.Current as App;

            TourService = app.TourService;
            TourService.Subscribe(this);

            mostVisitedDataGrid.Items.Clear();
            MostVisitedTourGenerraly = new ObservableCollection<Tour>(TourService.GetMostVisitedTourGenerally()); 
            MostVisitedTourThisYear = new ObservableCollection<Tour>(TourService.GetMostVisitedTourThisYear());

            FillComboBoxes();

            

            
            
        }

        private void FillComboBoxes()
        {
            List<string> items1 = new List<string>();

            using (StreamReader reader = new StreamReader("../../Resources/Data/tours.csv"))
            {
                while (!reader.EndOfStream)
                {
                    string[] fields = reader.ReadLine().Split('|');
                    if (fields[9] == TourService.SignedGuideId.ToString())
                    {
                        items1.Add(fields[1]);
                    }
                }
            }
            var distinctItems = items1.Distinct().ToList();
            comboBoxTours.ItemsSource = distinctItems;

        }

        public void Update()
        {
            MostVisitedTourGenerraly.Clear();
            foreach (Tour t in TourService.GetMostVisitedTourGenerally())
            {
                MostVisitedTourGenerraly.Add(t);
            }

            MostVisitedTourThisYear.Clear();
            foreach (Tour t in TourService.GetMostVisitedTourThisYear()) 
            {
                MostVisitedTourThisYear.Add(t);
            }
        }

        private void MostVisitedThisYear(object sender, RoutedEventArgs e)
        {
            buttonGenerraly.Background = Brushes.LightPink;
            buttonThisYear.Background = Brushes.LightGreen;

            mostVisitedDataGrid.ItemsSource = MostVisitedTourThisYear; 

            if(MostVisitedTourThisYear.Count() == 0)
            {
                MessageBox.Show("You don't have tours with guests in this year!");
            }

        }

        private void buttonGenerraly_Click(object sender, RoutedEventArgs e)
        {
            buttonGenerraly.Background = Brushes.LightGreen;
            buttonThisYear.Background = Brushes.LightPink;

            mostVisitedDataGrid.ItemsSource = MostVisitedTourGenerraly;

            if (MostVisitedTourGenerraly.Count() == 0)
            {
                MessageBox.Show("You don't have tours with guests!");
            }

        }

        private void Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }



        private void ShowStatistic(object sender, RoutedEventArgs e)
        {
            if(comboBoxTours.Text != "") 
            {
                Tour turapom = TourService.GetByName(comboBoxTours.Text);

                zeroToEighteenTextBlock.Text = TourService.numberOfZeroToEighteenGuests(turapom.Id).ToString();
                EighteenToFifthyTextBlock.Text = TourService.numberOfEighteenToFiftyGuests(turapom.Id).ToString();
                FifthyPlusTextBlock.Text = TourService.numberOfFiftyPlusGuests(turapom.Id).ToString();
            }
            else
            {
                MessageBox.Show("In order to show statistics, you first need to select some tour!");
            }
        }

        private void ResetStatistic(object sender, RoutedEventArgs e)
        {
            comboBoxTours.SelectedIndex = -1;
            zeroToEighteenTextBlock.Text = "";
            EighteenToFifthyTextBlock.Text = "";
            FifthyPlusTextBlock.Text = "";
        }
    }
}
