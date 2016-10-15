using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelpdeskDAL;

namespace HelpdeskViewModels
{
    /*
     *          DepartmentViewModel Class
     *          Provides Access to the DAL Layer for the Department Controller
     */
    public class DepartmentViewModel
    {
        private DepartmentDAO _dao;

        public string Id { get; set; }
        public string Name { get; set; }
        public int Version { get; set; }

        public DepartmentViewModel()
        {
            _dao = new DepartmentDAO();
        }

        public void GetByDepartmentName()
        {
            try
            {
                Department dep = _dao.GetByDepartmentName(Name);
                Id = dep.GetIdAsString();
                Name = dep.DepartmentName;
                Version = dep.Version;
            }
            catch(Exception ex)
            {
                ViewModelUtils.ErrorRoutine(ex, "DepartmentViewModel", "GetByDepartmentName");
            }
        }

        public void GetById()
        {
            try
            {
                Department dep = _dao.GetByDepartmentId(Id);
                Id = dep.GetIdAsString();
                Name = dep.DepartmentName;
                Version = dep.Version;
            }
            catch(Exception ex)
            {
                ViewModelUtils.ErrorRoutine(ex, "DepartmentViewModel", "GetById");
            }

        }

        public int Update()
        {
            UpdateStatus opStatus;

            try
            {
                Department dep = new Department();
                dep.SetIdFromString(Id);
                dep.DepartmentName = Name;
                dep.Version = Version;

                opStatus = _dao.Update(dep);
            }
            catch(Exception ex)
            {
                ViewModelUtils.ErrorRoutine(ex, "DepartmentViewModel", "Update");
                opStatus = UpdateStatus.Failed;
            }

            return Convert.ToInt16(opStatus);
        }

        public void Create()
        {
            try
            {
                Department dep = new Department();
                dep.DepartmentName = Name;
                dep.Version = Version;
                dep = _dao.Create(dep);
                Id = dep.GetIdAsString();
            }
            catch(Exception ex)
            {
                ViewModelUtils.ErrorRoutine(ex, "DepartmentViewModel", "Create");
            }
        }

        public List<DepartmentViewModel> GetAll()
        {
            List<DepartmentViewModel> depsViewModelList = new List<DepartmentViewModel>();

            try
            {
                List<Department> departments = _dao.GetAll();

                foreach(var department in departments)
                {
                    DepartmentViewModel dVM = new DepartmentViewModel();
                    dVM.Id = department.GetIdAsString();
                    dVM.Name = department.DepartmentName;
                    dVM.Version = department.Version;

                    depsViewModelList.Add(dVM);
                }
            }
            catch (Exception ex)
            {
                ViewModelUtils.ErrorRoutine(ex, "DepartmentViewModel", "GetAll");
            }

            return depsViewModelList;
        }

        public long Delete()
        {
            long delStatus = 0;
            try
            {
                delStatus = _dao.Delete(Id);
            }
            catch (Exception ex)
            {
                ViewModelUtils.ErrorRoutine(ex, "DepartmentViewModel", "Delete");
            }

            return delStatus;
        }

    }
}
