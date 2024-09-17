using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using NavExM.Int.Maintenance.APIs.Services;
using NuGet.Protocol;
using System.Text.RegularExpressions;

namespace NavExM.Int.Maintenance.APIs.Manager
{
    internal class UserManager : ManagerBase
    {
        internal async Task<Tuple<DateTime, int, int>> GetUserCount()
        {
            var c = dbctx.UserAccount.Count();
            var cm = dbctx.UserAccount.Where(x => x.IsPrimaryCompleted).Count();
            return Tuple.Create(DateTime.UtcNow, c, cm);
        }
        internal bool IsAny(string uName)
        {
            if (string.IsNullOrEmpty(uName)) return false;
            var id = uName.ToLower();
            var result = dbctx.UserAccount.FirstOrDefault(x => x.AuthEmail.Email.CompareTo(id) == 0);
            return result != null;
        }
        internal bool IsTradingAllowed(string uName)
        {
            Console2.WriteLine_RED("ToDo:Naveen Check if User is allowed for trading.(Current Status for any reason)");
            return true;
        }
        internal bool IsWithdrawAllowed(string uName)
        {
            Console2.WriteLine_RED("ToDo:Naveen Check if User is allowed to Withdraw Funds from NavExM.(Current Status for any reason)");
            return true;
        }
        internal Guid GetUserAccountId(string userAccount)
        {
            if (userAccount.IsNullorEmpty()) return Guid.Empty;
            userAccount = userAccount.ToLower();
            var usr = dbctx.UserAccount.FirstOrDefault(x => x.FAccountNumber ==
userAccount);

            if (usr == null) return Guid.Empty;
            return usr.UserAccountId;
        }
        internal eRefCodes GetUserRefCodes(string userAccount)
        {
            var ret = new eRefCodes();
            if (userAccount.IsNullorEmpty()) return ret;
            userAccount = userAccount.ToLower();
            var usr = dbctx.UserAccount.FirstOrDefault(x => x.FAccountNumber ==
userAccount);

            if (usr == null) return ret;
            ret = usr.RefCode;
            return ret;
        }
        internal eUserAccount GetUserOfCommunity(string RefCode)
        {
            if (RefCode.IsNullorEmpty()) return null;
            var id = RefCode.ToLower();
            var result = dbctx.UserAccount
                .Include(x => x.UserProfile)
                .Include(x => x.AuthEmail)
                .Include(x => x.SecurePassword)
                .Include(x => x.Authenticator)
                .Include(x => x.TaxResidency)
                .Include(x => x.Mobile)
                .Include(x => x.EmailValidationActions)
                .Include(x => x.SpotWallet)
                .Include(x => x.FundingWallet)
                .Include(x => x.EscrowWallet)
                .Include(x => x.EarnWallet)
                .Include(x => x.HoldingWallet)
                .FirstOrDefault(x => x.RefCode.myCommunity.ToLower()==RefCode);
            if (result != null && result.TaxResidency != null)
            {
                result.TaxResidency = dbctx.TaxResidency
                    .Include(x => x.Country).FirstOrDefault(x => x.TaxResidencyId == result.TaxResidency.TaxResidencyId);
            }
            if (result?.UserProfile != null)
                result.UserProfile.UserAccount = result;
            return result;
        }
        internal eUserAccount GetUser(string uName)
        {
            if (uName.IsNullorEmpty()) return null;
            var id = uName.ToLower();
            var result = dbctx.UserAccount
                .Include(x => x.UserProfile)
                .Include(x => x.AuthEmail)
                .Include(x => x.SecurePassword)
                .Include(x => x.Authenticator)
                .Include(x => x.TaxResidency)
                .Include(x => x.Mobile)
                .Include(x => x.EmailValidationActions)
                .Include(x => x.SpotWallet)
                .Include(x => x.FundingWallet)
                .Include(x => x.EscrowWallet)
                .Include(x => x.EarnWallet)
                .Include(x => x.HoldingWallet)
                .FirstOrDefault(x => x.AuthEmail.Email.CompareTo(id) == 0);
            if (result != null && result.TaxResidency != null)
            {
                result.TaxResidency = dbctx.TaxResidency
                    .Include(x => x.Country).FirstOrDefault(x => x.TaxResidencyId == result.TaxResidency.TaxResidencyId);
            }
            if (result?.UserProfile != null)
                result.UserProfile.UserAccount = result;
            return result;
        }
        internal eUserAccount GetUserOfAcc(string uAcc)
        {
            if (uAcc.IsNullorEmpty()) return null;
            var id = uAcc.ToLower();
            var result = dbctx.UserAccount
                .Include(x => x.UserProfile)
                .Include(x => x.AuthEmail)
                .Include(x => x.SecurePassword)
                .Include(x => x.Authenticator)
                .Include(x => x.TaxResidency)
                .Include(x => x.Mobile)
                .Include(x => x.EmailValidationActions)
                .Include(x => x.SpotWallet)
                .Include(x => x.FundingWallet)
                .Include(x => x.EscrowWallet)
                .Include(x => x.EarnWallet)
                .Include(x => x.HoldingWallet)
                .FirstOrDefault(x => x.FAccountNumber.CompareTo(id) == 0);
            if (result != null && result.TaxResidency != null)
            {
                result.TaxResidency = dbctx.TaxResidency
                    .Include(x => x.Country).FirstOrDefault(x => x.TaxResidencyId == result.TaxResidency.TaxResidencyId);
            }
            if (result?.UserProfile != null)
                result.UserProfile.UserAccount = result;
            return result;
        }

        internal mUser GetUserM(Guid id)
        {
            var usr = GetUserbyId(id);
            if (usr is null) return null;
            return usr.ToModel();
        }
        internal eUserAccount GetUserbyId(Guid id)
        {
            if (id == Guid.Empty) return null;
            var result = dbctx.UserAccount
                .Include(x => x.UserProfile)
                .Include(x => x.AuthEmail)
                .Include(x => x.SecurePassword)
                .Include(x => x.Authenticator)
                .Include(x => x.TaxResidency)
                .Include(x => x.Mobile)
                .Include(x => x.EmailValidationActions)
                 .Include(x => x.SpotWallet)
                .Include(x => x.FundingWallet)
                .Include(x => x.EscrowWallet)
                .Include(x => x.EarnWallet)
                .Include(x => x.HoldingWallet)
                .FirstOrDefault(x => x.UserAccountId == id);
            if (result != null && result.TaxResidency != null)
            {
                result.TaxResidency = dbctx.TaxResidency
                    .Include(x => x.Country).FirstOrDefault(x => x.TaxResidencyId == result.TaxResidency.TaxResidencyId);
            }

            if (result?.UserProfile != null)
                result.UserProfile.UserAccount = result;
            if (result is null) return null;
            return result;
        }
        internal Guid GetTaxResidencyOf(string userAccount)
        {
            var result = dbctx.UserAccount
                 .Include(x => x.TaxResidency)
                .FirstOrDefault(x => x.FAccountNumber == userAccount);
            if (result != null && result.TaxResidency != null)
            {
                result.TaxResidency = dbctx.TaxResidency
                    .Include(x => x.Country).FirstOrDefault(x => x.TaxResidencyId == result.TaxResidency.TaxResidencyId);
                return result.TaxResidency!.Country.CountryId;
            }
            else
            {
                //MM Order will Assume IN Tax Residency, since it is not Exculded
                if (userAccount.StartsWith("MM"))
                    return dbctx.Country.First(x => x.Abbrivation == "IN").CountryId;
                return Guid.Empty;

            }
        }
        internal Guid GetSpotWalletOf(string userAccount)
        {
            var result = dbctx.UserAccount
                 .Include(x => x.SpotWallet)
                .FirstOrDefault(x => x.FAccountNumber == userAccount);
            if (result != null)
                return result!.SpotWalletId!.Value;
            else
                return Guid.Empty;
        }
        internal Guid GetEarnWalletOf(string userAccount)
        {
            var result = dbctx.UserAccount
                 .Include(x => x.EarnWallet)
                .FirstOrDefault(x => x.FAccountNumber == userAccount);
            if (result != null)
                return result!.EarnWalletId!.Value;
            else
                return Guid.Empty;
        }
        internal Guid GetFundWalletOf(string userAccount)
        {
            var result = dbctx.UserAccount
                 .Include(x => x.FundingWallet)
                .FirstOrDefault(x => x.FAccountNumber == userAccount);
            if (result != null)
                return result!.FundingWalletId!.Value;
            else
                return Guid.Empty;
        }
        internal eUserAccount GetUserForNetworkWalletbyId(Guid id)
        {
            if (id == Guid.Empty) return null;
            var result = dbctx.UserAccount
                .Include(x => x.UserProfile)
                .Include(x => x.AuthEmail)
                .Include(x => x.FundingWallet)
                .ThenInclude(x => x.myNetworkWallets)
                .ThenInclude(x => x.NetworkWalletAddress)
                .ThenInclude(x => x.Network)
                .FirstOrDefault(x => x.UserAccountId == id && x.FundingWallet.myNetworkWallets.Count > 0);

            if (result is null) return null;
            return result;
        }
        internal Guid AddUserAccount(string uName, string refCode = "")
        {
            if (ValidateUserName(uName) && !IsAny(uName))
            {
                var email = new eAuthorizedEmail() { Email = uName.ToLower() };
                var Code = new eRefCodes
                {
                    RefferedBy = refCode,
                    myCommunity = Guid.NewGuid().ToString().Replace("-", string.Empty).ToUpper()
                };

                var user = new eUserAccount
                {
                    AuthEmail = email,
                    RefCode = Code
                };

                dbctx.UserAccount.Add(user);
                dbctx.SaveChanges();
                if (refCode.IsNullorEmpty())
                    return user.UserAccountId;

                Rewardctx.SignUpUsers.Add(new SignUpUser
                {
                    RefCode = refCode,
                    UserAccountId = user.UserAccountId,
                });
                Rewardctx.SaveChanges();
                return user.UserAccountId;
            }
            return Guid.Empty;
        }
        internal bool SendEmailOTPForgetPassword(string uName, IOptions<SmtpConfig> smtp, HttpContext http)
        {
            var user = GetUser(uName);
            if (user == null) return false;
            var otp = GeneratetOTP();
            var otpH = GetHash(otp);
            var eval = new eEmailValidationAction() { UserAccount = user!, OTPHash = otpH, GEOInfo = GetGeoInfo() };
            dbctx.EmailValidationAction.Add(eval);
            dbctx.SaveChanges();


            //Send Email
            var Htmbody = EmailTemplateManager.GetOTPEventTemplate(uName, otp);
            mEmail email = new mEmail()
            {
                To = uName,
                Subject = "OTP to verify Email",
                Body = Htmbody
            };

            new EmailManager(smtp).SendEmail(email);
            return true;

        }
        internal bool SendEmailFor500Signup(string uName, SmtpConfig smtp)
        {
            var Htmbody = EmailTemplateManager.GetPreBeta500NavCBuyTemplate(uName);
            mEmail email = new mEmail()
            {
                To = uName,
                Subject = "Welcome to NavExM Community",
                Body = Htmbody
            };
            return new EmailManager(smtp).SendEmail(email);
        }
        internal bool SendEmailFor5000Signup(string uName, SmtpConfig smtp)
        {
            var Htmbody = EmailTemplateManager.GetPreBeta5000NavCBuyTemplate(uName);
            mEmail email = new mEmail()
            {
                To = uName,
                Subject = "Welcome to NavExM Community",
                Body = Htmbody
            };
            return new EmailManager(smtp).SendEmail(email);
        }
        internal bool SendEmailFor50000Signup(string uName, SmtpConfig smtp)
        {
            var Htmbody = EmailTemplateManager.GetPreBeta50000NavCBuyTemplate(uName);
            mEmail email = new mEmail()
            {
                To = uName,
                Subject = "Welcome to NavExM Community",
                Body = Htmbody
            };
            return new EmailManager(smtp).SendEmail(email);
        }
        internal bool SendEmailForExceedBuyLimit(string uName, string Amount, SmtpConfig smtp)
        {
            var Htmbody = EmailTemplateManager.GetPreBetaExceedingNavCBuyTemplate(uName, Amount);
            mEmail email = new mEmail()
            {
                To = uName,
                Subject = "Received Access Funds",
                Body = Htmbody
            };
            return new EmailManager(smtp).SendEmail(email);
        }
        internal bool SendEmailForLessThanLimitBuyLimit(string uName, string Amount, SmtpConfig smtp)
        {
            var Htmbody = EmailTemplateManager.GetPreBetaLessThanNavCBuyTemplate(uName, Amount);
            mEmail email = new mEmail()
            {
                To = uName,
                Subject = "Received Less than 20 USDT",
                Body = Htmbody
            };
            return new EmailManager(smtp).SendEmail(email);
        }
        internal bool SendEmailPreBetaPurchasesSignup(string uName, string Amt, string PaidWith, string TxHash, DateTime ConfirmedOn, SmtpConfig smtp)
        {
            var Htmbody = EmailTemplateManager.GetPreBetaPurchasesTemplate(uName, Amt, TxHash, ConfirmedOn, PaidWith);
            mEmail email = new mEmail()
            {
                To = uName,
                Subject = $"Your have Secured {Amt} NavC",
                Body = Htmbody
            };
            return new EmailManager(smtp).SendEmail(email);
        }
        internal bool SendEmailPasswordReset(string uName, IOptions<SmtpConfig> smtp)
        {
            var Htmbody = EmailTemplateManager.GetPasswordResetEventTemplate(uName);
            mEmail email = new mEmail()
            {
                To = uName,
                Subject = $"Password Reset Confirmation",
                Body = Htmbody
            };
            return new EmailManager(smtp).SendEmail(email);
        }
        internal bool SendEmailOTP(string uName, IOptions<SmtpConfig> smtp, HttpContext http)
        {
            var user = GetUser(uName);
            if (user == null) return false;
            var otp = GeneratetOTP();
            //ApiAppContext.AllItems.Add($"{Guid.NewGuid}-OTP", otp);
            var otpH = GetHash(otp);
            var eval = new eEmailValidationAction() { UserAccount = user!, OTPHash = otpH, GEOInfo = GetGeoInfo() };
            dbctx.EmailValidationAction.Add(eval);
            dbctx.SaveChanges();

            //Send Email
            var Htmbody = EmailTemplateManager.GetOTPEventTemplate(uName, otp);
            mEmail email = new mEmail()
            {
                To = uName,
                Subject = "OTP to verify Email ",
                Body = Htmbody
            };
            return new EmailManager(smtp).SendEmail(email);
            //  return true;

        }
        internal void ForKYC()
        {
            //ToDo: KYC Related function
        }
        internal void UpdateUserProfile(string uName)
        {
            //ToDo: KYC Related function

        }
        internal bool SuspendMeAutoFor(string Msg, DateTime till)
        {
            /* Get Current Session User
             * set User.IsActive=False
             * Create UserAccountAuthenticationLog Record with
             *  -ResultedAutoLock=True
             *  -AutoLockExpierOn=till
             */
            return false;
        }
        internal bool SuspendMeForStaffIntervention(string Msg)
        {
            /* Get Current Session User
             * set User.IsActive=False
             * Create UserAccountAuthenticationLog Record with
             *  -ManualInterventionMandated=True
             */
            return false;
        }
        internal bool SignOut()
        {
            var t = GetSessionHash();
            var s = dbctx.UserSession.FirstOrDefault(x => x.SessionHash == t);
            if (s is null) throw new ArgumentException("Invalid Session Id for User to SignOut...");
            if (s.ExpieredOn == null || s.ExpieredOn >= DateTime.UtcNow)
                s.ExpieredOn = DateTime.UtcNow;
            dbctx.SaveChanges();
            SrvSessionManagement.LogOutSession(t);

            return true;
        }
        internal bool SignOutAllDevices()
        {
            var t = GetSessionHash();
            var s = dbctx.UserSession.FirstOrDefault(x => x.SessionHash == t);
            if (s is null) throw new ArgumentException("Invalid Session Id for User to SignOut...");
            if (s.ExpieredOn == null || s.ExpieredOn >= DateTime.UtcNow)
                s.ExpieredOn = DateTime.UtcNow;
            dbctx.SaveChanges();
            SrvSessionManagement.LogOutAllSession(t);

            return true;
        }
        internal mUserSession Report2ndFactor(Guid authID, string uName, string mode)
        {
            mUserSession result = null;

            var auth = dbctx.AuthenticationEvent
                 .Include(x => x.UserAccount)
                 .FirstOrDefault(x => x.AuthenticationEventId == authID && x.UserAccount.AuthEmail!.Email == uName);

            if (auth is null || mode.IsNullorEmpty()) throw new ArgumentException("Invalid auth ID or Mode");
            if (!auth.IsMultiFactor) throw new InvalidOperationException("User is not Configured for MultiFactor");
            if (auth.CreatedOn.AddMinutes(5) <= DateTime.UtcNow)
                if (auth is null)
                    throw new ApplicationException("Session timeout");
                else
                {
                    if (mode.ToLower() == "email")
                        auth.Is2FEmail = true;
                    else if (mode.ToLower() == "auth")
                        auth.Is2FAuthenticator = true;
                    else if (mode.ToLower() == "mobile")
                        auth.Is2FMobile = true;
                }
            if (AnySessionForThisAuth(authID))
                throw new InvalidOperationException("Invalid Request, Session already exist..");

            dbctx.SaveChanges();
            var e = CreateSession(auth);
            var ss = GetSession(e.SessionHash);

            result = ss!.ToModel();


            return result;
        }
        internal eUserAccount GetMyUserAccount()
        {
            var s = GetMySession();
            if (s is null) return null;
            var u = GetUserbyId(s.UserAccount.Id.ToGUID());
            return u;

        }
        internal Guid GetMyUserAccountId()
        {
            var str = GetSessionHash();
            if (str.IsNullorEmpty()) return Guid.Empty;
            var us = dbctx.UserSession
               .Include(x => x.UserAccount).FirstOrDefault(x => x.SessionHash == str);
            if (us == null) return Guid.Empty;

            return us.UserAccountId;
        }
        internal mUserSession GetMySession()
        {
            mUserSession result = null;
            var str = GetSessionHash();
            if (str.IsNullorEmpty()) return result;
            var ss = GetSession(str);
            if (ss is null) return result;
            ExtendSession(str);
            return ss.ToModel();
        }
        internal bool ExtendSession(string hash)
        {

            var se = dbctx.UserSession
                .FirstOrDefault(x => x.SessionHash == hash);
            if (se == null || se.ShouldExpierOn <= DateTime.UtcNow || se.ExpieredOn.HasValue)
            {
                return false;
            }
            se.ShouldExpierOn = DateTime.UtcNow.AddMinutes(120);
            dbctx.SaveChanges();
            return true;
        }
        internal eUserSession? GetSession(string hash)
        {
            /*ToDo: Naveen, Multi Session Impact,
             * Session Should only be Loaded from UM API service
             */
            var se = dbctx.UserSession
                .Include(x => x.UserAccount)
                .ThenInclude(x => x.UserProfile)
                .Include(x => x.UserAccount)
                .ThenInclude(x => x.AuthEmail)
                .Include(x => x.UserAccount)
                .ThenInclude(x => x.TaxResidency)
                 .Include(x => x.UserAccount)
                .ThenInclude(x => x.CitizenOf)
                .Include(x => x.UserAccount)
                .ThenInclude(x => x.Mobile)
                 .Include(x => x.UserAccount)
                .ThenInclude(x => x.Authenticator)
                 .Include(x => x.SessionAuthEvent)
                .FirstOrDefault(x => x.SessionHash == hash);

            if (se != null && se?.UserAccount.TaxResidency != null)
            {
                se.UserAccount.TaxResidency = dbctx.TaxResidency
                    .Include(x => x.Country).FirstOrDefault(x => x.TaxResidencyId == se.UserAccount.TaxResidency.TaxResidencyId);
            }

            if (se?.UserAccount?.UserProfile != null)
                se.UserAccount.UserProfile.UserAccount = se.UserAccount;
            return se;
        }
        internal mUserSession RequestSession(Guid authID, string uName)
        {
            mUserSession result = null;
            var auth = dbctx.AuthenticationEvent
                 .Include(x => x.UserAccount)
                 .FirstOrDefault(x => x.AuthenticationEventId == authID && x.UserAccount.AuthEmail!.Email == uName);
            if (auth is null)
                throw new ArgumentException("Invalid auth ID");

            if (auth.IsMultiFactor)
                throw new InvalidOperationException("User is Configured for MultiFactor, Request Denied..");
            if (AnySessionForThisAuth(authID))
                throw new InvalidOperationException("Invalid Request, Session already exist..");
            var e = CreateSession(auth);
            e = GetSession(e.SessionHash);
            if (e is null) throw new ApplicationException("Failed to create session...Internal error");
            result = e.ToModel();


            return result;
        }
        internal mAuth Authenticate(string uName, string password, HttpContext http)
        {
            mAuth result = null;
            var user = GetUser(uName);
            if (user == null) throw new ArgumentException("Invalid User name or password"); ;
            if (!user.IsPrimaryCompleted)
                throw new ArgumentException("User must rest its password");

            var res = CompareHash(user.SecurePassword!.Password, password);
            if (res == PasswordVerificationResult.Success)
            {
                BuildLog(user, true, http);
                result = BuildPassAuthEvent(user);
            }
            else
                BuildLog(user, false, http);

            return result;
        }
        /// <summary>
        /// Use this for Depost/WithDraw Request that need 2F Auth
        /// </summary>
        /// <returns></returns>
        internal mAuth InSession2FAuth()
        {
            mAuth result = null;
            var mySess = GetMySession();
            if (mySess == null)
                throw new ArgumentException("Invalid User Identity");
            var user = GetUserbyId(mySess.UserAccount.Id.ToGUID());
            if (user == null) throw new ArgumentException("Invalid User Identity");

            result = BuildPassAuthEvent(user);

            return result;
        }
        internal bool ValidateOTP(string uName, string OTP)
        {
            var user = GetUser(uName);
            if (user == null) return false;
            var dt = user.EmailValidationActions.Max(x => x.StartedOn);//Only LastOne should be validated
            var e = user.EmailValidationActions.FirstOrDefault(x => x.StartedOn == dt);
            if (e.ShouldExpierOn <= DateTime.UtcNow)
                throw new ArgumentException("OTP has expired, Try Again");
            if (CompareHash(e.OTPHash, OTP) == PasswordVerificationResult.Success)
            {
                e.IsCompleted = true;
                dbctx.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
        internal bool IncludeMFAuthenticator(string uName, string code)
        {
            var user = GetUser(uName);
            if (user == null) return false;
            if (user.IsMultiFactor)//User should have enbaled MultiFactor before calling this
            {
                user.Authenticator = new eAuthenticator();
                user.Authenticator.Code = code;
                dbctx.SaveChanges();
            }
            else
                throw new InvalidOperationException("Multi-Factor is alreday enabled on this Profile");

            return user.IsMultiFactor;

        }
        internal bool EnableMultiFactor(string uName)
        {
            var user = GetUser(uName);
            if (user == null) return false;
            if (user.IsMultiFactor) return true;
            if (!user.IsPrimaryCompleted)
                throw new InvalidOperationException("Primary Authentication is not Yet Completed.");
            user.IsMultiFactor = true;

            dbctx.SaveChanges();
            return user.IsMultiFactor;
        }
        internal bool DiableMultiFactor(string uName)
        {
            var user = GetUser(uName);
            if (user == null) return false;
            if (user.IsMultiFactor == false) return true;
            user.IsMultiFactor = false;

            dbctx.SaveChanges();
            return user.IsMultiFactor;
        }
        internal bool IsEmailVerified(string uName)
        {
            var user = GetUser(uName);
            if (user == null) return false;
            var u = dbctx.EmailValidationAction.FirstOrDefault(x => x.IsCompleted && x.UserAccount.UserAccountId == user.UserAccountId);
            if (u == null) return false;
            return true;
        }
        internal bool CanSetPassword(string uName)
        {
            var user = GetUser(uName);
            if (user == null) return false;
            var u = dbctx.EmailValidationAction.FirstOrDefault(x => x.IsCompleted && x.UserAccount.UserAccountId == user.UserAccountId);
            if (u == null) return false;
            return true;
        }
        internal eAuthenticationEvent GetAuthEvent(Guid Id)
        {
            if (Id == Guid.Empty) return null;

            var auth = dbctx.AuthenticationEvent
                  .Include(x => x.UserAccount)
                  .ThenInclude(x => x.AuthEmail)
                  .Include(x => x.UserAccount)
                  .ThenInclude(x => x.Authenticator)
                   .Include(x => x.UserAccount)
                  .ThenInclude(x => x.Mobile)
                  .FirstOrDefault(x => x.AuthenticationEventId == Id);
            return auth;
        }
        internal bool UpdatePasswordOfUserAccount(Guid userId, string password, IOptions<SmtpConfig> smtp, UserType ut = UserType.User, string key = "")
        {
            Nullable<bool> isFirstTime = null;
            if (!ValidatePassword(password))
                throw new ArgumentException("Invalid Password");
            var SecPwd = GetHash(password);
            var obj = dbctx.UserAccount.Include(x => x.SecurePassword).Include(x => x.AuthEmail).FirstOrDefault(x => x.UserAccountId == userId);
            if (obj is null) throw new ArgumentException("Invalid User Id");
            obj.SecurePassword = new eSecurePassword() { Password = SecPwd };
            //First time Password Setup?
            if (!obj.IsPrimaryCompleted)
            {
                obj.IsPrimaryCompleted = true;
                isFirstTime = true;
                obj.IsActive = true;
                GenerateAccount(obj, ut, key);
            }
            dbctx.SaveChanges();
            if (ut == UserType.MarketMaking) return true;

            // Send Email
            if (isFirstTime.HasValue && isFirstTime.Value)
            {
                var Htmbody = EmailTemplateManager.GetSignUpCompleteEventTemplate(obj.AuthEmail.Email);
                mEmail email = new mEmail()
                {
                    To = obj.AuthEmail.Email,
                    Subject = "Your Account is Confirmed",
                    Body = Htmbody
                };

                new EmailManager(smtp).SendEmail(email);
                Htmbody = EmailTemplateManager.GetPreBetaNavCBuySuggestionAfterSignUpTemplate(obj.AuthEmail.Email);
                mEmail email1 = new mEmail()
                {
                    To = obj.AuthEmail.Email,
                    Subject = "Welcome to NavExM ",
                    Body = Htmbody
                };
                new EmailManager(smtp).SendEmail(email1);
            }
            else
            {
                SendEmailPasswordReset(obj.AuthEmail.Email, smtp);
            }

            //staus update for Reward Record
            var rec = Rewardctx.SignUpUsers.FirstOrDefault(x => x.UserAccountId == obj.UserAccountId);
            if (rec != null)
            {
                rec.PrimaryCompletedOn = DateTime.UtcNow;
                Rewardctx.SaveChanges();
            }
            return true;
        }
        #region Private Methods

        private bool AnySessionForThisAuth(Guid authID)
        {
            var isThere = dbctx.UserSession.FirstOrDefault(x => x.SessionAuthEvent.AuthenticationEventId == authID);
            return isThere != null;
        }
        private eUserSession CreateSession(eAuthenticationEvent auth)
        {
            /* ToDo: Naveen, Multi Session impact on wallet Balance/Transaction
             * So, Identify Session and ensure that it is preserved in UM
             */
            var result = new eUserSession();
            result.UserAccount = auth.UserAccount;
            result.SessionAuthEvent = auth;
            result.SessionHash = GetSignedHash($"{auth.AuthenticationEventId}+{auth.CreatedOn}");
            dbctx.SaveChanges();
            dbctx.Add(result);
            dbctx.SaveChanges();
            result.UserAccount = GetUserbyId(result.UserAccount.UserAccountId);
            //ToDo: Naveen, It seems additional UM is not required based on our current implementation.
            // SrvSessionManagement.AddSession(result);
            return result;
        }

        internal mAuth BuildPassAuthEvent(eUserAccount user)
        {
            var mauth = new mAuth();
            var auth = new eAuthenticationEvent();
            auth.IsPasswordAuth = true;
            auth.IsUserNameAuth = true;
            auth.GeoInfo = GetGeoInfo();
            auth.UserAccount = user;
            auth.IsMultiFactor = user.IsMultiFactor;
            dbctx.AuthenticationEvent.Add(auth);
            dbctx.SaveChanges();

            auth = GetAuthEvent(auth.AuthenticationEventId);
            mauth = auth.ToModel();
            if (auth.IsMultiFactor)
                mauth.GAuthCode = user.Authenticator != null ? user.Authenticator.Code : string.Empty;
            //mauth.userName = user.AuthEmail!.Email;
            //mauth.AccountNumber = user.FAccountNumber!;
            //mauth.userAccountID = user.UserAccountId;
            return mauth;
        }
        private bool BuildLog(eUserAccount user, bool isSussess, HttpContext http)
        {
            var log = new eUserAccountAuthenticationLog();
            log.GeoInfo = GetGeoInfo(
                );
            log.UserAccount = user;
            log.IsSuccess = isSussess;
            if (isSussess)
                log.LogMsg = $"{user.FAccountNumber} achieved Authontication.{ConnectionTrace(http)}";
            else
                log.LogMsg = $"{user.FAccountNumber} failed to Authonticate.{ConnectionTrace(http)}";

            log.SessionHash = GetHash($"{user.UserAccountId}-{log.LogMsg}-{DateTime.UtcNow}");
            dbctx.UserAccountAuthenticationLog.Add(log);
            dbctx.SaveChanges();
            return true;
        }
        string ConnectionTrace(HttpContext ht)
        {
            //Add other Parameters if Required..
            return $"RemoteIP:{ht.Connection.RemoteIpAddress}|RemotePort:{ht.Connection.RemotePort}|MyIP:{ht.Connection.LocalIpAddress}|MyPort:{ht.Connection.LocalPort}";
        }
        private bool ValidateUserName(string uName)
        {
            var hasSpecial = new Regex(@"[!,#,$,' ',%,^,&,*,(,),<,>,\],\[]+");
            if (uName.IsNullorEmpty() || uName.Length > 51)
                throw new ApplicationException("Invalid User name attempted");
            var u = uName.Split("@");
            if (!(u.Count() == 2 && u.First().Length <= 25 && u.Last().Length <= 25))
                throw new ApplicationException("Invalid User name attempted");

            return !hasSpecial.IsMatch(uName);
        }
        private bool ValidatePassword(string pwd)
        {

            var hasNumber = new Regex(@"[0-9]+");
            var nothasSpace = new Regex(@"[^\s]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasMin8Max15Chars = new Regex(@".{8,15}");
            var hasSpecial = new Regex(@"[!,@,#,$,%,^,&,*,(,),<,>,\],\[]+");

            var isValidated = hasNumber.IsMatch(pwd);
            isValidated = isValidated && hasUpperChar.IsMatch(pwd);
            isValidated = isValidated && hasMin8Max15Chars.IsMatch(pwd);
            isValidated = isValidated && hasSpecial.IsMatch(pwd);
            isValidated = isValidated && nothasSpace.IsMatch(pwd);

            return isValidated;
        }
        private bool GenerateAccount(eUserAccount user, UserType ut = UserType.User, string key = "")
        {
            ulong min = 1005241;
            var m = dbctx.UserAccount.Max(x => x.AccountNumber) + 1;
            if (m is null || m < min) m = min + 1;
            if (user.AccountNumber is null)
            {
                user.AccountNumber = m;
                switch (ut)
                {
                    case UserType.User:
                        user.FAccountNumber = $"U{m}";
                        break;
                    case UserType.Staff:
                        user.FAccountNumber = $"S{m}";
                        break;
                    case UserType.StaffAdmin:
                        user.FAccountNumber = $"SA{m}";
                        break;
                    case UserType.MarketMaking:
                        user.FAccountNumber = $"MM-{key.ToUpper()}-{m}";
                        break;
                    default:
                        break;
                }
                GetWalletManager().GenerateWallets(user);
            }
            return true;
        }

        private string GeneratetOTP()
        {
            //Business Rule state OTP is 6 Char long with 3 Alpha and 3 Numeric
            int stringlength = 0;
            int numberLength = 6;
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string number = "0123456789";
            var s1 = new string(Enumerable.Repeat(chars, stringlength).Select(s => s[random.Next(s.Length)]).ToArray());
            var s2 = new string(Enumerable.Repeat(number, numberLength).Select(s => s[random.Next(s.Length)]).ToArray());

            return s1 + s2;

        }


        private eGeoInfo GetGeoInfo()
        {
            var geoInfo = new eGeoInfo();
            if (httpContext != null && !string.IsNullOrEmpty(httpContext.Request.Headers["cf-ipcountry"]))
            {
                geoInfo.IP = httpContext.Request.Headers["CF-Connecting-IP"];
                geoInfo.CountryCode = httpContext.Request.Headers["CF-IPCountry"];
                geoInfo.City = httpContext.Request.Headers["cf-ipcity"];
                geoInfo.Ipcontinent = httpContext.Request.Headers["cf-ipcontinent"];
                geoInfo.Longitude = httpContext.Request.Headers["cf-iplongitude"];
                geoInfo.Latitude = httpContext.Request.Headers["cf-iplatitude"];

                dbctx.GeoInfo.Add(geoInfo);
                dbctx.SaveChanges();

            }
            return geoInfo;
        }
        private WalletManager GetWalletManager()
        {
            var result = new WalletManager();
            result.dbctx = dbctx;
            result.httpContext = httpContext;
            return result;
        }

        #endregion
    }

}
