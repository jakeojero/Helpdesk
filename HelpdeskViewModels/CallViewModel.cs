using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelpdeskDAL;

namespace HelpdeskViewModels
{
    public class CallViewModel
    {
        private CallDAO _dao;

        public string Id { get; set; }
        public int Version { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string ProblemId { get; set; }
        public string ProblemDescription { get; set; }
        public string TechId { get; set; }
        public string TechName { get; set; }
        public System.DateTime DateOpened { get; set; }
        public System.DateTime? DateClosed { get; set; }
        public bool OpenStatus { get; set; }
        public string Notes { get; set; }

        public CallViewModel()
        {
            _dao = new CallDAO();
        }

        public void GetById()
        {
            try
            {
                Call call = _dao.GetById(Id);
                Id = call.GetIdAsString();
                EmployeeId = call.GetEmployeeIdAsString();
                ProblemId = call.GetProblemIdAsString();
                TechId = call.GetTechIdAsString();
                DateOpened = call.DateOpened;
                DateClosed = call.DateClosed;
                OpenStatus = call.OpenStatus;
                Version = call.Version;
                Notes = call.Notes;
            }
            catch(Exception ex)
            {
                Id = "not found";
                ViewModelUtils.ErrorRoutine(ex, "CallViewModel", "GetById");
            }
        }

        public List<CallViewModel> GetAll()
        {
            List<CallViewModel> callViewModelList = new List<CallViewModel>();

            try
            {
                List<Call> calls = _dao.GetAll();

                foreach(var call in calls)
                {
                    CallViewModel cVM = new CallViewModel();
                    Employee emp = new EmployeeDAO().GetById(call.GetEmployeeIdAsString());
                    Problem prob = new ProblemDAO().GetByProblemId(call.GetProblemIdAsString());
                    Employee techName = new EmployeeDAO().GetById(call.GetTechIdAsString());
                    cVM.Id = call.GetIdAsString();
                    cVM.ProblemDescription = prob.Description;
                    cVM.EmployeeName = emp.Lastname;
                    cVM.EmployeeId = call.GetEmployeeIdAsString();
                    cVM.ProblemId = call.GetProblemIdAsString();
                    cVM.TechId = call.GetTechIdAsString();
                    cVM.TechName = techName.Lastname;
                    cVM.DateOpened = call.DateOpened;
                    cVM.DateClosed = call.DateClosed;
                    cVM.OpenStatus = call.OpenStatus;
                    cVM.Notes = call.Notes;
                    cVM.Version = call.Version;
                    callViewModelList.Add(cVM);
                }
            }
            catch(Exception ex)
            {
                ViewModelUtils.ErrorRoutine(ex, "CallViewModel", "GetAll");
            }

            return callViewModelList;
        }

        public int Update()
        {
            UpdateStatus opStatus;

            try
            {
                Call call = new Call();
                call.SetIdFromString(Id);
                call.SetEmployeeIdFromString(EmployeeId);
                call.SetProblemIdFromString(ProblemId);
                call.SetTechIdFromString(TechId);
                call.OpenStatus = OpenStatus;
                call.DateClosed = DateClosed;
                call.DateOpened = DateOpened;
                call.Notes = Notes;
                call.Version = Version;
                opStatus = _dao.Update(call);
            }
            catch(Exception ex)
            {
                ViewModelUtils.ErrorRoutine(ex, "CallViewModel", "Update");
                opStatus = UpdateStatus.Failed;
            }

            return Convert.ToInt16(opStatus);
        }
        public void Create()
        {
            try
            {
                Call call = new Call();
                call.SetEmployeeIdFromString(EmployeeId);
                call.SetProblemIdFromString(ProblemId);
                call.SetTechIdFromString(TechId);
                call.OpenStatus = OpenStatus;
                call.DateClosed = DateClosed;
                call.DateOpened = DateOpened;
                call.Notes = Notes;
                call.Version = Version;
                call = _dao.Create(call);
                Id = call.GetIdAsString();

            }
            catch(Exception ex)
            {
                ViewModelUtils.ErrorRoutine(ex, "CallViewModel", "Create");
            }
        }

        public long Delete()
        {
            long delStatus = 0;
            try
            {
                delStatus = _dao.Delete(Id);
            }
            catch(Exception ex)
            {
                ViewModelUtils.ErrorRoutine(ex, "CallViewModel", "Delete");
            }
            return delStatus;
        }

    }
}
