using MongoDB.Bson;

namespace HelpdeskDAL
{
    public class Call : HelpdeskEntity
    {
        public ObjectId EmployeeId { get; set; }
        public ObjectId ProblemId { get; set; }
        public ObjectId TechId { get; set; }
        public System.DateTime DateOpened { get; set; }
        public System.DateTime? DateClosed { get; set; }
        public bool OpenStatus { get; set; }
        public string Notes { get; set; }
        public string GetEmployeeIdAsString()
        {
            return this.EmployeeId.ToString();
        }
        public void SetEmployeeIdFromString(string id)
        {
            this.EmployeeId = new ObjectId(id);
        }
        public string GetProblemIdAsString()
        {
            return this.ProblemId.ToString();
        }
        public void SetProblemIdFromString(string id)
        {
            this.ProblemId = new ObjectId(id);
        }
        public string GetTechIdAsString()
        {
            return this.TechId.ToString();
        }
        public void SetTechIdFromString(string id)
        {
            this.TechId = new ObjectId(id);
        }
    }
}
