using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace HelpdeskDAL
{
    public class DbContext
    {
        public IMongoDatabase Db;

        public DbContext()
        {
            MongoClient client = new MongoClient(); // Connect to local host
            Db = client.GetDatabase("HelpdeskDB2");
        }

        public IMongoCollection<HelpdeskEntity> GetCollection<HelpdeskEntity>()
        {
            return Db.GetCollection<HelpdeskEntity>(typeof(HelpdeskEntity).Name.ToLower() + "s");
        }
    }
}
