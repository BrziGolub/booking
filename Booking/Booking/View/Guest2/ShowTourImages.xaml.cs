using Booking.Model.Images;
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
	public partial class ShowTourImages : Window
	{
		public ShowTourImages(List<TourImage> tourImages)
		{
			InitializeComponent();
			DataContext = this;

			ListBoxImages.ItemsSource = tourImages;
		}

		private void ButtonClose(object sender, RoutedEventArgs e)
		{
			Close();
		}
	}
}
