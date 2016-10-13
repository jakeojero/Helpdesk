using MongoDB.Driver;
using MongoDB.Bson;
using System;
using System.Collections.Generic;

namespace HelpdeskDAL
{
    public class HelpdeskRepository : IRepository
    {
        private DbContext ctx = null;
        public HelpdeskRepository(DbContext context = null)
        {
            ctx = context != null ? context : new DbContext();
        }
        public HelpdeskEntity GetById<HelpdeskEntity>(string id)
        {
            var filter = Builders<HelpdeskEntity>.Filter.Eq("Id", new ObjectId(id));
            return GetOne<HelpdeskEntity>(filter);
        }
        public HelpdeskEntity GetOne<HelpdeskEntity>(FilterDefinition<HelpdeskEntity> filter)
        {
            var collection = GetCollection<HelpdeskEntity>();
            return collection.Find(filter).FirstOrDefault();
        }
        public List<HelpdeskEntity> GetMany<HelpdeskEntity>(FilterDefinition<HelpdeskEntity> filter)
        {
            var collection = GetCollection<HelpdeskEntity>();
            return collection.Find(filter).ToList();
        }
        public List<HelpdeskEntity> GetAll<HelpdeskEntity>()
        {
            var collection = GetCollection<HelpdeskEntity>();
            return collection.Find(new BsonDocument()).ToList();
        }
        public bool Exists<HelpdeskEntity>(string id)
        {
            var collection = GetCollection<HelpdeskEntity>();
            ObjectId ID = new ObjectId(id);
            var filter = Builders<HelpdeskEntity>.Filter.Eq("Id", ID);
            var emps = collection.Find(filter);
            return (emps.Count() > 0); // if id is found count = 1
        }
        public HelpdeskEntity Create<HelpdeskEntity>(HelpdeskEntity item)
        {
            var collection = GetCollection<HelpdeskEntity>();
            collection.InsertOne(item);
            return item;
        }
        public long Delete<HelpdeskEntity>(string id)
        {
            var filter = new FilterDefinitionBuilder<HelpdeskEntity>().Eq("Id", new ObjectId(id));
            var collection = GetCollection<HelpdeskEntity>();
            var deleteRes = collection.DeleteOne(filter);
            return deleteRes.DeletedCount;
        }
        private IMongoCollection<HelpdeskEntity> GetCollection<HelpdeskEntity>()
        {
            return ctx.GetCollection<HelpdeskEntity>();
        }

        public UpdateStatus Update<HelpdeskEntity>(string id, FilterDefinition<HelpdeskEntity> filter, UpdateDefinition<HelpdeskEntity> update)
        {
            UpdateStatus status = UpdateStatus.Failed;
            if (Exists<HelpdeskEntity>(id))
            {
                try
                {
                    var collection = GetCollection<HelpdeskEntity>();
                    var updateRes = collection.UpdateOne(filter, update);

                    if (updateRes.MatchedCount == 0)
                    {
                        status = UpdateStatus.Stale;
                    }
                    else
                    {
                        status = UpdateStatus.Ok;
                    }
                }
                catch (Exception ex)
                {
                    try
                    {
                        Console.WriteLine("Exception thrown in repo update " + ex.Message);
                    }
                    catch (Exception ex2) { } // don't bubble up ex, just return fail
                }
            }
            return status;
        }
    }
}