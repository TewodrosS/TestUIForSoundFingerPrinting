using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSender.Models
{
    public class UploadFileModel
    {
        [FileSize(90240000)]
        [FileTypes("mp3,wav,ogg,flac")]
        public HttpPostedFileBase File { get; set; }
    }
}