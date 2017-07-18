using System;
using System.Collections.Generic;
using Newtonsoft.Json;


namespace Dropbox.Api
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ShareResponse
    {
        [JsonProperty(PropertyName = "url")]
        public string url { get; set; }

        [JsonProperty(PropertyName = "expires")]
        public string expires { get; set; }

        [JsonProperty(PropertyName = "visibility")]
        public string visibility { get; set; }

        public ShareResponse()
        {
            url = "#";
            expires = "";
            visibility = "";
        }

    }
}

