using Booking.Model;
using Booking.Model.DAO;
using Booking.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Service
{
	public class TourReservationService
	{
		private readonly TourReservationRepository _repository;
		private List<TourReservation> _reservations;

		private TourService _tourService;

		public TourReservationService()
		{
			_repository = new TourReservationRepository();
			_reservations = new List<TourReservation>();

			_tourService = new TourService();

			Load();
		}

		public void Load()
		{
			_reservations = _repository.Load();
		}

		public void Save()
		{
			_repository.Save(_reservations);
		}

		public TourReservation GetById(int id)
		{
			return _reservations.Find(v => v.Id == id);
		}

		public List<TourReservation> GetAll()
		{
			return _reservations;
		}

		public int GenerateId()
		{
			if (_reservations.Count == 0) return 0;
			return _reservations.Max(s => s.Id) + 1;
		}

		public void CreateTourReservation(Tour tour, int visitors)
		{
			TourReservation reservation = new TourReservation();

			reservation.Id = GenerateId();
			reservation.Tour = tour;
			reservation.NumberOfVisitors = visitors;

			_reservations.Add(reservation);

			Save();
		}

		public int CheckAvailability(int id)
		{
			int availability = _tourService.GetById(id).MaxVisitors;
			int busyness = 0;


			foreach (TourReservation res in _reservations)
			{
				if (res.Tour.Id == id)
				{
					busyness += res.NumberOfVisitors;
				}
			}

			return availability - busyness;
		}
	}
}
