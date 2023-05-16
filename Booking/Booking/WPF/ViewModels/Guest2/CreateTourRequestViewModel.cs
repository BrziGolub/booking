using Booking.Commands;
using Booking.Domain.Model;
using Booking.Domain.ServiceInterfaces;
using Booking.Model;
using Booking.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Booking.WPF.ViewModels.Guest2
{
	public class CreateTourRequestViewModel : INotifyPropertyChanged
	{
		private readonly Window _window;
		private ILocationService _locationService;
		private ITourRequestService _tourRequestService;

		public event PropertyChangedEventHandler PropertyChanged;

		public List<string> States { get; set; }
		public ObservableCollection<string> Cities { get; set; }

		public string SelectedState { get; set; }
		public string SelectedCity { get; set; }
		public string Language { get; set; } = string.Empty;

		private int _numberOfPassengers;
		public int NumberOfPassengers
		{
			get => _numberOfPassengers;
			set
			{
				if (_numberOfPassengers != value)
				{
					_numberOfPassengers = value;
					OnPropertyChanged();
				}
			}
		}

		private bool _isEnabled;
		public bool CityComboBoxIsEnabled
		{
			get => _isEnabled;
			set
			{
				if (_isEnabled != value)
				{
					_isEnabled = value;
					OnPropertyChanged();
				}
			}
		}
		private int _selectedIndex;
		public int CityComboBoxSelectedIndex
		{
			get => _selectedIndex;
			set
			{
				if (_selectedIndex != value)
				{
					_selectedIndex = value;
					OnPropertyChanged();
				}
			}
		}

		public DateTime StartDate { get; set; } = DateTime.Now;
		public DateTime EndDate { get; set; } = DateTime.Now;
		public string Description { get; set; } = string.Empty;

		public RelayCommand Button_Click_Close { get; set; }
		public RelayCommand Button_Click_Request { get; set; }
		public RelayCommand Selection_Changed { get; set; }
		public RelayCommand Button_Click_NumericUp { get; set; }
		public RelayCommand Button_Click_NumericDown { get; set; }

		public CreateTourRequestViewModel(Window window)
		{
			_window = window;
			NumberOfPassengers = 1;

			_locationService = InjectorService.CreateInstance<ILocationService>();
			_tourRequestService = InjectorService.CreateInstance<ITourRequestService>();

			States = _locationService.GetAllStates();
			Cities = new ObservableCollection<string>();

			SelectedState = "All";
			SelectedCity = "All";

			FillCityComboBox();
			InitializeCommands();
		}

		private void InitializeCommands()
		{
			Button_Click_Close = new RelayCommand(ButtonCancel);
			Button_Click_Request = new RelayCommand(ButtonRequest);
			Selection_Changed = new RelayCommand(ComboBoxStateSelectionChanged);
			Button_Click_NumericUp = new RelayCommand(NumericUp);
			Button_Click_NumericDown = new RelayCommand(NumericDown);
		}

		private void ComboBoxStateSelectionChanged(object param)
		{
			FillCityComboBox();
		}

		private void FillCityComboBox()
		{
			Cities = _locationService.GetAllCitiesByState(Cities, SelectedState);
			SelectedCity = "All";
			CityComboBoxSelectedIndex = 0;
			if (SelectedState.Equals("All"))
			{
				CityComboBoxIsEnabled = false;
			}
			else
			{
				CityComboBoxIsEnabled = true;
			}
		}

		private void ButtonCancel(object sender)
		{
			CloseWindow();
		}

		private void CloseWindow()
		{
			_window.Close();
		}

		private void ButtonRequest(object param)
		{
			if (true)
			{
				TourRequest tourRequest = new TourRequest();
				Location location = new Location();

				location.Id = _locationService.GetIdByCountryAndCity(SelectedState, SelectedCity);
				location.State = SelectedState;
				location.City = SelectedCity;

				tourRequest.CreatedDate = DateTime.Now;
				tourRequest.Location = location;
				tourRequest.Language = Language;
				tourRequest.GuestsNumber = NumberOfPassengers;
				tourRequest.StartTime = StartDate;
				tourRequest.EndTime = EndDate;
				tourRequest.Description = Description;

				_tourRequestService.AddTourRequest(tourRequest);
				CloseWindow();
			}
		}

		private void NumericUp(object param)
		{
			NumberOfPassengers++;
		}

		private void NumericDown(object param)
		{
			if (NumberOfPassengers > 1)
				NumberOfPassengers--;
		}

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
