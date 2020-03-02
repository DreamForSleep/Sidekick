using System.Net.Http;
using System.Text.Json;

namespace PathOfExile.Apis.Official
{
    public interface IPoeApi
    {
        Language Language { get; set; }
        HttpClient Client { get; }
        JsonSerializerOptions JsonOptions { get; }
    }
}
