using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NoobChessServer.DTOs.ArticleDtos;

namespace NoobChessServer.Models
{
    public class NewsAPIResponse
    {
        public string Status { get; set; } = "";
        public int TotalResult { get; set; }
        public List<GetArticleDto>? Articles { get; set; }
    }
}