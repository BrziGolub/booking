
using Booking.Commands;
using Booking.WPF.Views.Guest1;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace Booking.WPF.ViewModels.Guest1
{
    public class QuickSearchSuggestionsViewModel: INotifyPropertyChanged
    {

        public ObservableCollection<AccommodationDTO> DTOs { get; set; }
        public AccommodationDTO SelectedDTO { get; set; }
        public RelayCommand ViewAvailableDatesCommand { get; set; }

        public NavigationService nagivation;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


       public QuickSearchSuggestionsViewModel(List<AccommodationDTO> dtos, NavigationService navigate)
        {
            DTOs = new ObservableCollection<AccommodationDTO>(dtos);
            ViewAvailableDatesCommand = new RelayCommand(Button_Click_ViewAvailableDates);

            nagivation = navigate;

        }

        private void Button_Click_ViewAvailableDates(object param)
        {
            // var availableDates = new ShowSuggestionsDatesView(SelectedDTO);
            // availableDates.Show();
            // CloseWindow();
            nagivation.Navigate(new SuggestedDatesQuickSearch(SelectedDTO));
        }

    }
}
