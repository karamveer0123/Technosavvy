using Microsoft.AspNetCore.Identity;
using NavExM.Int.Watcher.WatchDog.Data;

namespace NavExM.Int.Watcher.WatchDog.Manager
{
    public class ManagerBase
    {
        //internal ApiAppContext dbctx;
        internal HttpContext httpContext;

        internal protected Tuple<bool, string> Ok(bool result, string Msg)
        {
            return new Tuple<bool, string>(result, Msg);
        }
        internal protected void ThrowNullArgumentException(object? obj)
        {
            throw new ArgumentNullException($"{nameof(obj)} can't be null");
        }
        internal protected void ThrowInvalidOperationException(string msg = "")
        {
            if (msg.IsNullorEmpty())
                throw new InvalidOperationException("Mandatory field value was null. operation Aborted..");
            throw new InvalidOperationException(msg);
        }
        public string GetSessionHash()
        {
            //ToDo: Naveen Get this SessionHash fromRequest header
            return httpContext.Request.Headers["SessionToken"];
        }
        internal string GetSignedHash(string str)
        {
            //ToDo: Naveen, SignedHash should be implemented with AppSeed Private Key of Application Instances, Public Key would be communicated to Watcher for Hash Verification
            var a = new PasswordHasher<object>(optionsAccessor: default);
            var res = a.HashPassword(Guid.Empty, str);
            return res;
        }
        internal string GetHash(string str)
        {
            var a = new PasswordHasher<object>(optionsAccessor: default);
            var res = a.HashPassword(Guid.Empty, str);
            return res;
        }
        internal PasswordVerificationResult CompareHash(string Hpwd, string pwd)
        {
            var a = new PasswordHasher<object>(optionsAccessor: default);
            var res = a.VerifyHashedPassword(Guid.Empty, Hpwd, pwd);
            return res;
        }
        private string GetMsg(Exception ex)
        {
            if (ex is null) return string.Empty;
            var msg = $"{ex.Message}{GetMsg(ex.InnerException)}";
            return msg;
        }

    }
}
