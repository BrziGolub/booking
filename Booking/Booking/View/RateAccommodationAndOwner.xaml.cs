using Booking.Domain.ServiceInterfaces;
using Booking.Model;
using Booking.Model.Images;
using Booking.Repository;
using Booking.Service;
using Booking.Util;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Booking.View
{
    /// <summary>
    /// Interaction logic for RateAccommodationAndOwner.xaml
    /// </summary>
    public partial class RateAccommodationAndOwner : Page
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public String AccommodationLabel { get; set; } = String.Empty;
        public String OwnerLabel { get; set;} = String.Empty;

        public AccommodationAndOwnerGrade accommodationAndOwnerGrade;

        //public AccommodationAndOwnerGradeService AccommodationAndOwnerGradeService { get; set; }

        //public GuestsAccommodationImagesService GuestsAccommodationImagesService { get; set; }
        public IAccommodationAndOwnerGradeService AccommodationAndOwnerGradeService { get; set; }
        public IGuestsAccommodationImagesService GuestsAccommodationImagesService { get; set; }

        public AccommodationReservation AccommodationReservation { get; set; }

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
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public RateAccommodationAndOwner(AccommodationReservation accommodationReservation)
        {
            InitializeComponent();
            this.DataContext = this;
            AccommodationLabel = setAccommodationLabel(accommodationReservation);
            OwnerLabel = setOwnerLabel(accommodationReservation);
            AccommodationReservation = new AccommodationReservation();
            //GuestsAccommodationImagesService = new GuestsAccommodationImagesService();
            
            GuestsAccommodationImagesService = InjectorService.CreateInstance<IGuestsAccommodationImagesService>();

            AccommodationReservation = accommodationReservation;
            accommodationAndOwnerGrade = new AccommodationAndOwnerGrade();
            //AccommodationAndOwnerGradeService = new AccommodationAndOwnerGradeService(); //ovo povezi iz app
            AccommodationAndOwnerGradeService = InjectorService.CreateInstance<IAccommodationAndOwnerGradeService>();

            comboBoxCleaness.ItemsSource = new List<int>() { 1, 2, 3, 4, 5 };
            comboBoxCourtesy.ItemsSource = new List<int>() { 1, 2, 3, 4, 5 };
        }

        private String setAccommodationLabel(AccommodationReservation accommodationReservation)
        {
            return accommodationReservation.Accommodation.Name + "-" + accommodationReservation.Accommodation.Location.State + "-" + accommodationReservation.Accommodation.Location.City + "-" + accommodationReservation.Accommodation.Type;

        }

        private String setOwnerLabel(AccommodationReservation accommodationReservation)
        {
            return accommodationReservation.Accommodation.Owner.Username;
        }
    
        private void Button_Click_Subbmit(object sender, RoutedEventArgs e)
        {
            accommodationAndOwnerGrade.Cleaness = Cleaness;
            accommodationAndOwnerGrade.OwnersCourtesy = Courtesy;
            accommodationAndOwnerGrade.Comment = Comment;
            accommodationAndOwnerGrade.AccommodationReservation = AccommodationReservation;

            AccommodationAndOwnerGradeService.SaveGrade(accommodationAndOwnerGrade);

        }

        private void Button_Click_Plus(object sender, RoutedEventArgs e)
        {
          
            AccommodationImage Picture = new AccommodationImage();
            Picture.Url = tbPictures.Text;
            AccommodationReservation.Accommodation.Images.Add(Picture);

            Picture.Accomodation = AccommodationReservation.Accommodation;

            GuestsAccommodationImagesService.Create(Picture);

            tbPictures.Text = string.Empty;
        }
    }
}
