using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatEvaluator.SharedObject.Models
{
    public class LoginResponse
    {
        public LoginRequest? LoginRequest { get; set; }
        public string? JwtToken { get; set; }
    }
}
