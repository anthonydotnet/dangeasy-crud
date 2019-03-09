using System;
using DangEasy.Crud;
using Example.Console.Models;
using DangEasy.CosmosDb.Repository;
using DangEasy.Interfaces.Database;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Linq;
using Microsoft.Azure.Documents;
using DangEasy.Crud.ResponseModels;

namespace Example.Console
{
    class Program
    {
        public static IConfigurationRoot Configuration { get; set; }

        internal Program()
        {
            var builder = new ConfigurationBuilder()
                           .SetBasePath(Directory.GetCurrentDirectory())
                           .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            Configuration = builder.Build();
        }

        static void Main(string[] args)
        {
            var p = new Program();
            p.Run();
        }


        private void Run()
        {
            // get config values for CosmosDB
            var endpointUrl = Configuration["AppSettings:EndpointUrl"];
            var authorizationKey = Configuration["AppSettings:AuthorizationKey"];
            var databaseName = Configuration["AppSettings:DatabaseName"];
            var collectionName = typeof(Profile).Name;

            // instantiate IRepository
            DocumentClient documentClient = new DocumentClient(new Uri(endpointUrl), authorizationKey);
            IRepository<Profile> repository = new DocumentDbRepository<Profile>(documentClient, databaseName, collectionName);

            // instantiate service
            var service = new CrudService<Profile>(repository);


            // create 1
            var poco1 = new Profile
            {
                Id = "1234",
                FirstName = "Milo",
                LastName = "Aukerman",
                AccountId = "1",
                Occupation = ""
            };
            var createResponse = service.CreateAsync(poco1).Result;
            if (createResponse.Success)
            {
                System.Console.WriteLine($"CreateAsync. {createResponse.Success} {createResponse.StatusCode} {createResponse.Data.Id}");
            }


            // create with duplicate ID
            var poco2 = new Profile
            {
                Id = "1234",
                FirstName = "Bill",
                LastName = "Stevenson",
                AccountId = "2"
            };
            createResponse = service.CreateAsync(poco1).Result;
            if (createResponse is ConflictResponse<Profile> typedConflictRes)
            {
                System.Console.WriteLine($"CreateAsync. {createResponse.Success} {createResponse.StatusCode} {typedConflictRes.Id}");
            }


            // get
            var getResponse = service.GetByIdAsync("1234").Result;
            System.Console.WriteLine($"\nGetByIdAsync {getResponse.Success} {getResponse.StatusCode}.");
            if (getResponse is GetResponse<Profile> typedGetRes)
            {
                System.Console.WriteLine($"{typedGetRes.Data.Id}, {typedGetRes.Data.FirstName} {typedGetRes.Data.LastName}");
            }


            // update
            var docToUpdate = service.GetByIdAsync("1234").Result as GetResponse<Profile>;
            var modifiedModel = docToUpdate.Data;
            modifiedModel.Occupation = "Singer";

            var updateResponse = service.UpdateAsync(modifiedModel).Result;
            System.Console.WriteLine($"\nUpdateAsync. {updateResponse.Success} {updateResponse.StatusCode} " +
                "{updateResponse.Data.Id}, {updateResponse.Data.FirstName} {updateResponse.Data.LastName}, {updateResponse.Data.Occupation}");

            var nonExistingDoc = new Profile { Id = "Not-a-real-id" };
            updateResponse = service.UpdateAsync(nonExistingDoc).Result;
            if (updateResponse is UpdateNotFoundResponse<Profile> typedNotFoundRes)
            {
                System.Console.WriteLine($"\nUpdateAsync {typedNotFoundRes.Success} {typedNotFoundRes.StatusCode} - {typedNotFoundRes.Id}");
            }


            // get all
            var responseQueryable = service.GetAllAsync().Result;
            System.Console.WriteLine($"\nGetAllAsync {responseQueryable.Success} {responseQueryable.StatusCode}");
            responseQueryable.Data.ToList().ForEach(x => System.Console.WriteLine($"{x.Id}, {x.FirstName} {x.LastName}"));


            // query 
            responseQueryable = service.QueryAsync("SELECT * FROM Profile p WHERE p.firstName = 'Milo'").Result;
            System.Console.Write($"\nQueryAsync {responseQueryable.Success} {responseQueryable.StatusCode}");
            if (responseQueryable is QueryResponse<Profile> typedQueryRes)
            {
                typedQueryRes.Data.ToList().ForEach(x => System.Console.WriteLine($"{x.Id}, {x.FirstName} {x.LastName}"));
            }


            // count
            var countResponse = service.CountAsync().Result;
            System.Console.WriteLine($"CountAsync {countResponse.Success} {countResponse.StatusCode} - {countResponse.Count}");


            // count by query
            countResponse = service.CountAsync("SELECT * FROM Profile p WHERE p.firstName = 'Milo'").Result;
            System.Console.WriteLine($"\nCountAsync by SQL. {countResponse.Success} {countResponse.StatusCode} - {countResponse.Count}");



            // first or default by query
            var firstResponse = service.FirstOrDefaultAsync("SELECT * FROM Profile p WHERE p.firstName = 'Milo'").Result;
            System.Console.WriteLine($"\nFirstOrDefaultAsync {firstResponse.Success} {firstResponse.StatusCode}.");
            System.Console.WriteLine($"{firstResponse.Data.Id}, {firstResponse.Data.FirstName} {firstResponse.Data.LastName}");



            // delete 
            var responseDelete = service.DeleteAsync("1234").Result;
            System.Console.WriteLine($"\nDeleteAsync {responseDelete.Success} {responseDelete.StatusCode} - {responseDelete.Id}");


            // delete 
            responseDelete = service.DeleteAsync("Not-a-real-ID").Result;
            System.Console.WriteLine($"\nDeleteAsync {responseDelete.Success} {responseDelete.StatusCode} - {responseDelete.Id}");

            if (responseDelete is DeleteNotFoundResponse typedNotFoundDelRes)
            {
                System.Console.WriteLine($"{typedNotFoundDelRes.Id}");
            }


            AddStoredProc(documentClient, databaseName, collectionName, "my_sproc");
            var responseSproc = service.ExecuteStoredProcedureAsync<string>("my_sproc", "Milo").Result;
            System.Console.WriteLine($"\nExecuteStoredProcedureAsync {responseSproc.Success} {responseSproc.StatusCode} - {responseSproc.Data}");


            // cleanup
            System.Console.WriteLine($"\nDeleting {databaseName}");
            documentClient.DeleteDatabaseAsync(UriFactory.CreateDatabaseUri(databaseName)).Wait();
        }



        private static void AddStoredProc(DocumentClient documentClient, string databaseName, string collectionName, string sprocName)
        {
            var sproc = new StoredProcedure
            {
                Id = sprocName,
                Body = @"function (name){
                          // do something, then return null
                            var response = getContext().getResponse();
                            response.setBody('hello' + name);
                       }"
            };

            var uri = UriFactory.CreateDocumentCollectionUri(databaseName, collectionName);
            documentClient.CreateStoredProcedureAsync(uri, sproc).Wait();
        }
    }
}
