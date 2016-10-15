using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;

namespace HelpdeskDAL
{
    public class ProblemDAO
    {
        IRepository repo;

        public ProblemDAO()
        {
            repo = new HelpdeskRepository();
        }

        public Problem GetByProblemDescription(string description)
        {
            Problem prob = null;
            var builder = Builders<Problem>.Filter;
            var filter = builder.Eq("Description", description);

            try
            {
                prob = repo.GetOne<Problem>(filter);
            }
            catch (Exception ex)
            {
                DALUtils.ErrorRoutine(ex, "ProblemDAO", "GetByProblemDescription");
            }

            return prob;
        }

        public Problem GetByProblemId(string id)
        {
            Problem prob = null;
            var builder = Builders<Problem>.Filter;
            var filter = builder.Eq("Id", new ObjectId(id));

            try
            {
                prob = repo.GetOne<Problem>(filter);
            }
            catch (Exception ex)
            {
                DALUtils.ErrorRoutine(ex, "ProblemDAO", "GetByProblemId");
            }

            return prob;

        }

        public List<Problem> GetAll()
        {
            List<Problem> probs = null;

            try
            {
                probs = repo.GetAll<Problem>();
            }
            catch(Exception ex)
            {
                DALUtils.ErrorRoutine(ex, "ProblemDAO", "GetAll");
            }

            return probs;
        }

        public UpdateStatus Update(Problem prob)
        {
            UpdateStatus status = UpdateStatus.Failed;
            repo = new HelpdeskRepository(new DbContext());

            try
            {
                var builder = Builders<Problem>.Filter;
                var filter = builder.Eq("Id", prob.Id) & builder.Eq("Version", prob.Version);
                var update = Builders<Problem>.Update
                    .Set("Id", prob.Id)
                    .Set("Description", prob.Description)
                    .Inc("Version", 1);

                status = repo.Update<Problem>(prob.Id.ToString(), filter, update);
            }
            catch (Exception ex)
            {
                DALUtils.ErrorRoutine(ex, "ProblemDAO", "Update");
            }

            return status;
        }

        public Problem Create(Problem prob)
        {
            repo = new HelpdeskRepository(new DbContext());
            Problem probRet = null;
            
            try
            {
                probRet = repo.Create<Problem>(prob);
            }
            catch(Exception ex)
            {
                DALUtils.ErrorRoutine(ex, "ProblemDAO", "Create");
            }

            return probRet;
        }

        public long Delete(string id)
        {
            repo = new HelpdeskRepository(new DbContext());
            long delete_status = 0;

            try
            {
                delete_status = repo.Delete<Problem>(id);
            }
            catch(Exception ex)
            {
                DALUtils.ErrorRoutine(ex, "ProblemDAO", "Delete");
            }

            return delete_status;
        }
    }
}
