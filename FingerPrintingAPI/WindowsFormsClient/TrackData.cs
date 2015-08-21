using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsClient
{
    public class TrackData
    {
        public string Album { get; set; }
        public string Artist { get; set; }
        public string GroupId { get; set; }
        public string ISRC { get; set; }
        public int ReleaseYear { get; set; }
        public string Title { get; set; }
        public double TrackLengthSec { get; set; }        
    }
           
}
