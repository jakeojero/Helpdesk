using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelpdeskDAL;

namespace HelpdeskViewModels
{
    /*
     *          ProblemViewModel Class
     *          Provides Access to the DAL Layer for the Employee Controller
     */
    public class EmployeeViewModel
    {
        private EmployeeDAO _dao;

        public string Title { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Phoneno { get; set; }
        public int Version { get; set; }
        public string DepartmentId { get; set; }
        public string Id { get; set; }

        public EmployeeViewModel()
        {
            _dao = new EmployeeDAO();
        }

        public void GetByLastname()
        {
            try
            {
                Employee emp = _dao.GetByLastname(Lastname);
                Title = emp.Title;
                Firstname = emp.Firstname;
                Lastname = emp.Lastname;
                Phoneno = emp.Phoneno;
                Email = emp.Email;
                Id = emp.GetIdAsString();
                DepartmentId = emp.GetDepartmentIdAsString();
                Version = emp.Version;
            }
            catch(Exception ex)
            {
                ViewModelUtils.ErrorRoutine(ex, "EmployeeViewModel", "GetByLastName");
            }
        }

        public void GetById()
        {
            try
            {
                Employee emp = _dao.GetById(Id);
                Title = emp.Title;
                Firstname = emp.Firstname;
                Lastname = emp.Lastname;
                Phoneno = emp.Phoneno;
                Email = emp.Email;
                Id = emp.GetIdAsString();
                DepartmentId = emp.GetDepartmentIdAsString();
                Version = emp.Version;
            }
            catch(Exception ex)
            {
                ViewModelUtils.ErrorRoutine(ex, "EmployeeViewModel", "GetById");
            }
        }

        public int Update()
        {
            UpdateStatus opStatus;

            try
            {
                Employee emp = new Employee();
                emp.SetIdFromString(Id);
                emp.SetDepartmentIdFromString(DepartmentId);
                emp.Title = Title;
                emp.Firstname = Firstname;
                emp.Lastname = Lastname;
                emp.Phoneno = Phoneno;
                emp.Email = Email;
                emp.Version = Version;
                opStatus = _dao.Update(emp);
            }
            catch (Exception ex)
            {
                ViewModelUtils.ErrorRoutine(ex, "EmployeeViewModel", "Update");
                opStatus = UpdateStatus.Failed;
            }

            return Convert.ToInt16(opStatus);
        }

        public void Create()
        {
            try
            {
                Employee emp = new Employee();
                emp.SetDepartmentIdFromString(DepartmentId);
                emp.Title = Title;
                emp.Firstname = Firstname;
                emp.Lastname = Lastname;
                emp.Phoneno = Phoneno;
                emp.Email = Email;
                emp.Version = Version;
                emp = _dao.Create(emp);
                Id = emp.GetIdAsString();
            }
            catch (Exception ex)
            {
                ViewModelUtils.ErrorRoutine(ex, "EmployeeViewModel", "Create");
            }
        }

        public List<EmployeeViewModel> GetAll()
        {
            List<EmployeeViewModel> empsViewModelList = new List<EmployeeViewModel>();
            try
            {
                List<Employee> emps = _dao.GetAll();
                
                foreach (var employee in emps)
                {
                    EmployeeViewModel eVM = new EmployeeViewModel();
                    eVM.Title = employee.Title;
                    eVM.Firstname = employee.Firstname;
                    eVM.Lastname = employee.Lastname;
                    eVM.Phoneno = employee.Phoneno;
                    eVM.Email = employee.Email;
                    eVM.Version = employee.Version;
                    eVM.Id = employee.GetIdAsString();
                    eVM.DepartmentId = employee.GetDepartmentIdAsString();
                    empsViewModelList.Add(eVM);
                }
            }
            catch (Exception ex)
            {
                ViewModelUtils.ErrorRoutine(ex, "EmployeeViewModel", "GetAll");
            }

            return empsViewModelList;
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
                ViewModelUtils.ErrorRoutine(ex, "EmployeeViewModel", "Delete");
            }

            return delStatus;
        }

    }
}
