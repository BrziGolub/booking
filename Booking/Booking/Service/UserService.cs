using Booking.Model;
using Booking.Repository;
using Booking.Serializer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Service
{
    public class UserService
    {
        private readonly UserRepository _repository;

        private List<User> _users;

        public UserService()
        {
            _repository = new UserRepository();
            Load();
        }

        public void Load()
        {
            _users = _repository.Load();
        }

        public void Save()
        {
            _repository.Save(_users);
        }

        public User GetById(int id)
        {
            return _users.Find(v => v.Id == id);
        }

        public List<User> GetAll()
        {
            return _users;
        }

        public User GetByUsername(string username)
        {
            return _users.Find(u => u.Username == username);
        }
    }
}
