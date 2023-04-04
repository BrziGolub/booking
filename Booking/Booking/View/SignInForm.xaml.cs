using Booking.Model;
using Booking.Repository;
using Booking.Service;
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
			
			
		}
		private void SignIn(object sender, RoutedEventArgs e)
		{            
            User user = _repository.GetByUsername(Username);
            
			GuideHomePage.Username = usernameTextBox.Text;
            TourService.SignedGuideId = user.Id;

            if (user != null)
			{
				if (user.Password == txtPassword.Password)
				{

					if(user.Role == 1)
					{
						OwnerHomePage ownerHomePage = new OwnerHomePage();
						ownerHomePage.Show();
						Close();
					}
                    else if (user.Role == 2)
                    {
                        GuideHomePage guideHomePage = new GuideHomePage();
                        guideHomePage.Show();
                        Close();
                    }
                    else if(user.Role == 3)
					{
						FirstGuestHomePage fisrtGuestHomePage = new FirstGuestHomePage();
                        fisrtGuestHomePage.Show();
                    }
                    else if (user.Role == 4)
                    {
						SecondGuestHomePage secondGuestHomePage = new SecondGuestHomePage();
						secondGuestHomePage.Show();
						Close();
					}

                }
                else
				{
					MessageBox.Show("Wrong password!");
				}
			}
			else
			{
				MessageBox.Show("Wrong username!");
			}

		}

        private void passwordBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SignIn(sender, e);
            }
        }

        private void AboutUs(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Autors of the project: \n \tKristina Zelić RA4/2020 \n \tPetar Kovačević RA25/2020  \n \tAleksandar Milović RA67/2020 \n \tMiljan Lazić RA212/2020");
        }
    }
}
