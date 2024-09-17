using System.Runtime.CompilerServices;

namespace NavExM.Int.Watcher.WatchDog.Extention
{
    public static partial class Ext
    {
        #region Func Implementations
        public static bool IsNullorEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }
        public static Guid GVC(this Guid? gId)
        {
            if (gId.HasValue) return gId.Value;
            return Guid.Empty;
        }
        public static string GetDeepMsg(this Exception ex)
        {
            if (ex is null) return string.Empty;
            var msg = $"{ex.Message}{ex.InnerException.GetDeepMsg()}";
            return msg;
        }
        public static void CheckAndThrowNullArgumentException(this string? obj)
        {
            if (obj != null && obj.IsNOT_NullorEmpty()) return;
            throw new ArgumentNullException($"{nameof(obj)} can't be null");
        }
        public static void CheckAndThrowNullArgumentException(this int? obj)
        {
            throw new ArgumentNullException($"{nameof(obj)} can't be null or zero");
        }
        public static void CheckAndThrowNullArgumentException(this int obj)
        {
            throw new ArgumentNullException($"{nameof(obj)} can't be null or zero");
        }
        public static void CheckAndThrowNullArgumentException(this double? obj)
        {
            throw new ArgumentNullException($"{nameof(obj)} can't be null or zero");
        }
        public static void CheckAndThrowNullArgumentException(this double obj)
        {
            if (obj != null && obj > 0) return;
            throw new ArgumentNullException($"{nameof(obj)} can't be null or zero");
        }
        public static void CheckAndThrowNullArgumentException(this Guid obj)
        {
            if (obj != Guid.Empty) return;
            throw new ArgumentNullException($"{nameof(obj)} can't be null");
        }
        public static void CheckAndThrowNullArgumentException(this Guid? obj)
        {
            if (obj.HasValue) return;
            throw new ArgumentNullException($"{nameof(obj)} can't be null");
        }
        public static void CheckAndThrowNullArgumentException(this object obj)
        {
            if (obj is not null) return;
            throw new ArgumentNullException($"{nameof(obj)} can't be null");
        }
        public static void ThrowInvalidOperationException(this string msg, [CallerMemberName] string mName = "")
        {
            if (msg.IsNullorEmpty())
                msg = $"Invalid request for operation{mName}. Operation Aborted..";
            throw new InvalidOperationException(msg);
        }
        public static void CheckSignIdAndThrowOperException(this Guid me, string msg, [CallerMemberName] string mName = "")
        {
            if (me != Guid.Empty) return;
            if (msg.IsNullorEmpty())
                msg = $"Invalid request for operation{mName}. Operation Aborted..";
            throw new InvalidOperationException(msg);
        }
        public static void ThrowInvalidOperationException(this object obj, string msg = "", [CallerMemberName] string mName = "")
        {
            if (msg.IsNullorEmpty())
                msg = $"Invalid request for operation{mName}. Operation Aborted..";
            msg = $"{msg}.\"{mName}\" Operation Aborted..";
            throw new InvalidOperationException(msg);
        }
        public static bool IsNOT_NullorEmpty(this string str)
        {
            return !string.IsNullOrEmpty(str);
        }
        public static Guid ToGUID(this string str)
        {
            var g = Guid.Empty;
            if (!string.IsNullOrEmpty(str))
                Guid.TryParse(str, out g);

            return g;
        }
        public static T Check<T>(this T e) where T : class, new()
        {
            if (e is null)
                return new T();
            else
                return e;
        }
        #endregion

      
    }
}
