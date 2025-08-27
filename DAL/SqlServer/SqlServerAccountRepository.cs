using ChatEvaluator.SharedObject.Models;
using ChatEvaluator.Dal.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;

namespace ChatEvaluator.Dal.SqlServer
{
    public class SqlServerAccountRepository : IAccountRepository
    {
        public async Task<Users> GetUserByName(string username)
        {
            string sql = @"select top 1 Id, Username, password
                        from
                        Users
                        where
                        username=@username";
            object[] parms = { "@username", username };
            return await SqlServerBase.GetData<Users>(sql, parms);
        }
        public async Task<Users> GetUserById(int id)
        {
            string sql = @"select top 1 Id, Username, password
                        from
                        Users
                        where
                        id=@id";
            object[] parms = { "@id", id };
            return await SqlServerBase.GetData<Users>(sql, parms);
        }
        public async Task<int> SaveUser(LoginRequest model)
        {
            string sql = @"insert into Users(Username, password)
                        values
                        (@username, @password)";
            object[] parms = { "@username", model.Username, "@password", model.Password };
            return await SqlServerBase.SaveData(sql, parms);
        }
    }
}
