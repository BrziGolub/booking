using Booking.Commands;
using Booking.Domain.Model;
using Booking.Domain.ServiceInterfaces;
using Booking.Model;
using Booking.Service;
using Booking.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Booking.WPF.ViewModels.Guest2
{
	public class CreateTourComplexRequestViewModel : INotifyPropertyChanged
	{
		private readonly Window _window;
		private ILocationService _locationService;
		private ITourRequestService _tourRequestService;
		private ITourComplexRequestService _tourComplexRequestService;

		public event PropertyChangedEventHandler PropertyChanged;

		public ObservableCollection<TourRequest> TourRequests { get; set; }

		public TourRequest SelectedRequest { get; set; }

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
		public RelayCommand Button_Click_AddRequest { get; set; }
		public RelayCommand Button_Click_RemoveRequest { get; set; }

		public CreateTourComplexRequestViewModel(Window window)
		{
			_window = window;
			TourRequests = new ObservableCollection<TourRequest>();
			NumberOfPassengers = 1;

			_locationService = InjectorService.CreateInstance<ILocationService>();
			_tourRequestService = InjectorService.CreateInstance<ITourRequestService>();
			_tourComplexRequestService = InjectorService.CreateInstance<ITourComplexRequestService>();

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
			Button_Click_AddRequest = new RelayCommand(AddRequest);
			Button_Click_RemoveRequest = new RelayCommand(RemoveRequest);
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
			if (TourRequests.Count > 0)
			{
				TourComplexRequest tourComplexRequest = new TourComplexRequest();

				tourComplexRequest.User.Id = TourService.SignedGuideId;
				tourComplexRequest.CreatedDate = DateTime.Now;

				foreach (var v in TourRequests)
				{
					tourComplexRequest.Requests.Add(_tourRequestService.AddTourRequest(v));
				}

				_tourComplexRequestService.AddTourRequest(tourComplexRequest);
				CloseWindow();
			}
			else
			{
				MessageBox.Show("You need to add at least one tour!");
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

		private void AddRequest(object param)
		{
			if (true)//uslov za validaciju
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
				tourRequest.TourReservedStartTime = DateTime.Today;

				TourRequests.Add(tourRequest);
			}
			else
			{
				//poruka greske
			}
		}

		private void RemoveRequest(object param)
		{
			if (SelectedRequest != null)
			{
				TourRequests.Remove(SelectedRequest);
			}
			else
			{
				MessageBox.Show("No selected item for removal!");
			}
		}

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
