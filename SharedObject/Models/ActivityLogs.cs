using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ChatEvaluator.SharedObject.Models
{
    public class ActivityLogs
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? Expression { get; set; }
        public int Result { get; set; }
        public string? ResultCode { get; set; }
    }
}
