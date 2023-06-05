using Booking.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Booking.WPF.ViewModels.Guide
{
    public class GuideSuperGuideViewModel
    {
        public RelayCommand Close { get; set; }
        private readonly Window _window;
        public GuideSuperGuideViewModel(Window window) 
        {
            _window = window;
            SetCommands();
        }

        

        public void SetCommands()
        {
            Close = new RelayCommand(ButtonClose);
        }

        private void ButtonClose(object param)
        {
            CloseWindow();
        }
        private void CloseWindow()
        {
            _window.Close();
        }
    }
}
