using ChatEvaluator.Dal.SqlServer;
using ChatEvaluator.SharedObject.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatEvaluator.Bal
{
    public class ActivityLogRepository
    {
        public ActivityLogRepository()
        {
            
        }
        public async Task<int> SaveActivityLog(ActivityLogs model)
        {
            return await new SqlServerActivityLog().SaveActivityLog(model);
        }
        public async Task<List<ActivityLogs>> GetActivityLogs(int userId)
        {
            return await new SqlServerActivityLog().GetActivityLogs(userId);
        }
    }
}
