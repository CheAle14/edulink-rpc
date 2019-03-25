using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduLinkRPC.Classes
{
    public class HwkAttachment
    {
        [JsonProperty("filename")]
        public string FileName { get; internal set; }

        [JsonProperty("filesize")]
        public long FileSize { get; internal set; }

        [JsonProperty("mime_type")]
        public string MimeType { get; internal set; }

    }
}
