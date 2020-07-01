// using AutoFixture;
// using ResidentInformationApi.V1.Domain;
// using ResidentInformationApi.V1.Gateways;
// using FluentAssertions;
// using NUnit.Framework;
// using ResidentInformationApi.Tests.V1.Helper;

// namespace ResidentInformationApi.Tests.V1.Gateways
// {
//     [TestFixture]
//     public class ExampleGatewayTests : DatabaseTests
//     {
//         private Fixture _fixture = new Fixture();
//         private ExampleGateway _classUnderTest;

//         [SetUp]
//         public void Setup()
//         {
//             _classUnderTest = new ExampleGateway(DatabaseContext);
//         }

//         [Test]
//         public void GetEntityByIdReturnsEmptyArray()
//         {
//             var response = _classUnderTest.GetEntityById(123);

//             response.Should().BeNull();
//         }

//         [Test]
//         public void GetEntityByIdReturnsCorrectResponse()
//         {
//             var entity = _fixture.Create<Entity>();
//             var databaseEntity = DatabaseEntityHelper.CreateDatabaseEntityFrom(entity);

//             DatabaseContext.DatabaseEntities.Add(databaseEntity);
//             DatabaseContext.SaveChanges();

//             var response = _classUnderTest.GetEntityById(databaseEntity.Id);

//             databaseEntity.Id.Should().Be(response.Id);
//         }
//     }
// }
