﻿using Booking.Domain.ServiceInterfaces;
using Booking.Model;
using Booking.Observer;
using Booking.Repository;
using Booking.Service;
using Booking.Util;
using Booking.WPF.ViewModels.Guest1;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Booking.View
{

    public partial class FisrtGuestAllRequests : Page
    {

        public FisrtGuestAllRequests(NavigationService navigationService)
        {
            InitializeComponent();
            this.DataContext = new FirstGuestAllRequestsViewModel(navigationService);
             
            setWidthForReservationsDataGrid();
        }

        public void setWidthForReservationsDataGrid()
        {
            double totalWidth = 0;
            foreach (DataGridColumn column in RequestsDataGrid.Columns)
            {
                if (column.ActualWidth > 0)
                {
                    totalWidth += column.ActualWidth;
                }
            }
            RequestsDataGrid.Width = totalWidth;
        }

    }
}
