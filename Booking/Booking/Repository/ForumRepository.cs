using Booking.Domain.Model;
using Booking.Domain.RepositoryInterfaces;
using Booking.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Repository
{
    public class ForumRepository : IForumRepository
    {
        private const string FilePath = "../../Resources/Data/forums.csv";

        private readonly Serializer<Forum> _serializer;

        public List<Forum> _forums;

        public ForumRepository()
        {
            _serializer = new Serializer<Forum>();
            _forums = _serializer.FromCSV(FilePath);
        }
        public List<Forum> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }
        public Forum GetById(int id)
        {
            return _forums.Find(a => a.Id == id);
        }
        public int NextId()
        {
            if (_forums.Count == 0)
            {
                return 1;
            }
            else
            {
                return _forums.Max(t => t.Id) + 1;
            }
        }
        public Forum Add(Forum forum)
        {
            forum.Id = NextId();
            _forums.Add(forum);
            _serializer.ToCSV(FilePath, _forums);
            return forum;
        }
    }
}
