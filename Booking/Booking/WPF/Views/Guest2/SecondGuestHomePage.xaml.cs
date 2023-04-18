using Booking.Domain.ServiceInterfaces;
using Booking.Model;
using Booking.Service;
using Booking.Util;
using Booking.View.Guest2;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
//using System.Windows.Forms;

namespace Booking.View
{
	public partial class SecondGuestHomePage : Window
	{
		private ObservableCollection<Tour> tours;
		private ObservableCollection<TourReservation> activeTour;

		private ITourService _tourService;
		private ILocationService _locationService;
		private ITourReservationService _tourReservationService;
		private ITourGuestsService _tourGuestsService;

		public List<string> SearchState { get; set; }
		public ObservableCollection<string> SearchCity { get; set; }

		public string SearchDuration { get; set; } = string.Empty;
		public string SearchLanguage { get; set; } = string.Empty;
		public string SearchVisitors { get; set; } = string.Empty;

		public Tour SelectedTour { get; set; }
		public string SelectedState { get; set; }
		public string SelectedCity { get; set; }

		public SecondGuestHomePage()
		{
			InitializeComponent();
			DataContext = this;

			_tourService = InjectorService.CreateInstance<ITourService>();
			_locationService = InjectorService.CreateInstance<ILocationService>();
			_tourReservationService = InjectorService.CreateInstance<ITourReservationService>();
			_tourGuestsService = InjectorService.CreateInstance<ITourGuestsService>();

			tours = new ObservableCollection<Tour>(_tourService.GetValidTours());
			activeTour = new ObservableCollection<TourReservation> { _tourReservationService.GetActiveTour(TourService.SignedGuideId) };

			TourDataGrid.ItemsSource = tours;
			ActiveTourDataGrid.ItemsSource = activeTour;

			SearchState = _locationService.GetAllStates();
			ComboBoxState.SelectedIndex = 0;
			SearchCity = new ObservableCollection<string>();
			ComboBoxCity.SelectedIndex = 0;

			SelectedState = "All";
			SelectedCity = "All";

			CheckPresence();
		}

		private void ButtonSearch_Click(object sender, RoutedEventArgs e)
		{
			TourSearch(SelectedState, SelectedCity, SearchDuration, SearchLanguage, SearchVisitors);
		}

		public void ReserveTourSearch(string state, string city, int id)
		{
			TourSearch(state, city, "", "", "");
			tours.Remove(tours.Where(s => s.Id == id).Single());

			if (tours.Count() == 0)
			{
				MessageBox.Show("All tours at same location are full!");
				ShowAll();
			}
		}

		public void TourSearch(string state, string city, string duration, string lang, string passengers)
		{
			tours = _tourService.Search(tours, state, city, duration, lang, passengers);
		}

		private void ButtonShowAll(object sender, RoutedEventArgs e)
		{
			ShowAll();
		}

		public void ShowAll()
		{
			tours = _tourService.CancelSearch(tours);
		}

		private void ReserveTour(object sender, RoutedEventArgs e)
		{
			ReserveTour view = new ReserveTour(SelectedTour);
			view.Owner = this;
			view.ShowDialog();
		}

		private void ComboBoxStateSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			SearchCity = _locationService.GetAllCitiesByState(SearchCity, SelectedState);
			ComboBoxCity.SelectedIndex = 0;
		}

		private void ButtonSignOff_Click(object sender, RoutedEventArgs e)
		{
			SignInForm signInForm = new SignInForm();
			signInForm.Show();
			Close();
		}

		private void ShowImages(object sender, RoutedEventArgs e)
		{
			ShowTourImages view = new ShowTourImages(SelectedTour.Images);
			view.ShowDialog();
		}

		private void ShowDestinations(object sender, RoutedEventArgs e)
		{
			ShowTourDestinations view = new ShowTourDestinations(SelectedTour.Destinations);
			view.ShowDialog();
		}

		private void ShowVouchers(object sender, RoutedEventArgs e)
		{
			ShowVouchers view = new ShowVouchers();
			view.ShowDialog();
		}

		private void CheckPresence()
		{
			TourGuests tourGuest = _tourGuestsService.CheckPresence(TourService.SignedGuideId);
			if (tourGuest != null && !tourGuest.IsPresent)
			{
                MessageBoxResult dialogResult = MessageBoxResponse(tourGuest.Tour.Name);
				if (dialogResult == MessageBoxResult.Yes)
				{
					tourGuest.IsPresent = true;
					_tourGuestsService.UpdateTourGuest(tourGuest);
				}
			}
		}

		private MessageBoxResult MessageBoxResponse(string name)
		{
			string sMessageBoxText = $"Are you present on tour \n{name}?";
			string sCaption = "Presence on tour";

			MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
			MessageBoxImage icnMessageBox = MessageBoxImage.Question;

			MessageBoxResult result = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);
			return result;
		}
	}
}
