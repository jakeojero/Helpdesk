using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;

namespace HelpdeskDAL
{
    public class DepartmentDAO
    {
        IRepository repo;

        public DepartmentDAO()
        {
            repo = new HelpdeskRepository();
        }

        public Department GetByDepartmentName(string depName)
        {
            Department dep = null;
            var builder = Builders<Department>.Filter;
            var filter = builder.Eq("DepartmentName", depName);

            try
            {
                dep = repo.GetOne<Department>(filter);
            }
            catch (Exception ex)
            {
                DALUtils.ErrorRoutine(ex, "DepartmentDAO", "GetByDepartmentName");
            }

            return dep;
        }

        public Department GetByDepartmentId(string id)
        {
            Department dep = null;
            var builder = Builders<Department>.Filter;
            var filter = builder.Eq("Id", new ObjectId(id));

            try
            {
               dep = repo.GetOne<Department>(filter);
            }
            catch(Exception ex)
            {
                DALUtils.ErrorRoutine(ex, "DepartmentDAO", "GetByDepartmentId");
            }

            return dep;
        }

        public List<Department> GetAll()
        {
            List<Department> deps = null;

            try
            {
                deps = repo.GetAll<Department>();
            }
            catch (Exception ex)
            {
                DALUtils.ErrorRoutine(ex, "DepartmentDAO", "GetAll");
            }

            return deps;
        }

        public UpdateStatus Update(Department dep)
        {
            UpdateStatus status = UpdateStatus.Failed;
            repo = new HelpdeskRepository(new DbContext());

            try
            {
                var builder = Builders<Department>.Filter;
                var filter = builder.Eq("Id", dep.Id) & builder.Eq("Version", dep.Version);
                var update = Builders<Department>.Update
                    .Set("Id", dep.Id)
                    .Set("DepartmentName", dep.DepartmentName)
                    .Inc("Version", 1);

                status = repo.Update(dep.Id.ToString(), filter, update);
            }
            catch(Exception ex)
            {
                DALUtils.ErrorRoutine(ex, "DepartmentDAO", "Update");
            }

            return status;
        }

        public long Delete(string id)
        {
            repo = new HelpdeskRepository(new DbContext());
            long delete_status = 0;

            try
            {
                delete_status = repo.Delete<Department>(id);
            }
            catch (Exception ex)
            {
                DALUtils.ErrorRoutine(ex, "DepartmentDAO", "Delete");
            }

            return delete_status;
        }

        public Department Create(Department dep)
        {
            repo = new HelpdeskRepository(new DbContext());
            Department depRet = null;
            try
            {
                depRet = repo.Create<Department>(dep);
            }
            catch (Exception ex)
            {
                DALUtils.ErrorRoutine(ex, "DepartmentDAO", "Create");
            }

            return depRet;
        }
    }
}
