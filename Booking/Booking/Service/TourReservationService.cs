using Booking.Domain.RepositoryInterfaces;
using Booking.Domain.ServiceInterfaces;
using Booking.Model;
using Booking.Repository;
using Booking.Util;
using System.Collections.Generic;
using System.Linq;

namespace Booking.Service
{
	public class TourReservationService : ITourReservationService
	{
		private readonly ITourReservationRepository _repository;
		private readonly ITourRepository _tourRepository;

		public TourReservationService()
		{
			_repository = InjectorRepository.CreateInstance<ITourReservationRepository>();
			_tourRepository = InjectorRepository.CreateInstance<ITourRepository>();
		}

		public TourReservation GetById(int id)
		{
			return _repository.GetById(id);
		}

		public List<TourReservation> GetAll()
		{
			return _repository.GetAll();
		}

		public void CreateTourReservation(Tour tour, int visitors)
		{
			TourReservation reservation = new TourReservation();

			reservation.Tour = tour;
			reservation.User.Id = TourService.SignedGuideId;
			reservation.NumberOfVisitors = visitors;

			_repository.Add(reservation);
		}

		public int CheckAvailability(int id)
		{
			int availability = _tourRepository.GetById(id).MaxVisitors;
			int busyness = 0;


			foreach (TourReservation res in _repository.GetAll())
			{
				if (res.Tour.Id == id)
				{
					busyness += res.NumberOfVisitors;
				}
			}

			return availability - busyness;
		}

		public TourReservation GetActiveTour(int id)
		{
			List<TourReservation> list = _repository.GetReservationsByGuestId(id);
            TourReservation tourReservation = new TourReservation();

            foreach (TourReservation res in list)
			{
				res.Tour = _tourRepository.GetById(res.Tour.Id);
				if(res.Tour.IsStarted)
				{
                    tourReservation = res;
				}
			}

			foreach (TourKeyPoint kp in tourReservation.Tour.Destinations)
			{
				if(kp.Achieved)
				{
					tourReservation.Tour.Location = kp.Location;
				}
			}

			return tourReservation;
		}
	}
}
