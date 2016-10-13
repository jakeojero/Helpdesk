using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpdeskDAL
{
    public interface IRepository
    {
        HelpdeskEntity GetById<HelpdeskEntity>(string id);

        HelpdeskEntity GetOne<HelpdeskEntity>(FilterDefinition<HelpdeskEntity> filter);

        List<HelpdeskEntity> GetMany<HelpdeskEntity>(FilterDefinition<HelpdeskEntity> filter);

        List<HelpdeskEntity> GetAll<HelpdeskEntity>();

        HelpdeskEntity Create<HelpdeskEntity>(HelpdeskEntity item);

        long Delete<HelpdeskEntity>(string id);

        UpdateStatus Update<HelpdeskEntity>(string id,
                                            FilterDefinition<HelpdeskEntity> filter,
                                            UpdateDefinition<HelpdeskEntity> update);
    }
}
