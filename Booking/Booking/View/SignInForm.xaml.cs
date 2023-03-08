using Booking.Model;
using Booking.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
	/// Interaction logic for SignInForm.xaml
	/// </summary>
	public partial class SignInForm : Window
	{
		private readonly UserRepository _repository;

		private string _username;
		public string Username
		{
			get => _username;
			set
			{
				if (value != _username)
				{
					_username = value;
					OnPropertyChanged();
				}
			}
		}

		public string UserType { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public SignInForm()
		{
			InitializeComponent();
			DataContext = this;
			_repository = new UserRepository();

			comboBox.ItemsSource = new List<string>() { "Owner", "Guest1", "Guest2", "Guide" };
		}
		private void SignIn(object sender, RoutedEventArgs e)
		{
			User user = _repository.GetByUsername(Username);
			if (user != null)
			{
				if (user.Password == txtPassword.Password)
				{
					//MessageBox.Show(UserType);
					//stavio da uvek otvara vodica po defaultu treba napraviti zastitu da se zna ko se loguje (sef,gost,vodic)
					if (comboBox.SelectedIndex == 0)
					{
						//otvara prozor za Ownera
					}
					else if (comboBox.SelectedIndex == 1)
					{
						FirstGuestHomePage fisrtGuestHomePage = new FirstGuestHomePage();
                        fisrtGuestHomePage.Show();
                    }
					else if (comboBox.SelectedIndex == 2)
					{
						SecondGuestHomePage secondGuestHomePage = new SecondGuestHomePage();
						secondGuestHomePage.Show();
						Close();
					}
					else if (comboBox.SelectedIndex == 3)
					{
						GuideHomePage guideHomePage = new GuideHomePage();
						guideHomePage.Show();
						Close();
					}
					else 
					{
						MessageBox.Show("Choose option!");
					}
					

                }
                else
				{
					MessageBox.Show("Wrong password!");
				}
					/*
					 if( comboBox.SelectedIndex == 1 ) 
					{
					MessageBox.Show("SEF");
					}	
					else if (comboBox.SelectedIndex == 2)
					{
                    MessageBox.Show("GUEST1");
					}	
					*/
			}
			else
			{
				MessageBox.Show("Wrong username!");
			}

		}

	}
}
