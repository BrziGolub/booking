﻿using Booking.Model.DAO;
using Booking.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Booking.Observer;
using System.Collections.ObjectModel;

namespace Booking.Controller
{
	public class TourController
	{
		private readonly TourDAO tourDAO;

        public TourController(TourDAO tour)
        {
            tourDAO = tour;
        }

        public List<Tour> GetAll()
		{
			return tourDAO.GetAll();
		}

		public void Search(ObservableCollection<Tour> observe, string state, string city, string duration, string language, string passengers) 
		{
			tourDAO.Search(observe, state, city, duration, language, passengers);
		}

		public void CancelSearch(ObservableCollection<Tour> observe)
		{
			tourDAO.CancelSearch(observe);
		}

		public void Subscribe(IObserver observer)
		{
			tourDAO.Subscribe(observer);
		}

		public void Create(Tour tour)
		{
			tourDAO.addTour(tour);
		}
	}
}
