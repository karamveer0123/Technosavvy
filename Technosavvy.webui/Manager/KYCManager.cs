using TechnoApp.Ext.Web.UI.Service;
using TechnoApp.Ext.Web.UI.Model;
using System;
using Microsoft.EntityFrameworkCore;

namespace TechnoApp.Ext.Web.UI.Manager
{
    public class KYCManager : MaintenanceSvc
    {
        internal async Task<List<mCategoryDocFile>> GetvmKYCDocUploadStage2(string abbr)
        {
            var ret = new List<mCategoryDocFile>();
            var cat = await GetDocTemplates(abbr);
            if (cat == null) return ret;
            foreach (var c in cat)
            {
                var m = new mCategoryDocFile
                {
                    CategoryName = c.CategoryName,
                    CategoryId = c.CategoryId,
                    PassScore = c.PassScore,
                    DocumentTemplates = new List<mDocFileInstance>()
                };
                c.DocumentTemplates.ForEach(x =>
                {
                    m.DocumentTemplates.Add(new mDocFileInstance
                    {
                        HelpInfo = x.DocTemplateHelpInfo,
                        PlaceHolderId = x.DocumentTemplateId,
                        IsBack = x.IsBackRequired,
                        IsFront = x.IsFrontRequired,
                        MaxAllowedFileSize = x.DocInstanceSize.HasValue ? x.DocInstanceSize.Value : 20,
                        PlaceHolderName = x.DocTemplateName,
                        PointValue = x.Score.Value
                    });
                });
                ret.Add(m);
            }
            return ret;
        }
        internal async Task<List<vmCountry>> GetvmCountriesList()
        {
            var lst = (await GetAllCountries()).Select(x => new vmCountry { countryId = x.CountryId, countryName = x.Name, Abbri = x.Abbrivation }).ToList();

            return lst;
        }
        internal async Task<mProfile> GetMyProfile()
        {
            var m = await GetProfile(_appSessionManager.mySession.UserId.ToGuid());
            if (m != null && m.myDocs != null)
                m.myDocs = m.myDocs.Where(x => x.IsFront).ToList();
            return m;
        }
        internal async Task<bool> SaveDocInstances(vmKYCDocUploadStage vm)
        {
            var docx = ToDocRecords(vm);
            await AddKYCDoc(docx);

            return true;
        }
        internal List<mKYCDocRecord> ToDocRecords(vmKYCDocUploadStage vm)
        {
            var ret = new List<mKYCDocRecord>();
            foreach (var Inst in vm.DocInstances)
            {
                foreach (var doc in Inst.DocumentTemplates)
                {
                    if (doc.Front != null && doc.IsFront)
                    {
                        var m = new mKYCDocRecord
                        {
                            CategoryId = Inst.CategoryId,
                            CategoryName = Inst.CategoryName,
                            CountryAbbrivation = vm.Profile.Address.Country.Abbri,
                            DocFileSize = (int)doc.Front.Length,
                            InternalName = doc.DocNumber,
                            PublicName = $"{doc.PlaceHolderName}_Front_{DateTime.UtcNow.ToString("yyyyMMddhhmmss")}_{Guid.NewGuid().ToString().Replace("-", "")}",
                            IsFront = true,
                            PlaceHolderId = doc.PlaceHolderId,
                            PlaceHolderName = doc.PlaceHolderName,
                            Ext = Path.GetExtension(doc.Front.FileName),
                            ProfileId = vm.Profile.ProfileId,
                            UserAccountId = vm.Profile.UserAccountId,
                            Status = eDocumentStatus.Submitted
                        };
                        using (var ms = new MemoryStream())
                        {
                            doc.Front.CopyTo(ms);
                            m.data = ImageString(ms.ToArray(), m.Ext);
                        }
                        ret.Add(m);
                    }
                    if (doc.Back != null && doc.IsBack)
                    {
                        var m = new mKYCDocRecord
                        {
                            CategoryId = Inst.CategoryId,
                            CategoryName = Inst.CategoryName,
                            CountryAbbrivation = vm.Profile.Address.Country.Abbri,
                            DocFileSize = (int)doc.Back.Length,
                            InternalName = Guid.NewGuid().ToString().Replace("-", ""),
                            PublicName = $"{doc.PlaceHolderName}_Back_{DateTime.UtcNow.ToString("yyyyMMddhhmmss")}",
                            IsBack = true,
                            PlaceHolderId = doc.PlaceHolderId,
                            PlaceHolderName = doc.PlaceHolderName,
                            Ext = Path.GetExtension(doc.Back.FileName),
                            ProfileId = vm.Profile.ProfileId,
                            UserAccountId = vm.Profile.UserAccountId,
                            Status = eDocumentStatus.Submitted
                        };
                        using (var ms = new MemoryStream())
                        {
                            doc.Front.CopyTo(ms);
                            m.data = ImageString(ms.ToArray(), m.Ext);
                        }
                        ret.Add(m);
                    }
                }
            }
            return ret;
        }

        //private string ImageString(byte[] imageBytes, string ext)
        //{
        //    string base64String = Convert.ToBase64String(imageBytes, 0, imageBytes.Length);
        //    string imageUrl = string.Concat($"data:image/{ext};base64,", base64String);
        //    return imageUrl;
        //}
        internal List<mCategoryDocFile> ValidateDocInstances(List<mCategoryDocFile> vm)
        {
            vm.CheckAndThrowNullArgumentException();
            bool isException = false;
            foreach (var Inst in vm)
            {
                Inst.CatErrorList ??= new List<string>();
                Inst.CatErrorList.Clear();
                var av = Inst.DocumentTemplates.Where(x =>
                (x.IsFront && x.Front != null) && (x.IsBack && x.Back != null)).ToList()
                .Union(
                Inst.DocumentTemplates.Where(x =>
                (x.IsFront && x.Front != null) && (x.IsBack == false && x.Back == null)).ToList()).ToList();

                var bm = Inst.DocumentTemplates.Where(x =>
             (x.IsFront && x.Front != null) && (x.IsBack && x.Back == null)).ToList();

                var fm = Inst.DocumentTemplates.Where(x =>
            (x.IsFront && x.Front == null)).ToList();
                var f = Inst.DocumentTemplates.Where(x => x.Front != null && x.Front.Length > 0 && x.IsFront).ToList();

                var score = av.Sum(x => x.PointValue);
                if (score < Inst.PassScore)
                {
                    Inst.CatErrorList.Add($"Insufficent documents provided in '{Inst.CategoryName}' category");
                    isException = true;
                    foreach (var i in bm)
                    {
                        Inst.CatErrorList.Add($"{i.PlaceHolderName} back page is required.");
                    }
                    foreach (var i in fm)
                    {
                        Inst.CatErrorList.Add($"{i.PlaceHolderName} Front page is required.");
                    }
                }

                foreach (var i in av)
                {
                    if (i.DocNumber.IsNullOrEmpty())
                        Inst.CatErrorList.Add($"{i.PlaceHolderName} Document Number is required.");
                    if (i.IsFront && (i.MaxAllowedFileSize * 1024) < i.Front.Length)
                        Inst.CatErrorList.Add($"{i.PlaceHolderName} Document exceeds limit size({i.MaxAllowedFileSize} KB).");
                    if (i.IsBack && (i.MaxAllowedFileSize * 1024) < i.Back.Length)
                        Inst.CatErrorList.Add($"{i.PlaceHolderName} Document exceeds limit size({i.MaxAllowedFileSize} KB).");

                }
                isException = isException || Inst.CatErrorList.Count > 0;
            }
            if (isException)
                throw new ApplicationException("ERROR in Support Documents");

            return vm;
        }
        internal bool ValidateForMiniProfile(vmProfile vm)
        {
            vm.FirstName.CheckAndThrowNullArgumentException();
            vm.LastName.CheckAndThrowNullArgumentException();
            vm.NickName.CheckAndThrowNullArgumentException();
            vm.DateOfBirth.CheckAndThrowNullArgumentException();
            if (vm.DateOfBirth.AddYears(18) > DateTime.UtcNow)
            {
                throw new ArgumentNullException($"User must be 18 years or over");
            }
            if ((vm.TaxResidencyId == null || vm.TaxResidencyId == Guid.Empty) && vm.selectedTaxResidency.countryId == Guid.Empty)
            {
                throw new ArgumentNullException($"Tax Residency must be provided");
            }
            if ((vm.CitizenshipId == null || vm.CitizenshipId == Guid.Empty) && vm.selectedCitizenOf.countryId == Guid.Empty)
            {
                throw new ArgumentNullException($"Citizenship must be provided");
            }
            return true;
        }
        internal bool ValidateForProfile(vmProfile vm)
        {
            ValidateForMiniProfile(vm);
            if (vm.Address == null)
            {
                throw new ArgumentNullException($"Address must be provided");
            }
            if (vm.Address.City.IsNullOrEmpty())
            {
                throw new ArgumentNullException($"City Name must be provided");
            }
            if (vm.Address.StreetAdd.IsNullOrEmpty())
            {
                throw new ArgumentNullException($"Street Name must be provided");
            }
            if (vm.Address.PostCode.IsNullOrEmpty())
            {
                throw new ArgumentNullException($"Post Code must be provided");
            }
            if (vm.Address.State.IsNullOrEmpty())
            {
                throw new ArgumentNullException($"State must be provided");
            }
            return true;
        }
        protected internal async Task<mProfile> CreateProfile(mProfile userProfile)
        {
            mProfile res = new mProfile();
            var _endPoint = $"Profile/CreateProfile";

            HttpResponseMessage response = await GetMaintAPIChannel().PostAsJsonAsync(_endPoint, userProfile);
            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<mProfile>();
            }
            return res;
        }
        protected internal async Task<List<mKYCDocRecord>> AddKYCDoc(List<mKYCDocRecord> m)
        {
            List<mKYCDocRecord> res = new List<mKYCDocRecord>();
            var _endPoint = $"KYC/AddKYCDoc";

            HttpResponseMessage response = await GetMaintAPIChannel().PostAsJsonAsync(_endPoint, m);
            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<List<mKYCDocRecord>>();
            }
            return res;
        }
        protected internal async Task<mProfile> UpdateProfilePersonalInfo(mProfile userProfile)
        {
            mProfile res = new mProfile();
            var _endPoint = $"Profile/UpdateProfilePersonalDetails";

            HttpResponseMessage response = await GetMaintAPIChannel().PostAsJsonAsync(_endPoint, userProfile);
            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<mProfile>();
            }
            return res;
        }
        protected internal async Task<List<mCategoryDocTemplate>> GetDocTemplates(string Abbr)
        {
            List<mCategoryDocTemplate> res = null;

            var _endPoint = $"KYC/GetDocTemplatesOf?Abb={Abbr}";

            HttpResponseMessage response = await GetMaintAPIChannel().GetAsync(_endPoint);
            if (response.IsSuccessStatusCode)
            {
                res = await response.Content.ReadFromJsonAsync<List<mCategoryDocTemplate>>();
            }
            return res;
        }


    }

}
