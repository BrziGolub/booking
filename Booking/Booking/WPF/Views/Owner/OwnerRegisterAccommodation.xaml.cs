using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using Booking.Model;
using Booking.Model.Images;
using Booking.Model.Enums;
using System.IO;
using System.Collections.ObjectModel;
using Booking.Service;
using Booking.Domain.ServiceInterfaces;
using Booking.Util;

namespace Booking.View
{
    /// <summary>
    /// Interaction logic for OwnerRegisterAccommodation.xaml
    /// </summary>
    public partial class OwnerRegisterAccommodation : Window
    {

        public IAccommodationService AccommodationService { get; set; }
        public ILocationService LocationService { get; set; }
        public IAccommodationImageService accommodationImageService { get; set; }

        public Accommodation accommodation = new Accommodation();
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<string> CityCollection { get; set; }
        List<string> imagePaths = new List<string>();
        int currentImageIndex = -1;

        public void FillCity(object sender, SelectionChangedEventArgs e)
        {
            CityCollection.Clear();

            var locations = LocationService.GetAll().Where(l => l.State.Equals(Country));

            foreach (Location c in locations)
            {
                CityCollection.Add(c.City);
            }

            CitycomboBox.IsEnabled = true;

        }
        public OwnerRegisterAccommodation()
        {
            InitializeComponent();
            TypecomboBox.ItemsSource = new List<string>() { "APARTMENT", "HOUSE", "COTTAGE" };
            this.DataContext = this;
            AccommodationService = InjectorService.CreateInstance<IAccommodationService>();
            CityCollection = new ObservableCollection<string>();
            LocationService = InjectorService.CreateInstance<ILocationService>();
            accommodationImageService = InjectorService.CreateInstance<IAccommodationImageService>();


            FillComboBox();
            NextPreviousPhotoButtonsVisibility();
        }
        private void NextPreviousPhotoButtonsVisibility()
        {
            if (imagePaths.Count == 0)
            {
                imageSlideshow.Source = null;
                buttonNext.Visibility = Visibility.Collapsed;
                buttonPrevious.Visibility = Visibility.Collapsed;
            }
            else
            {
                buttonNext.Visibility = Visibility.Visible;
                buttonPrevious.Visibility = Visibility.Visible;
            }
        }

        public void FillComboBox()
        {
            List<string> items = new List<string>();

            using (StreamReader reader = new StreamReader("../../Resources/Data/locations.csv"))
            {
                while (!reader.EndOfStream)
                {

                    string[] fields = reader.ReadLine().Split(',');
                    foreach (var field in fields)
                    {
                        string[] Countries = field.Split('|');
                        items.Add(Countries[1]);
                    }
                }
            }
            var distinctItems = items.Distinct().ToList();

            CountrycomboBox.ItemsSource = distinctItems;

            if (CountrycomboBox.SelectedItem == null)
            {
                CitycomboBox.IsEnabled = false;
            }

        }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string _name;

        public string AccommodationName
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
                    OnPropertyChanged("Country");
                }
            }
        }



        public AccommodationType _type;
        public AccommodationType Type
        {
            get => _type;
            set
            {
                if (_type != value)
                {
                    _type = value;
                    OnPropertyChanged();
                }
            }
        }

        public int _capacity;
        public int Capacity
        {
            get => _capacity;
            set
            {
                if (_capacity != value)
                {
                    _capacity = value;
                    OnPropertyChanged();
                }
            }
        }

        public int _minNumberOfDays;
        public int MinNumberOfDays
        {
            get => _minNumberOfDays;
            set
            {
                if (_minNumberOfDays != value)
                {
                    _minNumberOfDays = value;
                    OnPropertyChanged();
                }
            }
        }

        public int _cancelationPeriod;
        public int CancelationPeriod
        {
            get => _cancelationPeriod;
            set
            {
                if (_cancelationPeriod != value)
                {
                    _cancelationPeriod = value;
                    OnPropertyChanged();
                }
            }
        }

        public List<AccommodationImage> _pictures;
        public List<AccommodationImage> Pictures
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

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void CreateAccommodation(object sender, RoutedEventArgs e)
        {
            accommodation.Name = AccommodationName;

            Location location = new Location
            {
                State = Country,
                City = City
            };

            int LocationID = LocationService.GetIdByCountryAndCity(Country, City);
            accommodation.Location.Id = LocationID;
            accommodation.Location = LocationService.GetById(LocationID);

            accommodation.Type = Type;
            accommodation.Capacity = Capacity;
            accommodation.MinNumberOfDays = MinNumberOfDays;
            accommodation.CancelationPeriod = CancelationPeriod;

            if (accommodation.Name == null)
            {
                MessageBox.Show("'NAME' not entered");
            }
            else if (accommodation.Location.City == null)
            {
                MessageBox.Show("'CITY' not entered");
            }
            else if (accommodation.Location.State == null)
            {
                MessageBox.Show("'COUNTRY' not entered");
            }
            else if (accommodation.Type.Equals(""))
            {
                MessageBox.Show("'TYPE' not entered");
            }
            else if (accommodation.Capacity == 0)
            {
                MessageBox.Show("'CAPACITY' not entered");
            }
            else if (accommodation.MinNumberOfDays == 0)
            {
                MessageBox.Show("'MIN NUMBER OF DAYS' not entered");
            }
            else if (accommodation.CancelationPeriod < 0)
            {
                MessageBox.Show("'CANCELATION PERIOD' should be greater or equal than 0");
            }
            else if (accommodation.Images.Count == 0)
            {
                MessageBox.Show("'PICTURES URL' not entered");
            }

            else
            {
                AccommodationService.AddAccommodation(accommodation);
                MessageBox.Show("Accommodation successfully created");
                this.Close();
            }
        }

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
                AccommodationImage Pictures = new AccommodationImage();
                Pictures.Url = tbPictures.Text;
                accommodation.Images.Add(Pictures);
                NextPreviousPhotoButtonsVisibility();
            }
            else 
            {
                MessageBox.Show("Photo url can not be empty");
            }

            tbPictures.Text = string.Empty;
        }
        private void RemovePicture(object sender, RoutedEventArgs e)
        {
            AccommodationImage Images = new AccommodationImage();
            Images.Url = tbPictures.Text;
            imageSlideshow.Source = null;
            imagePaths.Remove(Images.Url);
            accommodation.Images.Remove(Images);
            tbPictures.Text = "";
            NextPreviousPhotoButtonsVisibility();
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

        private void CountrycomboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
         
        }
    }
}