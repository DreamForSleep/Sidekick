using System.Collections.Generic;
using System.Threading.Tasks;

namespace PathOfExile.Apis.Official.Trade.Leagues
{
    public interface ILeagueApi
    {
        Task<List<League>> GetList();
    }
}
