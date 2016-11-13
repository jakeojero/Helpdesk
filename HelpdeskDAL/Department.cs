using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;

namespace HelpdeskDAL
{
    public class Department : HelpdeskEntity
    {
        public string DepartmentName { get; set; }

        public ObjectId ManagerId { get; set; }

        public string GetManagerIdAsString()
        {
            return this.ManagerId.ToString();
        }
        public void SetDepartmentIdFromString(string id)
        {
            this.ManagerId = new ObjectId(id);
        }
    }
}
