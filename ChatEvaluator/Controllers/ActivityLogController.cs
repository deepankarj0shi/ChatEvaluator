using ChatEvaluator.SharedObject.Models;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using System.Security.Claims;

namespace ChatEvaluator.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/activitylog")]
    public class ActivityLogController : ControllerBase
    {
        public ActivityLogController()
        {
            
        }
        [HttpPost("submit")]
        public async Task<ActivityLogs> SubmitChat([FromBody]string expression)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var result = await CSharpScript.EvaluateAsync<int>(expression);
            var resultCode = result.ToWords();
            ActivityLogs activityLogs = new()
            {
                UserId= userId,
                Expression = expression,
                Result=result,
                ResultCode = resultCode
            };
            await new Bal.ActivityLogRepository().SaveActivityLog(activityLogs);
            return activityLogs;
        }
        [HttpGet("history")]
        public async Task<List<ActivityLogs>> GetActivityLog()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            return await new Bal.ActivityLogRepository().GetActivityLogs(userId);
        }

    }
}
