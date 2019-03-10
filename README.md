dangeasy-crud

A simple CRUD library for dotnet Standard 2.0. 

This library exposes an intuitive interface, methods, models and events for doing CRUD in your application. A standard repository pattern is used, and returns strongly typed models which are easy to use and understand. DangEasy.Crud does all the mundane CRUD work so you can concentrate on the real logic of your application


Features
- Repository Pattern
- Strongly typed response models
- Events to add more logic during CRUD operations
- Implement your own IRepository to use any type of database
- Success & status codes
- Caught Exceptions returned as response models

It's Dang Easy! It just works!


[![Nuget](https://img.shields.io/badge/nuget-0.1.0-blue.svg?maxAge=3600)](https://www.nuget.org/packages/DangEasy.Crud)


# Installation

Use NuGet to install the [package](https://www.nuget.org/packages/DangEasy.Crud/).

```
PM> Install-Package DangEasy.Crud
```

# Getting started

## Step 1: Instantiate your IRepository

In this example, I am using my [CosmosDB repository](https://www.nuget.org/packages/DangEasy.CosmosDb.Repository/).

```csharp

		// instantiate IRepository - In this case I'm using my CosmosDB repository 
        DocumentClient documentClient = new DocumentClient(new Uri(endpointUrl), authorizationKey);
        IRepository<Profile> repository = new DocumentDbRepository<Profile>(documentClient, databaseName, collectionName);

        // instantiate the CRUD service
        var service = new CrudService<Profile>(repository);
```


## Step 2: Just start using it!

Full example below...


```csharp
internal class Program
{
    public DocumentClient Client { get; set; }

	private static void Main(string[] args)
	{		
		// instantiate IRepository - In this case I'm using my CosmosDB repository
        DocumentClient documentClient = new DocumentClient(new Uri(endpointUrl), authorizationKey);
        IRepository<Profile> repository = new DocumentDbRepository<Profile>(documentClient, databaseName, collectionName);

        // instantiate the CRUD service
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


        var responseSproc = service.ExecuteStoredProcedureAsync<string>("my_sproc", "Milo").Result;
        System.Console.WriteLine($"\nExecuteStoredProcedureAsync {responseSproc.Success} {responseSproc.StatusCode} - {responseSproc.Data}");


	}
}    
```