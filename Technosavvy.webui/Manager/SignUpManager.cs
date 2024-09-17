using TechnoApp.Ext.Web.UI.Service;

namespace TechnoApp.Ext.Web.UI.Manager
{
    public class SignUpManager : MaintenanceSvc
    {
        //User Signup Related Functions should be here
        public async Task<Tuple<Guid, bool>> NewUserCheckAndSendEmailOtp(string uName,string RefCode="")
        {
            var id = Guid.Empty;
            var OTPStatus = false;
            var result = await GetAnyUser(uName);
            if (result == false)
            {
                id = await RegisterUser(uName,RefCode);
                if (!id.IsGuidNullorEmpty())
                {
                    OTPStatus = await RequestEmailOTP(uName);
                }
            }
            return new Tuple<Guid, bool>(id, OTPStatus);
        }
        //This function work for check user and send otp for forget user
        public async Task<bool> UserCheckAndSendEmailOtp(string uName)
        {

            var OTPStatus = false;
            var result = await GetAnyUser(uName);
            if (result == true)
            {
                OTPStatus = await RequestForgetEmailOTP(uName);
            }
            return OTPStatus;
        }

        //This function work for check user and Resend otp for user
        public async Task<bool> UserCheckAndReSendEmailOtp(string uName)
        {

            var OTPStatus = false;
            var result = await GetAnyUser(uName);
            if (result == true)
            {
                OTPStatus = await RequestEmailOTP(uName);
            }
            return OTPStatus;
        }

        public async Task<bool> NewUserCheckOTPAndConfirmForPassword(string uName, string OTP)
        {
            var result = await VerifyEmailOTP(uName, OTP);
            if (result)
                result = await CanSetUserPassword(uName);
            return result;
        }
    }
}
