using System;
using System.Collections.Generic;

namespace Hishop.Weixin.MP.Domain
{
    public interface IMedia
    {
        string MediaId { get; set; }
    }

    public interface IThumbMedia
    {
        string ThumbMediaId { get; set; }
    }


    public class Image : IMedia
    {
        public string MediaId { get; set; }
    }

    public class Voice : IMedia 
    {
        public string MediaId { get; set; }
    }

    public class Video : IMedia, IThumbMedia
    {
        public string MediaId { get; set; }

        public string ThumbMediaId { get; set; }        
    }

    public class Music : IThumbMedia
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string MusicUrl { get; set; }

        public string HQMusicUrl { get; set; }

        public string ThumbMediaId { get; set; }
    }
}
