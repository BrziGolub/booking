using Booking.Controller;
using Booking.Model;
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
		private TourController controller;

		public string SearchState { get; set; } = string.Empty;

		public string SearchCity { get; set; } = string.Empty;

		public string SearchDuration { get; set; } = string.Empty;

		public string SearchLanguage { get; set; } = string.Empty;

		public string SearchGuestNumber { get; set; } = string.Empty;

		public SecondGuestHomePage()
		{
			InitializeComponent();
			DataContext = this;

			controller = new TourController();

			tours = new ObservableCollection<Tour>(controller.GetAll());

			TourDataGrid.ItemsSource = tours;
		}

		private void buttonSearch_Click(object sender, RoutedEventArgs e)
		{
			controller.Search(tours, SearchState, SearchCity, SearchDuration, SearchLanguage, SearchGuestNumber);
		}

		private void ButtonCancelSearch_Click(object sender, RoutedEventArgs e)
		{
			controller.CancelSearch(tours);
		}
	}
}
