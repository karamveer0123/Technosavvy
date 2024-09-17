using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace NavExM.Int.Maintenance.APIs.Controllers;

public class sControllerBase : ControllerBase
{
    internal ContentAppContext cctx;
        internal CareerAppContext jddbctx;
    internal ApiAppContext ctx;
    internal PreBetaDBContext pbctx;
    internal EventAppContext ectx;
    internal RewardAppContext rctx;
    //internal ILogger logger;
    internal HttpContext httpContext;
    internal IOptions<SmtpConfig> smtp;
    public sControllerBase()
    {
        //ToDo: Naveen Create Geo Info
        //ToDo: Check Client Certificate to ensure no external client called.
    }
    protected bool EnsureValidSession()
    {
        /* 1. Session must be Valid
         * 2. Request Frequency must be within allowed limit
         * 3. Controller is for External User
         */
#if (DEBUG)
        return true;
#else
        throw new NotImplementedException("Ensure Valid Session is not yet done");
#endif
    }

}
