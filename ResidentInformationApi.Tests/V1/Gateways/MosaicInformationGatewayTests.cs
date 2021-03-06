using System;
using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using ResidentInformationApi.Tests.V1.Helper;
using ResidentInformationApi.V1.Boundary.Requests;
using ResidentInformationApi.V1.Domain;
using ResidentInformationApi.V1.Gateways;
using System.Collections.Generic;

namespace ResidentInformationApi.Tests.V1.Gateways
{
    [TestFixture]
    public class MosaicInformationGatewayTests
    {
        private Fixture _fixture;
        private MosaicInformationGateway _classUnderTest;
        private Mock<HttpMessageHandler> _messageHandler;
        private Uri _uri;
        private string _currentEnv;
        private string _apiToken;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _uri = new Uri("http://test-domain-name.com/");
            _currentEnv = Environment.GetEnvironmentVariable("MOSAIC_API_URL");
            Environment.SetEnvironmentVariable("MOSAIC_API_URL", _uri.OriginalString);
            _apiToken = Environment.GetEnvironmentVariable("MOSAIC_API_TOKEN");
            Environment.SetEnvironmentVariable("MOSAIC_API_TOKEN", "secretKey");
            _messageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            var _httpClient = new HttpClient(_messageHandler.Object)
            {
                BaseAddress = _uri,
            };

            _httpClient.DefaultRequestHeaders.Add("Authorization", Environment.GetEnvironmentVariable("MOSAIC_API_TOKEN"));
            _classUnderTest = new MosaicInformationGateway(_httpClient);
        }

        [TearDown]
        public void TearDown()
        {
            Environment.SetEnvironmentVariable("MOSAIC_API_URL", _currentEnv);
            Environment.SetEnvironmentVariable("MOSAIC_API_TOKEN", _apiToken);

        }

        [Test]
        public async Task ApiTokenSuccessfullyCalled()
        {
            var rqp = new ResidentQueryParam();
            TestHelper.SetUpMessageHandlerToReturnJson(_messageHandler, "residents", expectedJsonString: "{residents: []}", expectedApiToken: "secretKey");
            await _classUnderTest.GetResidentInformation(rqp).ConfigureAwait(true);
            _messageHandler.Verify();

        }

        [Test]
        public async Task GetResidentInformationReturnsEmptyArrayIfNoResultsFound()
        {
            var rqp = new ResidentQueryParam();
            TestHelper.SetUpMessageHandlerToReturnJson(_messageHandler, "residents", expectedJsonString: "{residents: []}");
            var received = await _classUnderTest.GetResidentInformation(rqp).ConfigureAwait(true);

            received.Should().BeEmpty();
            received.Should().NotBeNull();
        }

        [Test]
        public async Task GetResidentInformationReturnsArrayOfResidentInformationObjects()
        {
            var rqp = new ResidentQueryParam { Address = "Address Line 1" };
            var expected = _fixture.CreateMany<MosaicResidentInformation>();
            var expectedJson = JsonConvert.SerializeObject(expected);
            TestHelper.SetUpMessageHandlerToReturnJson(_messageHandler, "residents", "?address=" + rqp.Address, "{residents: " + expectedJson + "}");

            var received = await _classUnderTest.GetResidentInformation(rqp).ConfigureAwait(true);

            _messageHandler.Verify();
            received.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task GetResidentInformationThrowsErrorIfAPIReturnsBadRequest()
        {
            var rqp = new ResidentQueryParam();
            TestHelper.SetUpMessageHandlerToReturnErrorCode(_messageHandler);
            Func<Task<List<MosaicResidentInformation>>> testFunction = () => _classUnderTest.GetResidentInformation(rqp);

            await testFunction.Should().ThrowAsync<HttpRequestException>().ConfigureAwait(true);
        }
    }
}
