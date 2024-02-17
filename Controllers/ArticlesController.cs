using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NoobChessServer.DTOs.ArticleDtos;
using NoobChessServer.Services.ArticlesService;

namespace NoobChessServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArticlesController : ControllerBase
    {
        private readonly IArticlesService _articlesService;

        public ArticlesController(IArticlesService articlesService)
        {
            _articlesService = articlesService;
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<List<GetArticleDto>>>> FetchArticles(FetchArticlesDto fetchArticlesDto)
        {
            var response = new ServiceResponse<List<GetArticleDto>>();

            if (!(fetchArticlesDto.Page >= 1))
            {
                // Bad request
                response.IsSuccess = false;
                response.Message = "Invalid page number";

                return BadRequest(response);
            }
            else
            {
                // Call the service
                response = await _articlesService.FetchArticles(fetchArticlesDto);
                return Ok(response);
            }
        }
    }
}