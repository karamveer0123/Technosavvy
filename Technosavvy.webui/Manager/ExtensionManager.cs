using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace TechnoApp.Ext.Web.UI.Manager
{
    public static partial class ExtensionManager
    {
        /// <summary>
        /// Trim all white spaces and return name in lower case
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string ToLowerCaseTrimWhiteSpace(this string name)
        {
            return !string.IsNullOrEmpty(name) ? name.Replace(" ", "").Trim().ToLower() : name;

        }

        //public static bool NotIsNullOrEmpty(this string strIn)
        //{
        //    return (!string.IsNullOrEmpty(strIn));
        //}
        //public static bool IsNullOrEmpty(this string strIn)
        //{
        //    return (string.IsNullOrEmpty(strIn));
        //}
        //public static bool IsGuidNullorEmpty(this Guid guidId)
        //{
        //    return guidId == Guid.Empty;
        //}

        public static bool IsGuidNullorEmpty(this Guid? guidId)
        {
            if (!guidId.HasValue)
                return true;
            Guid? nullable = guidId;
            Guid empty = Guid.Empty;
            if (!nullable.HasValue)
                return false;
            return !nullable.HasValue || nullable.GetValueOrDefault() == empty;
        }

        public static bool DateTimeNullOrEmpty(this DateTime? dateTime)
        {
            if (dateTime.HasValue)
            {
                DateTime? nullable = dateTime;
                DateTime dateTime1 = new DateTime();
                if ((nullable.HasValue ? (nullable.HasValue ? (nullable.GetValueOrDefault() == dateTime1 ? 1 : 0) : 1) : 0) == 0)
                    return false;
            }
            return true;
        }

        public static bool DateTimeNullOrEmpty(this DateTime dateTime)
        {
            return dateTime == new DateTime();
        }
        public static Guid ToGuid(this string guid)
        {
            if (guid.IsNullOrEmpty()) return Guid.Empty;
            return Guid.Parse(guid);
        }
        public static Guid ToGuid(this Guid? source)
        {
            return source ?? Guid.Empty;
        }
        public static void MessageToaster(Controller controller, string MessageTitle, string MessageBody, string MessageType = "success", string OptionalUrl = "")
        {
            ToastMsgVM toastMsg = new ToastMsgVM()
            {
                MsgType = MessageType,
                MsgTitle = MessageTitle,
                MsgBody = MessageBody
            };
            controller.TempData["ToastMsg"] = ToJSON(toastMsg);



            //message type = warning,info,error,success
        }
        public static string ToJSON(object obj)
        {
            return JsonSerializer.Serialize(obj);
        }
    }



}
