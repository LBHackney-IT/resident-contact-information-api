// using AutoFixture;
// using ResidentInformationApi.V1.Domain;
// using ResidentInformationApi.V1.Infrastructure;

// namespace ResidentInformationApi.Tests.V1.Helper
// {
//     public static class DatabaseEntityHelper
//     {
//         public static DatabaseEntity CreateDatabaseEntity()
//         {
//             var entity = new Fixture().Create<Entity>();

//             return CreateDatabaseEntityFrom(entity);
//         }

//         public static DatabaseEntity CreateDatabaseEntityFrom(Entity entity)
//         {
//             return new DatabaseEntity
//             {
//                 Id = entity.Id,
//             };
//         }
//     }
// }
