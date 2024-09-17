using Microsoft.AspNetCore.Mvc;
using TechnoApp.Ext.Web.UI.Models;
using System.Text.Json;

namespace TechnoApp.Ext.Web.UI.Manager
{
    public class GUtilityManager
    {
        public static string RandomString(int stringlength, int numberLength)
        {

          
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string number = "0123456789";
            var s1 = new string(Enumerable.Repeat(chars, stringlength).Select(s => s[random.Next(s.Length)]).ToArray());
            var s2 = new string(Enumerable.Repeat(number, numberLength).Select(s => s[random.Next(s.Length)]).ToArray());
            return s1 + s2;
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
        public static T FromJSON<T>(string obj)
        {
            return JsonSerializer.Deserialize<T>(obj);
        }

        public bool IsRefererExixts(HttpContext context)
        {
            bool isExists = context.Request.Headers.ContainsKey("Referer") ? true : false;
            return isExists;
        }

        public static string GetSessionAttr(HttpContext context)
        {
            var retval = JsonSerializer.Serialize(context.Request.Headers);
            if (retval.Length >= 10000)
                retval = retval.Substring(0, 10000);
            return retval;
            //var reomte = context.Request.Headers;
            //List<string> lst = new List<string>();
            //foreach (var h in reomte)
            //{
            //    lst.Add($"{h.Key}...{h.Value}");
            //}
            //var lstxml = lst.Serialize(); // To check Length
            //return lstxml;
        }

    }



}
