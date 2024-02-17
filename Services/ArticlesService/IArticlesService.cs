using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NoobChessServer.DTOs.ArticleDtos;

namespace NoobChessServer.Services.ArticlesService
{
    public interface IArticlesService
    {
        Task<ServiceResponse<List<GetArticleDto>>> FetchArticles(FetchArticlesDto fetchArticlesDto);
    }
}