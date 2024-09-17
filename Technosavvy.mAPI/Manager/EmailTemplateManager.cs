using static System.Net.WebRequestMethods;

namespace NavExM.Int.Maintenance.APIs.Manager
{
    public class EmailTemplateManager
    {
        /// <summary>
        /// this function work for get opt template for directory
        /// </summary>
        /// <returns></returns>
        private static string GetPreBetaOtpTemplate()
        {
            string body = string.Empty;
            var getDirectory = Directory.GetCurrentDirectory();
            getDirectory = getDirectory + "/Template/t6.html";
            StreamReader reader = new StreamReader(getDirectory);
            body = reader.ReadToEnd();
            return body;
        }
        internal static string GetPreBetaNavCBuySuggestionAfterSignUpTemplate(string uName)
        {
            string body = string.Empty;
            var getDirectory = Directory.GetCurrentDirectory();
            getDirectory = getDirectory + "/Template/t7.html";
            StreamReader reader = new StreamReader(getDirectory);
            body = reader.ReadToEnd();
            body = body.Replace("{##userName##}", uName);
            return body;
        }
        internal static string GetPreBeta500NavCBuyTemplate(string uName)
        {
            string body = string.Empty;
            var getDirectory = Directory.GetCurrentDirectory();
            getDirectory = getDirectory + "/Template/t8A.html";
            StreamReader reader = new StreamReader(getDirectory);
            body = reader.ReadToEnd();
            body = body.Replace("{##userName##}", uName);
            return body;
        }
        internal static string GetPreBeta5000NavCBuyTemplate(string uName)
        {
            string body = string.Empty;
            var getDirectory = Directory.GetCurrentDirectory();
            getDirectory = getDirectory + "/Template/t8B.html";
            StreamReader reader = new StreamReader(getDirectory);
            body = reader.ReadToEnd();
            body = body.Replace("{##userName##}", uName);
            return body;
        }
        internal static string GetPreBeta50000NavCBuyTemplate(string uName)
        {
            string body = string.Empty;
            var getDirectory = Directory.GetCurrentDirectory();
            getDirectory = getDirectory + "/Template/t8C.html";
            StreamReader reader = new StreamReader(getDirectory);
            body = reader.ReadToEnd();
            body = body.Replace("{##userName##}", uName);
            return body;
        }
        internal static string GetPreBetaExceedingNavCBuyTemplate(string uName, string Amount)
        {
            string body = string.Empty;
            var getDirectory = Directory.GetCurrentDirectory();
            getDirectory = getDirectory + "/Template/t10.html";
            StreamReader reader = new StreamReader(getDirectory);
            body = reader.ReadToEnd();
            body = body.Replace("{##userName##}", uName).Replace("{##Amount##}", Amount);
            return body;
        }
        internal static string GetPreBetaLessThanNavCBuyTemplate(string uName, string Amount)
        {
            string body = string.Empty;
            var getDirectory = Directory.GetCurrentDirectory();
            getDirectory = getDirectory + "/Template/t11.html";
            StreamReader reader = new StreamReader(getDirectory);
            body = reader.ReadToEnd();
            body = body.Replace("{##userName##}", uName).Replace("{##Amount##}", Amount);
            return body;
        }
        internal static string GetPreBetaPurchasesTemplate(string uName, string Amount, string Txhash, DateTime dt, string paidWith)
        {
            string body = string.Empty;
            var getDirectory = Directory.GetCurrentDirectory();
            getDirectory = getDirectory + "/Template/t2.html";
            StreamReader reader = new StreamReader(getDirectory);
            body = reader.ReadToEnd();
            body = body.Replace("{##userName##}", uName).Replace("{##PurchaseOf##}", $"{Amount} NavC").Replace("{##TxHash##}", Txhash).Replace("{##Date##}", dt.ToString("dd MMMM yyyy")).Replace("{##PaidWith##}", paidWith);
            return body;
        }
        internal static string GetPreBetaPasswordResetConfirmationTemplate(string uName)
        {
            string body = string.Empty;
            var getDirectory = Directory.GetCurrentDirectory();
            getDirectory = getDirectory + "/Template/t1.html";
            StreamReader reader = new StreamReader(getDirectory);
            body = reader.ReadToEnd();
            body = body.Replace("{##userName##}", uName);
            return body;
        }
        internal static string GetPreBetaSignupTemplate(string uName)
        {
            string body = string.Empty;
            var getDirectory = Directory.GetCurrentDirectory();
            getDirectory = getDirectory + "/Template/t3.html";
            StreamReader reader = new StreamReader(getDirectory);
            body = reader.ReadToEnd();
            body = body.Replace("{##userName##}", uName);

            return body;
        }

        /// <summary>
        /// this function work for map otp and other urls for template
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="OTP"></param>
        /// <param name="ReferralUrl"></param>
        /// <param name="AirdropUrl"></param>
        /// <param name="homeUrl"></param>
        /// <returns></returns>
        public static string GetOTPEventTemplate(string UserName, string otp)
        {
            var referralUrl = mAppConstant.NavExMSite + "/en/home/ReferralProgram";
            var airdropUrl = mAppConstant.NavExMSite + "/en/home/AirdropProgram";
            var homeUrl = mAppConstant.NavExMSite + "/en/home/Index";
            var template = GetPreBetaOtpTemplate();
            string body = template.Replace("{##userName##}", UserName).Replace("{##PassCode##}", otp).Replace("{##referral##}", referralUrl).Replace("{##airdrop##}", airdropUrl).Replace("{##navexmHome##}", homeUrl);

            return body;
        }

        /// <summary>
        /// This function work for get html
        /// </summary>
        /// <returns></returns>
        private static string GetForgetOtpTemplate()
        {
            string body = string.Empty;
            var getDirectory = Directory.GetCurrentDirectory();
            getDirectory = getDirectory + "/Template/ForgetPass.html";
            //var FilePath = Path.Combine(getDirectory, "/wwwroot/Template/index.html");
            StreamReader reader = new StreamReader(getDirectory);
            body = reader.ReadToEnd();
            return body;
        }
        // this function work for replace value 
        public static string GetOTPForgetTem(string UserName, string OTP)
        {
            var template = GetForgetOtpTemplate();
            string body = template.Replace("{##userName##}", UserName).Replace("{##PassCode##}", OTP);
            //.Replace("{##Date##}", user.AccountActivationExpiryTime.HasValue ?
            //    user.AccountActivationExpiryTime.Value.ToString("dd/MMMM/yyyy dd:mm")
            //: string.Empty);
            return body;
        }


        internal static string GetSignupTemplate(string UserName)
        {
            var referralUrl = mAppConstant.NavExMSite + "/en/home/ReferralProgram";
            var airdropUrl = mAppConstant.NavExMSite + "/en/home/AirdropProgram";
            var homeUrl = mAppConstant.NavExMSite + "/en/home/Index";

            var getDirectory = Directory.GetCurrentDirectory();
            getDirectory = getDirectory + "/Template/PasswordSuccess.html";
            StreamReader reader = new StreamReader(getDirectory);
            var template = reader.ReadToEnd();

            string body = template.Replace("{##userName##}", UserName).Replace("{##referral##}", referralUrl).Replace("{##airdrop##}", airdropUrl).Replace("{##navexmHome##}", homeUrl);
            return body;
        }

        public static string GetPasswordResetEventTemplate(string UserName)
        {
            if (ConfigEx.VersionType == versionType.PreBeta)
            {
                return GetPreBetaPasswordResetConfirmationTemplate(UserName);
            }
            else
            {
                //ToDo: Beta should have its own template
                return GetPreBetaPasswordResetConfirmationTemplate(UserName);
            }
        }
        public static string GetSignUpCompleteEventTemplate(string UserName)
        {
            if (ConfigEx.VersionType == versionType.PreBeta)
            {
               return  GetPreBetaSignupTemplate(UserName);
            }
            else
            {
                return GetSignupTemplate(UserName);
            }
        }
    }
}
