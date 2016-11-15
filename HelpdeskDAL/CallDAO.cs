using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;

namespace HelpdeskDAL
{
    public class CallDAO
    {
        private IRepository repo;

        public CallDAO()
        {
            repo = new HelpdeskRepository();
        }

        public Call GetById(string id)
        {
            Call call = null;
            var builder = Builders<Call>.Filter;
            var filter = builder.Eq("Id", new ObjectId(id));

            try
            {
                call = repo.GetOne<Call>(filter);
            }
            catch(Exception ex)
            {
                DALUtils.ErrorRoutine(ex, "CallDAO", "GetById");
            }

            return call;
        }

        public List<Call> GetAll()
        {
            List<Call> calls = null;

            try
            {
                calls = repo.GetAll<Call>();
            }
            catch(Exception ex)
            {
                DALUtils.ErrorRoutine(ex, "CallDAO", "GetAll");
            }

            return calls;
        }

        public UpdateStatus Update(Call call)
        {
            UpdateStatus status = UpdateStatus.Failed;
            repo = new HelpdeskRepository(new DbContext());

            try
            {
                if(Exists(call.Id))
                {
                    var builder = Builders<Call>.Filter;
                    var filter = builder.Eq("Id", call.Id) & builder.Eq("Version", call.Version);
                    var update = Builders<Call>.Update
                        .Set("EmployeeId", call.EmployeeId)
                        .Set("ProblemId", call.ProblemId)
                        .Set("TechId", call.TechId)
                        .Set("DateClosed", call.DateClosed)
                        .Set("DateOpened", call.DateOpened)
                        .Set("OpenStatus", call.OpenStatus)
                        .Set("Notes", call.Notes)
                        .Inc("Version", 1);

                    status = repo.Update(call.Id.ToString(), filter, update);
                }
                else
                {
                    status = UpdateStatus.Failed;
                }
            }
            catch(Exception ex)
            {
                DALUtils.ErrorRoutine(ex, "CallDAO", "Update");
            }

            return status;

        }

        public long Delete(string id)
        {
            repo = new HelpdeskRepository(new DbContext());
            long delete_status = 0;

            try
            {
                delete_status = repo.Delete<Call>(id);
            }
            catch(Exception ex)
            {
                DALUtils.ErrorRoutine(ex, "CallDAO", "Delete");
            }

            return delete_status;
        }

        public Call Create(Call call)
        {
            repo = new HelpdeskRepository(new DbContext());
            Call callRet = null;
            try
            {
                callRet = repo.Create<Call>(call);
            }
            catch(Exception ex)
            {
                DALUtils.ErrorRoutine(ex, "EmployeeDAO", "Create");
            }

            return callRet;
        }

        private bool Exists(ObjectId id)
        {
            var filter = Builders<Call>.Filter.Eq("Id", id);
            Call call = repo.GetOne(filter);
            return (call.GetIdAsString().Length == 24);
        }
    }
}
