using Booking.Model;
using Booking.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
	public partial class SecondGuestHomePage : Window
	{
		private ObservableCollection<Tour> tours;
		private TourService _tourService;

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

			_tourService = new TourService();

			tours = new ObservableCollection<Tour>(_tourService.GetAll());

			TourDataGrid.ItemsSource = tours;

			SearchState = _tourService.GetAllStates();
			ComboBoxState.SelectedIndex = 0;
			SearchCity = new ObservableCollection<string>();
			ComboBoxCity.SelectedIndex = 0;

			SelectedState = "All";
			SelectedCity = "All";
		}

		private void ButtonSearch_Click(object sender, RoutedEventArgs e)
		{
			TourSearch(SelectedState, SelectedCity, SearchDuration, SearchLanguage, SearchVisitors);
		}

		public void ReserveTourSearch(string state, string city, int id)
		{
			TourSearch(state, city, "", "", "");
			tours.Remove(tours.Where(s => s.Id == id).Single());

			if(tours.Count() == 0)
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
			SearchCity = _tourService.GetAllCitiesByState(SearchCity, SelectedState);
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
    }
}
