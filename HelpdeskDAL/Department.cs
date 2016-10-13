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
    }
}
