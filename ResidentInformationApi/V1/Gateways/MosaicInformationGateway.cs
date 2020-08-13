using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ResidentInformationApi.V1.Boundary.Requests;
using ResidentInformationApi.V1.Domain;
using ResidentInformationApi.V1.Gateways.Helpers;

namespace ResidentInformationApi.V1.Gateways
{
    public class MosaicInformationGateway : IMosaicInformationGateway
    {
        private readonly HttpClient _client;
        //private readonly string _baseUrl;
        public MosaicInformationGateway(HttpClient client)
        {
            _client = client;
            client.DefaultRequestHeaders.Add("X-API-Key", Environment.GetEnvironmentVariable("MOSAIC_API_KEY"));

        }
        public async Task<List<MosaicResidentInformation>> GetResidentInformation(ResidentQueryParam rqp)
        {
            var rqpString = DictionaryBuilder.BuildQueryDictionary(rqp);
            var builder = new UriBuilder();
            builder.Query = rqpString;
            var response = await _client.GetAsync(new Uri("api/v1/residents" + builder.Query, UriKind.Relative)).ConfigureAwait(true);

            //throw exception if not 200
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(true);
            var results = JsonConvert.DeserializeObject<MosaicResidentResponse>(content);

            return results.Residents;
        }
    }
}
