using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfHostApi.Models
{
    public class AudioRequest
    {
        public string FileType { get; set; }
        public string FileName { get; set; }
        public byte[] Content { get; set; }
    }
}
