using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduLinkRPC.Classes
{
    public class HwkAttachment : EdulinkObject
    {
        [JsonProperty("filename")]
        public string FileName { get; internal set; }

        [JsonProperty("filesize")]
        public long FileSize { get; internal set; }

        [JsonProperty("mime_type")]
        public string MimeType { get; internal set; }


        internal HwkAttachment(Edulink client, API.Attachment model) : base(client)
        {
            Update(model);
        }

        public static HwkAttachment Create(Edulink client, API.Attachment modeL)
        {
            return new HwkAttachment(client, modeL);
        }

        internal void Update(API.Attachment model)
        {
            this.FileName = model.filename;
            this.FileSize = model.filesize;
            this.MimeType = model.mime_type;
        }
    }
}
