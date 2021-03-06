﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSender.Models
{
    public class TrackData
    {
        public string Status { get; set; }
        public string Album { get; set; }
        public string Artist { get; set; }
        public string GroupId { get; set; }
        public int ReleaseYear { get; set; }
        public string Title { get; set; }
        public double TrackLengthSec { get; set; }
    }
}