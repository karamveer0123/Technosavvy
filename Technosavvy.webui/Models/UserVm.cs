using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace TechnoApp.Ext.Web.UI.Models
{
    public class BaseVm
    {

        private BaseVm _baseVm;

        public BaseVm baseVm
        {
            get { DoLoad(); return _baseVm; }
            private set { _baseVm = value; }
        }
        private void DoLoad()
        {

        }

        public string CurrentUserId { get; set; }
        public string CurrentUserSessionId { get; set; }
    }
    public class CFGeoVm
    {
        [JsonProperty("cf-ipcountry")]
        public string CountryCode { get; set; }

        [JsonProperty("cf-ipcity")]
        public string City { get; set; }

        [JsonProperty("cf-ipcontinent")]
        public string Ipcontinent { get; set; }

        [JsonProperty("cf-iplongitude")]
        public string Longitude { get; set; }

        [JsonProperty("cf-iplatitude")]
        public string Latitude { get; set; }

        [JsonProperty("cf-ip")]
        public string IP { get; set; }

        [JsonProperty("headerstr")]
        public string headerstr { get; set; }

    }

 
   
}
