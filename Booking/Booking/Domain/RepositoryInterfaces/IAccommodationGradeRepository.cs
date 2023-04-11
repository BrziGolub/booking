﻿using Booking.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.RepositoryInterfaces
{
	public interface IAccommodationGradeRepository : IRepository<AccommodationGrade>
	{
		int NextId();
		AccommodationGrade Add(AccommodationGrade accommodationGrade);
		bool IsReservationGraded(int accommodationReservationId);
	}
}
