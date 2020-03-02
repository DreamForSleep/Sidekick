using System.Collections.Generic;
using PathOfExile.Apis.Official.Trade.Leagues;

namespace Sidekick.Business.Leagues
{
    public interface ILeagueService
    {
        List<League> Leagues { get; }
    }
}
