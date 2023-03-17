﻿using Booking.Controller;
using Booking.Conversion;
using Booking.Model;
using Booking.Model.DAO;
using Booking.Model.Images;
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

namespace Booking.View
{
    /// <summary>
    /// Interaction logic for GuideCreateTour.xaml
    /// </summary>
    public partial class GuideCreateTour : Window
    {
        public TourController tourController { get; set; }
        public LocationController locationController { get; set; }
        public Tour tour = new Tour();
        public ObservableCollection<string> CityCollection { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        LocationDAO locationDAO = new LocationDAO();
        public GuideCreateTour()
        {
            InitializeComponent();
            this.DataContext = this;
            var app = Application.Current as App;
            tourController = app.TourController;

            locationController = app.LocationController;

            //**********************FILL COMBOBOX COUNTRY***************************************
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

            //**********************FILL COMBOBOX CITY*************************************

            CityCollection = new ObservableCollection<string>();

            //**********************FILL COMBOBOX KEYPOINTS*************************************

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

            //*************************************************************************

            if(comboBox1.SelectedItem == null)
            {
                comboBox2.IsEnabled = false;
            }

                


        }

        public void FillCity(object sender, SelectionChangedEventArgs e)
        {
            CityCollection.Clear();

            var locations = locationController.findAllLocations().Where(l => l.State.Equals(Country));

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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (tbName.Text == "")
            {
                MessageBox.Show("'NAME' not entered");
            }
            else { tour.Name = TourName; }
            //---------LOCATION----------//

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
                int LocationID = locationController.FindIdByCountryAndCity(Country, City);
                tour.Location.Id = LocationID;
                tour.Location = locationController.FindById(LocationID);
            }
            //-------------------------------
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
            { tour.StartTime = DateConversion.StringToDate(StartTime); }

            if (tbDuration.Text == "")
            { 
               MessageBox.Show("'DURATION' not entered");
            }
            else { tour.Duration = Duration; }


            //------------EXTRA PROTECTIONS---------------
            if (tour.Destinations.Count < 2)
            {
                MessageBox.Show("Tour need to have 2 'KEY POINTS' at least");
            }
            else if(tour.Images.Count == 0) // maybe not?
            {
                MessageBox.Show("Tour need to have 1 'PICTURE' at least");
            }
            else if (comboBox1.Text == "") //tour.Location.State == null
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
            //------------------------------------------

            else
            {
                tourController.Create(tour);
                MessageBox.Show("Tour successfully created");
                this.Close();
            }

            
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

            if (comboBox.SelectedItem != null)
            {
                string pom = comboBox.SelectedItem.ToString();
                string[] CountryCity = pom.Split(',');
                string Country = CountryCity[0];
                string City = CountryCity[1].Trim(); 

                int locationId= locationController.FindIdByCountryAndCity(Country, City);        
                Location location = locationController.FindById(locationId);

                TourKeyPoints tourKeyPoints = new TourKeyPoints();
                tourKeyPoints.Location = location;
                tour.Destinations.Add(tourKeyPoints);
            }
            comboBox.SelectedIndex = -1;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
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

            tbPictures.Text = string.Empty;
        }
    }
}
