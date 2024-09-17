using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using NavExM.Int.Watcher.WatchDog.Data;
using NavExM.Int.Watcher.WatchDog.Manager;
using NavExM.Int.Watcher.WatchDog.Model.AppInt;
using NavExM.Int.Watcher.WatchDog.WHub;

namespace NavExM.Int.Watcher.WatchDog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WatcherController : sControllerBase
    {
        private IHubContext<ErrorHub> _errorHub;
        private IHubContext<EventHub> _eventHub;
        private IHubContext<LogHub> _logHub;
        public WatcherController( IHttpContextAccessor _http, IHubContext<ErrorHub> errorHub, IHubContext<EventHub> eventHub, IHubContext<LogHub> logHub)
        {
          //  ctx = _ctx;
            httpContext = _http.HttpContext!;
            _errorHub = errorHub;
            _eventHub = eventHub;
            _logHub = logHub;
        }
        #region Error And Logs

        [HttpPost("EnableAutoRegistration")]
        public ActionResult<Tuple<bool, string>> EnableAutoRegistration(string RegistryName)
        {
            try
            {
                var wm = GetWatcherManager();
                var res = wm.EnableAutoRegistration(RegistryName);
                return Ok(res);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return BadRequest($"{GetMsg(ex)}");

            }
        }

        [HttpPost("EnableAutoRegistrationForAll")]
        public ActionResult<Tuple<bool, string>> EnableAutoRegistrationForAll(int duration)
        {
            try
            {
                var wm = GetWatcherManager();
                var res = wm.EnableAutoRegistrationForAllRegistries(duration);
                return Ok(res);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return BadRequest($"{GetMsg(ex)}");

            }
        }
        [HttpPost("GetAllRegistries")]
        public ActionResult<List<string>> GetAllRegistries()
        {
            try
            {
                var wm = GetWatcherManager();
                var res = wm.GetRegistries();
                return Ok(res);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return BadRequest($"{GetMsg(ex)}");

            }
        }
        //[HttpPost("GetAllRegistries")]
        //public ActionResult<List<string>> Get()
        //{
        //    try
        //    {
        //        var wm = GetWatcherManager();
        //        var res = wm.GetRegistries();
        //        return Ok(res);
        //    }
        //    catch (Exception ex)
        //    {
        //        LogError(ex);
        //        return BadRequest($"{GetMsg(ex)}");

        //    }
        //}
        [HttpPost("GetRegistryAutoStatus")]
        public ActionResult<string> GetAllRegistriesAutoStatus(string RegistryName)
        {
            try
            {
                var wm = GetWatcherManager();
                var res = wm.GetRegistryStatus(RegistryName);
                return Ok(res);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return BadRequest($"{GetMsg(ex)}");
            }
        }

        [HttpPost("GetAllRegistriesAutoStatus")]
        public ActionResult<List<string>> GetAllRegistriesAutoStatus()
        {
            try
            {
                var wm = GetWatcherManager();
                var res = wm.GetAllRegistriesAutoStatus();
                return Ok(res);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return BadRequest($"{GetMsg(ex)}");
            }
        }
        [HttpPost("GetRegistrationCandidates")]
        public ActionResult<mHandShakePackage> GetRegistrationCandidates(string RegistryName)
        {
            try
            {
                var wm = GetWatcherManager();
                var res = wm.GetRegistrationCandidates(RegistryName);
                return Ok(res);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return BadRequest($"{GetMsg(ex)}");
            }
        }
        [HttpGet("RejectRegistration")]
        public ActionResult<Tuple<bool, string>> RejectRegistration(string RegistryName, Guid InstanceId)
        {
            try
            {
                var wm = GetWatcherManager();
                var res = wm.RejectRegistration(RegistryName, InstanceId);
                return Ok(res);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return BadRequest($"{GetMsg(ex)}");
            }
        }
        [HttpGet("AcceptRegistration")]
        public ActionResult<Tuple<bool, string>> AcceptRegistration(string RegistryName, Guid InstanceId)
        {
            try
            {
                var wm = GetWatcherManager();
                var res = wm.AcceptRegistration(RegistryName, InstanceId);
                return Ok(res);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return BadRequest($"{GetMsg(ex)}");
            }
        }
        #endregion

        #region Error And Logs
        [HttpPost("ReportError")]
        public async Task<ActionResult<bool>> ReportError(mLogT ml)
        {
            try
            {
                var wm = GetWatcherManager();
                var res = await wm.SaveLog(ml, eLogType.Error.ToString());
                return Ok(res);


            }
            catch (Exception ex)
            {
                LogError(ex);
                return BadRequest($"{GetMsg(ex)}");

            }
        }

        [HttpPost("ReportEvent")]
        public async Task<ActionResult<bool>> ReportEvent(mLogT ml)
        {
            try
            {
                var wm = GetWatcherManager();
                var res = await wm.SaveLog(ml, eLogType.Event.ToString());
                return Ok(res);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return BadRequest($"{GetMsg(ex)}");

            }
        }

        [HttpGet("GetMessageLogs")]
        public ActionResult<List<mLogT>> GetMessageLogs()
        {
            try
            {
                var wm = GetWatcherManager();
                var res = wm.GetLogData();
                return Ok(res);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return BadRequest($"{GetMsg(ex)}");
            }
        }
        [HttpGet("GetLogsEvent")]
        public ActionResult<List<mLogT>> GetLogsEvent()
        {
            try
            {
                var wm = GetWatcherManager();
                var res = wm.GetLogEventList();
                _eventHub.Clients.All.SendAsync("SendLogEventData", res);
                return Ok(res);
                //naveen     //return Ok(new { Message = "Request Completed Successfully" });
            }
            catch (Exception ex)
            {
                LogError(ex);
                return BadRequest($"{GetMsg(ex)}");
            }
        }
        [HttpGet("GetLogsError")]
        public ActionResult<List<mLogT>> GetLogsError()
        {
            try
            {
                var wm = GetWatcherManager();
                var res = wm.GetLogErrorList();
                _errorHub.Clients.All.SendAsync("SendLogErrorData", res);
                return Ok(new { Message = "Request Completed Successfully" });
            }
            catch (Exception ex)
            {
                LogError(ex);
                return BadRequest($"{GetMsg(ex)}");
            }
        }
        [HttpGet("GetLogs")]
        public ActionResult<List<mLogT>> GetLogs()
        {
            try
            {
                var wm = GetWatcherManager();
                var res = wm.GetLogList();
                _logHub.Clients.All.SendAsync("SendLogData", res);
                return Ok(new { Message = "Request Completed Successfully" });


            }
            catch (Exception ex)
            {
                LogError(ex);
                return BadRequest($"{GetMsg(ex)}");
            }

        }

        #endregion

        #region Private
        private WatcherManager GetWatcherManager()
        {
            var result = new WatcherManager();
            //result.dbctx = ctx;
            result.httpContext = httpContext;
            return result;
        }

        #endregion
    }
}
