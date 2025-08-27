using ChatEvaluator.Dal.Interfaces;
using ChatEvaluator.SharedObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ChatEvaluator.Dal.SqlServer
{
    public class SqlServerActivityLog : IActivityLogRepository
    {
        public async Task<int> SaveActivityLog(ActivityLogs model)
        {
            string sql = @"insert into ActivityLogs(UserId,Expression,Result,ResultCode)
                          values
                          (@userId,@expression,@result,@resultCode)";
            object[] parms = { "@userId", model.UserId, "@expression", model.Expression, "@result", model.Result, "@resultCode", model.ResultCode };
            return await SqlServerBase.SaveData(sql, parms);
        }
        public async Task<List<ActivityLogs>> GetActivityLogs(int userId)
        {
            string sql = @"select Id, UserId, Expression, Result, ResultCode
                          from dbo.ActivityLogs
                          where UserId=@userid
                          order by Id DESC";
            object[] parms = { "@userid", userId };
            return await SqlServerBase.GetDataList<ActivityLogs>(sql, parms);
        }
    }
}
