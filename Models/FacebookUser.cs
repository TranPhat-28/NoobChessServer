using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoobChessServer.Models
{
    public class FacebookUser
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public FacebookPicture Picture { get; set; }
    }

    public class FacebookPicture
    {
        public PictureData Data { get; set; }
    }

    public class PictureData
    {
        public int Height { get; set; }
        public bool IsSilhouette { get; set; }
        public string Url { get; set; } = "";
        public int Width { get; set; }
    }
}