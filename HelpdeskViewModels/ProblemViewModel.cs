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
     *          Provides Access to the DAL Layer for the Problem Controller
     */
    public class ProblemViewModel
    {
        private ProblemDAO _dao;

        public string Id { get; set; }
        public string Description { get; set; }
        public int Version { get; set; }

        public ProblemViewModel()
        {
            _dao = new ProblemDAO();
        }

        public void GetByProblemDescription()
        {
            try
            {
                Problem prob = _dao.GetByProblemDescription(Description);
                Id = prob.GetIdAsString();
                Description = prob.Description;
                Version = prob.Version;
            }
            catch(Exception ex)
            {
                ViewModelUtils.ErrorRoutine(ex, "ProblemViewModel", "GetByProblemDescription");
            }

        }

        public void GetById()
        {
            try
            {
                Problem prob = _dao.GetByProblemId(Id);
                Id = prob.GetIdAsString();
                Description = prob.Description;
                Version = prob.Version;
            }
            catch(Exception ex)
            {
                ViewModelUtils.ErrorRoutine(ex, "ProblemViewModel", "GetById");
            }
        }

        public int Update()
        {
            UpdateStatus opStatus;

            try
            {
                Problem prob = new Problem();
                prob.SetIdFromString(Id);
                prob.Description = Description;
                prob.Version = Version;

                opStatus = _dao.Update(prob);
            }
            catch(Exception ex)
            {
                ViewModelUtils.ErrorRoutine(ex, "ProblemViewModel", "Update");
                opStatus = UpdateStatus.Failed;
            }

            return Convert.ToInt16(opStatus);
        }

        public void Create()
        {
            try
            {
                Problem prob = new Problem();
                prob.Description = Description;
                prob.Version = Version;
                prob = _dao.Create(prob);
                Id = prob.GetIdAsString();
            }
            catch(Exception ex)
            {
                ViewModelUtils.ErrorRoutine(ex, "ProblemViewModel", "Create");
            }
        }

        public List<ProblemViewModel> GetAll()
        {
            List<ProblemViewModel> problemsViewModelList = new List<ProblemViewModel>();

            try
            {
                List<Problem> problems = _dao.GetAll();

                foreach(var problem in problems)
                {
                    ProblemViewModel pVM = new ProblemViewModel();
                    pVM.Id = problem.GetIdAsString();
                    pVM.Description = problem.Description;
                    pVM.Version = problem.Version;

                    problemsViewModelList.Add(pVM);
                }
            }
            catch(Exception ex)
            {
                ViewModelUtils.ErrorRoutine(ex, "ProblemViewModel", "GetAll");
            }

            return problemsViewModelList;
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
                ViewModelUtils.ErrorRoutine(ex, "ProblemViewModel", "Delete");
            }

            return delStatus;
        }
    }
}
