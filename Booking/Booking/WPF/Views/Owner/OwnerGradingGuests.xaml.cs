﻿using Booking.Domain.ServiceInterfaces;
using Booking.Model;
using Booking.Service;
using Booking.Util;
using Booking.WPF.ViewModels.Owner;
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
    /// <summary>
    /// Interaction logic for OwnerGradingGuests.xaml
    /// </summary>
    public partial class OwnerGradingGuests : Window
    {
        public OwnerGradingGuests()
        {
            InitializeComponent();
            this.DataContext = new OwnerGradingGuestsViewModel(this);
        }
    }
}
