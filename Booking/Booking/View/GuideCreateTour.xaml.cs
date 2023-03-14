using Booking.Controller;
using Booking.Conversion;
using Booking.Model;
using Booking.Model.DAO;
using System;
using System.Collections.Generic;
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
        public TourController tourController;
        public LocationController locationController { get; set; }
        public Tour tour = new Tour();

        public event PropertyChangedEventHandler PropertyChanged;
        public GuideCreateTour()
        {
            InitializeComponent();
            this.DataContext = this;
            var app = Application.Current as App;
            tourController = app.TourController;

            locationController = app.LocationController;

            //**********************FILL COMBOBOX*****************************************
            
            List<string> items = new List<string>();

            using (StreamReader reader = new StreamReader("../../Resources/Data/locations.csv"))
            {
                while (!reader.EndOfStream)
                {
                    string[] fields = reader.ReadLine().Split(',');
                    items.Add(fields[0]); // assuming the first field contains the item name
                }
            }

            comboBox.ItemsSource = items;

            //*************************************************************************



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
            tour.Name = TourName;
            //---------LOCATION----------//

            Location location = new Location
            {
                State = Country,
                City = City
            };
            locationController.Create(location);
           
            tour.Location.Id = location.Id;
            tour.Location = location;       
            //-------------------------------

            tour.Description = Description;
            tour.Language = TourLanguage;
            tour.MaxGuestsNumber = MaxGuestNumber;
            tour.StartTime = DateConversion.StringToDate(StartTime);
            tour.Duration = Duration;
           
            //------------PROTECTIONS------------------
            if(tour.Name == null) 
            {
                MessageBox.Show("'NAME' not entered");
            }
            else if (tour.Location.City == null)
            {
                MessageBox.Show("'CITY' not entered");
            }
            else if (tour.Location.State == null)
            {
                MessageBox.Show("'COUNTRY' not entered");
            }
            else if (tour.Description == null)
            {
                MessageBox.Show("'DESCRIPTION' not entered");
            }
            else if (tour.Language == null)
            {
                MessageBox.Show("'LANGUAGE' not entered");
            }
            else if (tour.MaxGuestsNumber == 0)
            {
                MessageBox.Show("'MAX GUESTS NUMBER' not entered");
            }
            else if(tour.MaxGuestsNumber < 0)
            {
                MessageBox.Show("'MAX GUESTS NUMBER' should be greater than 0");
            }
            else if (tour.Destinations == null)
            {
                MessageBox.Show("'KEY POINTS' not entered");
            }
            else if (tour.StartTime == null)
            {
                MessageBox.Show("'TOUR START DATE' not entered");
            }
            else if (tour.Duration == 0)
            {
                MessageBox.Show("'DURATION' not entered");
            }
            else if (tour.Duration < 0)
            {
                MessageBox.Show("'DURATION' should be greater than 0");
            }
            else if (tour.Pictures == null)
            {
                MessageBox.Show("'PICTURES URL' not entered");
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
                LocationDAO locationDAO = new LocationDAO();

                //izvlacim samo prvi deo iz stringa comboboxa tj. izvlacim ID(sve pre prve '|')
                string pom = comboBox.SelectedItem.ToString();
                string[] substring = pom.Split('|');
                string sid = substring[0];
                int id = Convert.ToInt32(sid);
                //trazim tu lokaciju od koje je id
                Location location = locationDAO.FindById(id);
                //i samo lokaciju koju sam gore nasao dodam u listu destinationsa
                tour.Destinations.Add(location);
  
            }

            comboBox.SelectedIndex = -1;

        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (tbPictures.Text != null)
            {
                string space = " ";
                string input = tbPictures.Text.ToString();
                tour.Pictures.Add(input);
                tour.Pictures.Add(space);
            }

            tbPictures.Text = string.Empty;
        }
    }
}
