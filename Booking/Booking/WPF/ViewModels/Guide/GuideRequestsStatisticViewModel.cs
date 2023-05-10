using Booking.Commands;
using Booking.Model;
using Booking.Observer;
using Booking.Service;
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

namespace Booking.WPF.ViewModels.Guide
{
    public class GuideRequestsStatisticViewModel : IObserver, INotifyPropertyChanged
    {

        private bool _isLocationSelected;
        private bool _isLanguageSelected;
        private bool _isLocationEnabled;
        private bool _isLanguageEnabled;

        public bool IsLocationSelected
        {
            get { return _isLocationSelected; }
            set
            {
                _isLocationSelected = value;
                OnPropertyChanged(nameof(IsLocationSelected));
                OnPropertyChanged(nameof(IsLanguageSelected));
                UpdateControlsEnabledState();
            }
        }

        public bool IsLanguageSelected
        {
            get { return _isLanguageSelected; }
            set
            {
                _isLanguageSelected = value;
                OnPropertyChanged(nameof(IsLanguageSelected));
                OnPropertyChanged(nameof(IsLocationSelected));
                UpdateControlsEnabledState();
            }
        }

        public bool IsLocationEnabled
        {
            get { return _isLocationEnabled; }
            set
            {
                _isLocationEnabled = value;
                OnPropertyChanged(nameof(IsLocationEnabled));
            }
        }

        public bool IsLanguageEnabled
        {
            get { return _isLanguageEnabled; }
            set
            {
                _isLanguageEnabled = value;
                OnPropertyChanged(nameof(IsLanguageEnabled));
            }
        }

        private void UpdateControlsEnabledState()
        {
            IsLocationEnabled = IsLocationSelected;
            IsLanguageEnabled = IsLanguageSelected;
        }


        private string _selectedLocation;
        public string SelectedLocation
        {
            get { return _selectedLocation; }
            set
            {
                _selectedLocation = value;
                OnPropertyChanged("SelectedLocation");
            }
        }

        public ObservableCollection<string> LocationsCollection { get; set; }

        private void FillLocationsComboBox()
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

            LocationsCollection.Clear();
            foreach (string str in items)
            {
                LocationsCollection.Add(str);
            }
        }


        public RelayCommand Close { get; set; }
        public RelayCommand Show { get; set; }
        

        public event PropertyChangedEventHandler PropertyChanged;
        private readonly Window _window;
        public GuideRequestsStatisticViewModel(Window window)
        {
            _window = window;



            LocationsCollection = new ObservableCollection<string>();

            IsLocationEnabled = true;
            IsLocationSelected = true;
            FillLocationsComboBox();
            SetCommands();
        }

        public void SetCommands()
        {
            Close = new RelayCommand(ButtonClose);
            Show = new RelayCommand(ButtonShow);
        }

        private void ButtonClose(object param)
        {
            CloseWindow();
        }
        private void ButtonShow(object param)
        {
            MessageBox.Show("Show");
        }
        
        private void CloseWindow()
        {
            _window.Close();
        }

        public void Update()
        {

        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
