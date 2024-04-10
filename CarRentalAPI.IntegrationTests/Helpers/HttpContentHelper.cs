using Newtonsoft.Json;
using System.Text;

namespace CarRentalAPI.IntegrationTests.Helpers
{
    public static class HttpContentHelper
    {
        public static HttpContent ToHttpContent(this object obj)
        {
            var json = JsonConvert.SerializeObject(obj);
            return new StringContent(json, UnicodeEncoding.UTF8, "application/json");
        }
    }
}