using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace HelpdeskDAL
{
    public class Problem : HelpdeskEntity
    {
        public string Description { get; set; }
    }
}
