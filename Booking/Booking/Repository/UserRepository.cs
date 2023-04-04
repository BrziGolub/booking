﻿using Booking.Model;
using Booking.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Repository
{
	public class UserRepository
	{
		private const string FilePath = "../../Resources/Data/users.csv";

		private readonly Serializer<User> _serializer;

		private List<User> _users;

		public UserRepository()
		{
			_serializer = new Serializer<User>();
			_users = _serializer.FromCSV(FilePath);
		}
		//Load i Save napraviti 
		public User GetByUsername(string username)
		{
			_users = _serializer.FromCSV(FilePath);
			return _users.FirstOrDefault(u => u.Username == username);
		}

	}
}
