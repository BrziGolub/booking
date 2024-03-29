﻿using Booking.Commands;
using Booking.Model;
using Booking.WPF.Views.Owner;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace Booking.WPF.ViewModels.Owner
{
    public class SchedulingRenovationsViewModel:INotifyPropertyChanged
    {
        public Accommodation selectedAccommodation { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public String AccommodationLabel { get; set; } = String.Empty;
        public RelayCommand Find { get; set; }
        public RelayCommand Close { get; set; }
        private readonly Window _window;
        public DateTime _startDay;
        public DateTime StartDay
        {
            get => _startDay;
            set
            {
                if (_startDay != value)
                {
                    _startDay = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime _endDay;
        public DateTime EndDay
        {
            get => _endDay;
            set
            {
                if (_endDay != value)
                {
                    _endDay = value;
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
        public SchedulingRenovationsViewModel(Accommodation SelectedAccommodation, Window window)
        {
            _window = window;
            selectedAccommodation = SelectedAccommodation;
            SetAccommodationLabel();
            Find = new RelayCommand(FindDate);
            Close = new RelayCommand(CloseWindow);
            StartDay = DateTime.Now;
            EndDay = DateTime.Now;
            Duration = 0;
        }
        public void SetAccommodationLabel()
        {
            AccommodationLabel = selectedAccommodation.Name + "-" + selectedAccommodation.Location.State + "-" + selectedAccommodation.Location.City;
        }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void CloseWindow(object param)
        {
            _window.Close();
        }
        private void FindDate(object param)
        {
            if (StartDay > EndDay)
            {
                MessageBox.Show("Starting day can not be after end day");
                return;
            }
            else if (Duration == 0)
            {
                MessageBox.Show("Choose a duration of the renovations");
                return;
            }
            else if (StartDay < DateTime.Now)
            {
                MessageBox.Show("Start date can not be a date that has passed");
                return;
            }
            else
            {
                OwnerSchedulingRenovations ownerSchedulingRenovations = new OwnerSchedulingRenovations(selectedAccommodation, StartDay, EndDay, Duration);
                ownerSchedulingRenovations.Show();
            }
        }

    }
}
