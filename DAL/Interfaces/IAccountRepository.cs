using ChatEvaluator.SharedObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatEvaluator.Dal.Interfaces
{
    public interface IAccountRepository
    {
        Task<Users> GetUserByName(string username);
        Task<Users> GetUserById(int id);
        Task<int> SaveUser(LoginRequest model);


    }
}
