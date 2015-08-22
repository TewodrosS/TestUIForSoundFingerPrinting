using SoundFingerprinting.DAO;
using SoundFingerprinting.DAO.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerPrintingAPI.Models
{
    public class TrackDataExternal
    {
        public TrackDataExternal(TrackData t)
        {
            Album = t.Album;
            Artist = t.Artist;
            GroupId = t.GroupId;
            ISRC = t.ISRC;
            ReleaseYear = t.ReleaseYear;
            Title = t.Title;
            TrackLengthSec = t.TrackLengthSec;            
        }

        public string Status { get; set; }
        public string Album { get; set; }
        public string Artist { get; set; }
        public string GroupId { get; set; }
        public string ISRC { get; set; }
        public int ReleaseYear { get; set; }
        public string Title { get; set; }
        public double TrackLengthSec { get; set; }              
    }   
}
