using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PathOfExile.Apis.Official.Models;

namespace PathOfExile.Apis.Official.Trade.Leagues
{
    public class LeagueApi : ILeagueApi
    {
        private readonly IPoeApi api;
        private readonly ILogger logger;

        public LeagueApi(IPoeApi api,
            ILogger logger)
        {
            this.api = api;
            this.logger = logger;
        }

        public async Task<List<League>> GetList()
        {
            var response = await api.Client.GetAsync("trade/leagues");
            var content = await response.Content.ReadAsStreamAsync();
            var result = await JsonSerializer.DeserializeAsync<QueryResult<League>>(content, api.JsonOptions);
            logger.LogInformation($"LeagueApi.GetList - Found {result.Result.Count} leagues.");
            return result.Result;
        }
    }
}
