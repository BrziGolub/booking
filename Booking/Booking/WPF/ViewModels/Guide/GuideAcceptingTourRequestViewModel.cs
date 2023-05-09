using Booking.Application.UseCases;
using Booking.Commands;
using Booking.Domain.Model;
using Booking.Domain.ServiceInterfaces;
using Booking.Model;
using Booking.Observer;
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
using System.Windows.Input;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;

namespace Booking.WPF.ViewModels.Guide
{
    public class GuideAcceptingTourRequestViewModel : IObserver, INotifyPropertyChanged
    {

        public RelayCommand Close { get; set; }
        public RelayCommand Filter { get; set; }
        public RelayCommand Accept { get; set; }
        
        public ICommand FillCityCommand { get; set; }
        

        public ITourRequestService tourRequestService { get; set; }
        public ILocationService locationService { get; set; }
        public string SelectedCountry { get; set; } = string.Empty;
        public string SelectedCity { get; set; } = string.Empty;

        public string SelectedNumberOfGuests { get; set; } = string.Empty;
        public string SelectedLanguage { get; set;} = string.Empty;


        private TourRequest _selectedTourRequest;
        public TourRequest SelectedTourRequest
        {
            get { return _selectedTourRequest; }
            set
            {
                _selectedTourRequest = value;
                OnPropertyChanged("SelectedTourRequest");
            }
        }
        public ObservableCollection<TourRequest> TourRequests { get; set; }
        public ObservableCollection<string> CityCollection { get; set; }
        public ObservableCollection<string> CountryCollection { get; set; }

        private bool _cityComboBoxEnabled;
        public bool CityComboboxEnabled
        {
            get => _cityComboBoxEnabled;
            set
            {
                if (_cityComboBoxEnabled != value)
                {
                    _cityComboBoxEnabled = value;
                    OnPropertyChanged();
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private readonly Window _window;
        public GuideAcceptingTourRequestViewModel(Window window)
        {
            _window = window;

            tourRequestService = InjectorService.CreateInstance<ITourRequestService>();
            locationService = InjectorService.CreateInstance<ILocationService>();

            TourRequests = new ObservableCollection<TourRequest>(tourRequestService.GetAll());

            CityCollection = new ObservableCollection<string>();
            CountryCollection = new ObservableCollection<string>();


            FillComboBox();
            SetCommands();
        }

        public void FillCity(object param)
        {
            CityCollection.Clear();

            var locations = locationService.GetAll().Where(l => l.State.Equals(SelectedCountry));

            foreach (Location c in locations)
            {
                CityCollection.Add(c.City);
            }

            CityComboboxEnabled = true;
        }

        public void FillComboBox()
        {
            List<string> items = new List<string>() { "All" };

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

            UpdateCountryCollection(distinctItems);

            if (SelectedCountry == null )
            {
                CityComboboxEnabled = false;
            }
            /*if (SelectedCountry == "All")
            {
                CityComboboxEnabled = false;
            }*/ // TREBA DA NAPRAVIM 

        }

        public void UpdateCountryCollection(List<string> coutries)
        {
            CountryCollection.Clear();
            foreach (string str in coutries)
            {
                CountryCollection.Add(str);
            }
        }


        public void SetCommands()
        {
            Close = new RelayCommand(ButtonClose);
            Filter = new RelayCommand(ButtonFilter);
            Accept = new RelayCommand(ButtonAccept);

            FillCityCommand = new RelayCommand(FillCity);
        }
            private void ButtonClose(object param)
        {
            CloseWindow();
        }

        private void ButtonFilter(object param)
        {
            //MessageBox.Show("filter");
            TourRequests.Clear();

            tourRequestService.Search(TourRequests, SelectedCity, SelectedCountry, SelectedNumberOfGuests, SelectedLanguage);

        }

        private void ButtonAccept(object param)
        {
            MessageBox.Show("Accept");
        }
        private void CloseWindow()
        {
            _window.Close();
        }
        public void Update()
        {
            TourRequests.Clear();
            foreach(TourRequest request in tourRequestService.GetAll()) 
            {
            TourRequests.Add(request);
            }
        }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
