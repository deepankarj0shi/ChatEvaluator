using ChatEvaluator.SharedObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatEvaluator.Dal.Interfaces
{
    public interface IActivityLogRepository
    {
        Task<int> SaveActivityLog(ActivityLogs model);
        Task<List<ActivityLogs>> GetActivityLogs(int userId);

    }
}
