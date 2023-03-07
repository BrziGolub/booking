using System;
using System.Collections.Generic;
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
	/// <summary>
	/// Interaction logic for SecondGuestHomePage.xaml
	/// </summary>
	public partial class SecondGuestHomePage : Window
	{
		public SecondGuestHomePage()
		{
			InitializeComponent();
			DataContext = this;
		}

		private void buttonSearch_Click(object sender, RoutedEventArgs e)
		{
			SecondGuestSearch secondGuestSearch = new SecondGuestSearch();
			secondGuestSearch.Show();
		}
	}
}
