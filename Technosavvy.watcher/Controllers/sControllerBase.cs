using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NavExM.Int.Watcher.WatchDog.Data;

namespace NavExM.Int.Watcher.WatchDog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class sControllerBase : ControllerBase
    {
        
        //internal ApiAppContext ctx;        
        internal HttpContext httpContext;        
        public sControllerBase()
        {
            
        }
        internal void LogEvent(string msg)
        {
            mLogT vL = new mLogT() { Message = msg, Type = eLogType.Event };
            LogList.AddLog(vL);
            Console.WriteLine(msg);
        }
        internal void LogError(string msg)
        {
            mLogT vL = new mLogT() { Message = msg, Type = eLogType.Error };
            LogList.AddLog(vL);
            Console.WriteLine(msg);
           
        }
        internal void LogError(Exception ex)
        {

            mLogT vL = new mLogT() { Message = GetMsg(ex), Type = eLogType.Event };
            LogList.AddLog(vL);
            Console.WriteLine(GetMsg(ex));
         
        }
        internal string GetMsg(Exception ex)
        {
            if (ex is null) return string.Empty;
            var msg = $"{ex.Message}{GetMsg(ex.InnerException)}";
            return msg;
        }
    }
}
