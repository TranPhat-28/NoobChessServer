using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NoobChessServer.DTOs.ArticleDtos;

namespace NoobChessServer.Services.ArticlesService
{
    public class ArticlesService : IArticlesService
    {
        public async Task<ServiceResponse<List<GetArticleDto>>> FetchArticles(FetchArticlesDto fetchArticlesDto)
        {
            var response = new ServiceResponse<List<GetArticleDto>>();

            try
            {
                // Call NewsAPI to fetch articles
                using (var client = new HttpClient())
                {
                    var uri = new Uri($"https://newsapi.org/v2/everything?q=+chess&searchIn=title&pageSize=3&apiKey=009f843c9303421b9048011b95ccf8c9&page={fetchArticlesDto.Page}");
                    client.DefaultRequestHeaders.UserAgent.ParseAdd("NoobChessServer");

                    // Read the response
                    var result = await client.GetAsync(uri);
                    var jsonMessage = await result.Content.ReadAsStringAsync();

                    // Now parse it to get the information
                    var articles = JsonConvert.DeserializeObject<NewsAPIResponse>(jsonMessage);

                    // Response
                    response.Data = articles!.Articles;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.Message = "Server error";
                response.IsSuccess = false;
            }

            return response;
        }
    }
}