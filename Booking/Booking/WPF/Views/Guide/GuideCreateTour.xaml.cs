using Booking.Domain.ServiceInterfaces;
using Booking.Model;
using Booking.Model.Images;
using Booking.Service;
using Booking.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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
using Microsoft.Win32;
using Booking.Application.UseCases;
using Booking.Observer;

namespace Booking.View
{
    public partial class GuideCreateTour : Window ,IObserver
    {
        public ITourService tourService { get; set; }
        public ILocationService locationService { get; set; }
        public ITourImageService tourImageService { get; set; }
        TourKeyPoint SelectedTourKeyPoint { get; set; }

        public Tour tour = new Tour();
        public ObservableCollection<string> CityCollection { get; set; }
        public ObservableCollection<TourKeyPoint> tourKeyPoints1 { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public GuideCreateTour()
        {
            InitializeComponent();
            this.DataContext = this;
            tourService = InjectorService.CreateInstance<ITourService>();
            locationService = InjectorService.CreateInstance<ILocationService>();
            tourImageService = InjectorService.CreateInstance<ITourImageService>();

            tourKeyPoints1 = new ObservableCollection<TourKeyPoint>(tourService.GetTourKeyPoints());

            FillComboBoxes();
        }

        private void FillComboBoxes()
        {
            FillLocationsComboBox();

            CityCollection = new ObservableCollection<string>();
            FillKeyPointsComboBox();

            if (comboBox1.SelectedItem == null)
            {
                comboBox2.IsEnabled = false;
            }
        }

        private void FillKeyPointsComboBox()
        {
            List<string> items = new List<string>();

            using (StreamReader reader = new StreamReader("../../Resources/Data/locations.csv"))
            {
                while (!reader.EndOfStream)
                {
                    string[] fields = reader.ReadLine().Split(',');
                    foreach (var field in fields)
                    {
                        string[] pom = field.Split('|');
                        items.Add(pom[1] + ", " + pom[2]);
                    }
                }
            }

            comboBox.ItemsSource = items;
        }

        private void FillLocationsComboBox()
        {
            List<string> items1 = new List<string>();

            using (StreamReader reader = new StreamReader("../../Resources/Data/locations.csv"))
            {
                while (!reader.EndOfStream)
                {

                    string[] fields = reader.ReadLine().Split(',');
                    foreach (var field in fields)
                    {
                        string[] Countries = field.Split('|');
                        items1.Add(Countries[1]);
                    }
                }
            }
            var distinctItems = items1.Distinct().ToList();
            comboBox1.ItemsSource = distinctItems;
        }

        public void FillCity(object sender, SelectionChangedEventArgs e)
        {
            CityCollection.Clear();

            var locations = locationService.GetAll().Where(l => l.State.Equals(Country));

            foreach (Location c in locations)
            {
                CityCollection.Add(c.City);
            }
            comboBox2.IsEnabled = true;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string _name;

        public string TourName
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }

        public string _city;
        public string City
        {
            get => _city;
            set
            {
                if (_city != value)
                {
                    _city = value;
                    OnPropertyChanged();
                }
            }
        }

        public string _country;
        public string Country
        {
            get => _country;
            set
            {
                if (_country != value)
                {
                    _country = value;
                    OnPropertyChanged();
                }
            }
        }

        public string _description;
        public string Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged();
                }
            }
        }

        public string _language;
        public string TourLanguage
        {
            get => _language;
            set
            {
                if (_language != value)
                {
                    _language = value;
                    OnPropertyChanged();
                }
            }
        }

        public int _maxguestnumber;
        public int MaxGuestNumber
        {
            get => _maxguestnumber;
            set
            {
                if (_maxguestnumber != value)
                {
                    _maxguestnumber = value;
                    OnPropertyChanged();
                }
            }
        }

        public List<Location> _destinations;
        public List<Location> Destinations
        {
            get => _destinations;
            set
            {
                if (_destinations != value)
                {
                    _destinations = value;
                    OnPropertyChanged();
                }
            }
        }


        public string _startTime;
        public string StartTime
        {
            get => _startTime;
            set
            {
                if (_startTime != value)
                {
                    _startTime = value;
                    OnPropertyChanged();
                }
            }
        }

        public int _duration;
        public int Duration
        {
            get => _duration;
            set
            {
                if (_duration != value)
                {
                    _duration = value;
                    OnPropertyChanged();
                }
            }
        }

        public List<string> _pictures;
        public List<string> Pictures
        {
            get => _pictures;
            set
            {
                if (_pictures != value)
                {
                    _pictures = value;
                    OnPropertyChanged();
                }
            }
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Create(object sender, RoutedEventArgs e)
        {
            if (tbName.Text == "")
            {
                MessageBox.Show("'NAME' not entered");
            }
            else { tour.Name = TourName; }

            Location location = new Location
            {
                State = Country,
                City = City
            };

            if (comboBox1.Text == "" || comboBox2.Text == "")
            {
                MessageBox.Show("'COUNTRY' AND 'CITY' should be entered");
            }
            else
            {
                int LocationID = locationService.GetIdByCountryAndCity(Country, City);
                tour.Location.Id = LocationID;
                tour.Location = locationService.GetById(LocationID);
            }

            if (tbDescription.Text == "")
            {
                MessageBox.Show("'DESCRIPTION' not entered");
            }
            else
            { tour.Description = Description; }

            if (tbLanguage.Text == "")
            { MessageBox.Show("'LANGUAGE' not entered"); }
            else
            { tour.Language = TourLanguage; }

            if (tbMaxGuests.Text == "")
            { MessageBox.Show("'MAX GUESTS NUMBER' not entered"); }
            else
            { tour.MaxVisitors = MaxGuestNumber; }

            if (datePicker.Text == "")
            { MessageBox.Show("'TOUR START DATE' not entered"); }
            else
            { tour.StartTime = DateTime.Parse(StartTime); }

            if (tbDuration.Text == "")
            {
                MessageBox.Show("'DURATION' not entered");
            }
            else { tour.Duration = Duration; }

            CheckProtections();

        }

        private void CheckProtections()
        {
            if (tour.Destinations.Count < 2)
            {
                MessageBox.Show("Tour need to have 2 'KEY POINTS' at least");
            }
            else if (tour.Images.Count == 0)
            {
                MessageBox.Show("Tour need to have 1 'PICTURE' at least");
            }
            else if (comboBox1.Text == "")
            {
                MessageBox.Show("'COUNTRY' not entered");
            }
            else if (tour.Location.City == null)
            {
                MessageBox.Show("'CITY' not entered");
            }
            else if (tour.MaxVisitors < 0)
            {
                MessageBox.Show("'MAX GUESTS NUMBER' should be greater than 0");
            }
            else if (tour.Duration < 0)
            {
                MessageBox.Show("'DURATION' should be greater than 0");
            }

            else
            {
                tourService.Create(tour);
                MessageBox.Show("Tour successfully created");
                this.Close();
            }
        }

        private void AddKeyPoint(object sender, RoutedEventArgs e)
        {

            if (comboBox.SelectedItem != null)
            {
                string pom = comboBox.SelectedItem.ToString();
                string[] CountryCity = pom.Split(',');
                string Country = CountryCity[0];
                string City = CountryCity[1].Trim();

                int locationId = locationService.GetIdByCountryAndCity(Country, City);
                Location location = locationService.GetById(locationId);

                TourKeyPoint tourKeyPoints = new TourKeyPoint();
                tourKeyPoints1.Add(tourKeyPoints);
                tourKeyPoints.Location = location;
                tour.Destinations.Add(tourKeyPoints);
                
            }
            comboBox.SelectedIndex = -1;
        }


        List<string> imagePaths = new List<string>();
        int currentImageIndex = -1;
        private void AddPicture(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog dialog = new System.Windows.Forms.OpenFileDialog();
            dialog.Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png";

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string imagePath = dialog.FileName;
                imagePaths.Add(imagePath);
                currentImageIndex = imagePaths.Count - 1;
                imageSlideshow.Source = new BitmapImage(new Uri(imagePath));
                tbPictures.Text = imagePath;
            }

            if (tbPictures.Text != "")
            {
                TourImage Images = new TourImage();
                Images.Url = tbPictures.Text;
                tour.Images.Add(Images);
            }
            else
            {
                MessageBox.Show("Photo URL can't be empty!");
            }

        }

        private void RemovePicture(object sender, RoutedEventArgs e)
        {
            TourImage Images = new TourImage();
            Images.Url = tbPictures.Text;
            imageSlideshow.Source = null;
            imagePaths.Remove(Images.Url);
            tour.Images.Remove(Images);
            tbPictures.Text = "";
        }



        private void buttonPrevious_Click(object sender, RoutedEventArgs e)
        {
            if (currentImageIndex > 0)
            {
                currentImageIndex--;
                string imagePath = imagePaths[currentImageIndex];
                imageSlideshow.Source = new BitmapImage(new Uri(imagePath));
                tbPictures.Text = imagePath;
            }
        }

        private void buttonNext_Click(object sender, RoutedEventArgs e)
        {
            if (currentImageIndex < imagePaths.Count - 1)
            {
                currentImageIndex++;
                string imagePath = imagePaths[currentImageIndex];
                imageSlideshow.Source = new BitmapImage(new Uri(imagePath));
                tbPictures.Text = imagePath;
            }
        }

        private void IncreaseMaxGuests(object sender, RoutedEventArgs e)
        {
            int currentValue;
            if (int.TryParse(tbMaxGuests.Text, out currentValue))
            {
                tbMaxGuests.Text = (currentValue + 10).ToString();
            }
        }

        private void DecreaseMaxGuests(object sender, RoutedEventArgs e)
        {
            int currentValue;
            if (int.TryParse(tbMaxGuests.Text, out currentValue) && currentValue > 0)
            {
                tbMaxGuests.Text = (currentValue - 10).ToString();
            }
            if(currentValue<9)
            {
                tbMaxGuests.Text = "0";
            }
        }

        private void IncreaseDuration(object sender, RoutedEventArgs e)
        {
            int currentValue;
            if (int.TryParse(tbDuration.Text, out currentValue))
            {
                tbDuration.Text = (currentValue + 1).ToString();
            }
        }
        private void DecreaseDuration(object sender, RoutedEventArgs e)
        {
            int currentValue;
            if (int.TryParse(tbDuration.Text, out currentValue) && currentValue > 0)
            {
                tbDuration.Text = (currentValue - 1).ToString();
            }
        }

        public void Update()
        {
            tourKeyPoints1.Clear();

            foreach (TourKeyPoint t in tourService.GetTourKeyPoints())
            {
                tourKeyPoints1.Add(t);
            }
        }

        private void RemoveKeyPoint(object sender, RoutedEventArgs e)
        {
            tourKeyPoints1.Remove(SelectedTourKeyPoint);
            Update();
        }
    }
}
