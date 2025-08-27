using BCrypt.Net;
using ChatEvaluator.Dal.SqlServer;
using ChatEvaluator.SharedObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatEvaluator.Bal
{
    public class AccountRepository
    {
        public AccountRepository()
        {

        }
        public async Task<Users> GetUserByName(string username)
        {
            return await new SqlServerAccountRepository().GetUserByName(username);
        }
        public async Task<Users> GetUserById(int id)
        {
            return await new SqlServerAccountRepository().GetUserById(id);
        }
        public async Task<int> RegisterUser(LoginRequest model)
        {
            model.Password = BCrypt.Net.BCrypt.HashPassword(model.Password);
            return await new SqlServerAccountRepository().SaveUser(model);

        }
    }
}
