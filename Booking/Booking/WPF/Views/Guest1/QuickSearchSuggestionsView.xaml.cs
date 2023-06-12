﻿using Booking.WPF.ViewModels.Guest1;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Booking.WPF.Views.Guest1
{
    /// <summary>
    /// Interaction logic for QuickSearchSuggestionsView.xaml
    /// </summary>
    public partial class QuickSearchSuggestionsView : Page
    {
        public QuickSearchSuggestionsView(List<AccommodationDTO> dto, NavigationService navigate)
        {
            //List<AccommodationDTO> dto,NavigationService navigate parametar
            InitializeComponent();
            this.DataContext = new QuickSearchSuggestionsViewModel(dto,navigate);
        }
    }
}