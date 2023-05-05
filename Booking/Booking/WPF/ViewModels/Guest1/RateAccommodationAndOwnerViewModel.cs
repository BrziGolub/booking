using Booking.Domain.Model.Images;
using Booking.Domain.ServiceInterfaces;
using Booking.Model;
using Booking.Service;
using Booking.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using System.Windows;
using System.Collections.ObjectModel;
using Booking.Commands;
using Notifications.Wpf;
using System.Text.RegularExpressions;
using Booking.Model.Images;
using System.Windows.Media.Imaging;

namespace Booking.WPF.ViewModels.Guest1
{
    public class RateAccommodationAndOwnerViewModel: INotifyPropertyChanged, IDataErrorInfo
    {
       
        public event PropertyChangedEventHandler PropertyChanged;
        public String AccommodationLabel { get; set; } = String.Empty;
        public String OwnerLabel { get; set; } = String.Empty;

        public ObservableCollection<int> comboBoxCourtesy { get; set; }
        public ObservableCollection<int> comboBoxCleaness { get; set; }


        public AccommodationAndOwnerGrade accommodationAndOwnerGrade;
        public IAccommodationAndOwnerGradeService AccommodationAndOwnerGradeService { get; set; }
        public IGuestsAccommodationImagesService GuestsAccommodationImagesService { get; set; }
        public AccommodationReservation AccommodationReservation { get; set; }

        public NavigationService NavigationService { get; set; }

     

        private int _cleaness;
        public int Cleaness
        {
            get => _cleaness;
            set
            {
                if (_cleaness != value)
                {
                    _cleaness = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _courtesy;
        public int Courtesy
        {
            get => _courtesy;
            set
            {
                if (_courtesy != value)
                {
                    _courtesy = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _comment;
        public string Comment
        {
            get => _comment;
            set
            {
                if (_comment != value)
                {
                    _comment = value;
                    OnPropertyChanged();
                }
            }
    


      


        List<string> imagePaths = new List<string>();
        int currentImageIndex = -1;
        public RelayCommand Button_Click_Subbmit { get; set; }
        public RelayCommand Button_Click_Plus { get; set; }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                if (columnName.Equals("Comment") )
                {
                    if (Comment == String.Empty)
                    {
                        return "This filed is required!";
                    }
                }

                if (columnName.Equals("Courtesy"))
                {
                    if(Courtesy == 0)
                    {
                        return "This filed is required!";
                    }
                }

                if (columnName.Equals("Cleaness"))
                {
                    if(Cleaness == 0)
                    {
                        return "This filed is required!";
                    }
                }
                return null;
            }
        }

        private readonly string[] _validatedProperties = { "Comment", "Courtesy", "Cleaness" };

        public bool IsValid
        {
            get
            {
                foreach (var property in _validatedProperties)
                {
                    if (this[property] != null)
                        return false;
                }

                return true;
            }
        }



        public RateAccommodationAndOwnerViewModel(AccommodationReservation accommodationReservation, NavigationService navigationService)
        {
            AccommodationLabel = setAccommodationLabel(accommodationReservation);
            OwnerLabel = setOwnerLabel(accommodationReservation);
            AccommodationReservation = new AccommodationReservation();

            GuestsAccommodationImagesService = InjectorService.CreateInstance<IGuestsAccommodationImagesService>();

            AccommodationReservation = accommodationReservation;
            accommodationAndOwnerGrade = new AccommodationAndOwnerGrade();
            AccommodationAndOwnerGradeService = InjectorService.CreateInstance<IAccommodationAndOwnerGradeService>();
            accommodationAndOwnerGrade.Id = AccommodationAndOwnerGradeService.NextId();

            NavigationService = navigationService;
            Button_Click_Subbmit = new RelayCommand(ButtonSubbmit);
            //Button_Click_Plus = new RelayCommand(ButtonPlus);
            Comment = String.Empty;
            Courtesy = 0;
            Cleaness = 0;
            setComboBoxes();
           
        }

        public void setComboBoxes()
        {
            List<int> grades = new List<int>(){ 1, 2, 3, 4, 5 };
            comboBoxCleaness = new ObservableCollection<int>(grades);
            comboBoxCourtesy = new ObservableCollection<int>(grades);
        }


        private String setAccommodationLabel(AccommodationReservation accommodationReservation)
        {
            return accommodationReservation.Accommodation.Name + "-" + accommodationReservation.Accommodation.Location.State + "-" + accommodationReservation.Accommodation.Location.City + "-" + accommodationReservation.Accommodation.Type;

        }

        private String setOwnerLabel(AccommodationReservation accommodationReservation)
        {
            return accommodationReservation.Accommodation.Owner.Username;
        }

        private void MakeGrade()
        {
            accommodationAndOwnerGrade.Cleaness = Cleaness;
            accommodationAndOwnerGrade.OwnersCourtesy = Courtesy;
            accommodationAndOwnerGrade.Comment = Comment;
            accommodationAndOwnerGrade.AccommodationReservation = AccommodationReservation;
        }

        private void ButtonSubbmit(object param)
        {
           /* if(IsValid == false){
                //obavetsenje
                return;
            }*/
            MakeGrade();

            AccommodationAndOwnerGradeService.SaveGrade(accommodationAndOwnerGrade);
            this.NavigationService.GoBack();
            AccommodationAndOwnerGradeService.CheckSuper(AccommodationReservation); //ovo je od Coe 

            var notificationManager = new NotificationManager();
            NotificationContent content = new NotificationContent { Title = "Congratulations!", Message = "You succesfuly rate your accommodation", Type = NotificationType.Success};
            notificationManager.Show(content, areaName: "WindowArea", expirationTime: TimeSpan.FromSeconds(5));
        }


        /*  private void MakePicture(GuestsAccommodationImages Picture)
          {
              Picture.Url = Pictures;
              Picture.Guest.Id = AccommodationReservationService.SignedFirstGuestId;
              Picture.Grade.Id = accommodationAndOwnerGrade.Id;
              accommodationAndOwnerGrade.Images.Add(Picture);
          }

          private void ButtonPlus(object param)
          {
              GuestsAccommodationImages Picture = new GuestsAccommodationImages();

              MakePicture(Picture);
              GuestsAccommodationImagesService.Create(Picture);

              Pictures = string.Empty;
          }*/

     

      



    }
}
