using ChatEvaluator.SharedObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatEvaluator.Dal.Interfaces
{
    public interface IJwtTokenManagement
    {
        string GenerateToken(Users user);
        void ClearToken();
    }
}
