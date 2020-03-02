using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PathOfExile.Apis.Official.Models;
using PathOfExile.Apis.Official.Trade.Leagues;

namespace PathOfExile.Apis.Official
{
    public class PoeApi : IPoeApi
    {
        private readonly ILogger logger;
        private readonly HttpClient client;

        public PoeApi(ILogger logger,
            IHttpClientFactory httpClientFactory)
        {
            this.logger = logger;
            client = httpClientFactory.CreateClient("poe_api");
            client.BaseAddress = new Uri("https://www.pathofexile.com/api/");
        }

        private Language language;
        public Language Language
        {
            get
            {
                return language;
            }
            set
            {
                language = value;
                client.BaseAddress = new Uri(BaseApiUrl);
            }
        }

        public HttpClient Client { get; private set; }

        private string BaseApiUrl => Language switch
        {
            Language.French => "https://fr.pathofexile.com/api/",
            Language.German => "https://de.pathofexile.com/api/",
            Language.Korean => "https://poe.game.daum.net/api/",
            Language.Portuguese => "https://br.pathofexile.com/api/",
            Language.Russian => "https://ru.pathofexile.com/api/",
            Language.Spanish => "https://es.pathofexile.com/api/",
            Language.Thai => "https://th.pathofexile.com/api/",
            Language.TraditionalChinese => "http://web.poe.garena.tw/api/",
            _ => "https://www.pathofexile.com/api/"
        };

        public JsonSerializerOptions JsonOptions
        {
            get
            {
                var options = new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    IgnoreNullValues = true,
                };
                options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
                return options;
            }
        }

        public async Task<List<TReturn>> Fetch<TReturn>()
        {
            string path;
            string name = string.Empty;
            switch (typeof(TReturn).Name)
            {
                case nameof(ItemCategory):
                    name = "items";
                    path = "data/items/";
                    break;
                case nameof(League):
                    name = "leagues";
                    path = "data/leagues/";
                    break;
                case nameof(StaticItemCategory):
                    name = "static items";
                    path = "data/static/";
                    break;
                case nameof(AttributeCategory):
                    name = "attributes";
                    path = "data/stats/";
                    break;
                default: throw new Exception("The type to fetch is not recognized by the PoeApiService.");
            }

            logger.LogInformation($"Fetching {name} started.");
            QueryResult<TReturn> result = null;
            var success = false;

            while (!success)
            {
                try
                {
                    var response = await client.GetAsync(BaseApiUrl + path);
                    var content = await response.Content.ReadAsStreamAsync();

                    result = await JsonSerializer.DeserializeAsync<QueryResult<TReturn>>(content, JsonOptions);

                    logger.LogInformation($"{result.Result.Count} {name} fetched.");
                    success = true;
                }
                catch
                {
                    logger.LogInformation($"Could not fetch {name}.");
                    logger.LogInformation("Retrying every minute.");
                    await Task.Delay(TimeSpan.FromMinutes(1));
                }
            }

            logger.LogInformation($"Fetching {name} finished.");
            return result.Result;
        }
    }
}
