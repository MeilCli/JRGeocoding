using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace JRGeocoding.Core
{
    public class HttpContentReader : IContentReader
    {
        private readonly HttpClient client = new HttpClient();
        private readonly Uri baseUrl;

        public HttpContentReader(string baseUrl)
        {
            this.baseUrl = new Uri(baseUrl);
        }

        public async ValueTask<string?> ReadContentAsync(string fileName)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(new Uri(baseUrl, fileName));
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException)
            {
                throw new GeocodingException("Not found address or content");
            }
        }
    }
}
