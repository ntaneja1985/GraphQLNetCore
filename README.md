# GraphQL with .Net Core
Using Graph QL with .NET Core

## What is Graph QL? 
- A query language to construct and retrieve data from APIs 
- Invented by Facebook in 2012 
- Open sourced in 2015 
- lot of companies now use it.
- Language and Platform Independent, just has to support JSON 

## Why we need GraphQL? How does it compare to REST 
- Solves the problem of underfetching and overfetching.
- Lets say we want to get all posts of a user, comments on user posts and likes on user posts 
- We will have APIs like this 
- ![alt text](image.png)
- We can see that to get data for a single post including comments and likes we need 3 endpoints.
- This is called under-fetching because a single endpoint will not return all the data. 
- Over-fetching means to get extra data which is not required. 
- ![alt text](image-1.png)
- ![alt text](image-2.png)
- We dont need all these fields and they just increase JSON payload size. 
- ![alt text](image-3.png)
- In GraphQL we just need one single endpoint to get all the results. 
- In the GraphQL query we will pass the userId and we will specify what kind of data we want to retrieve from the server. 
- ![alt text](image-4.png)
- Now there is no need 3 different requests to fetch the data. We can get all the results in one go. 
- Think of facebook, why they invented GraphQL, they wanted to overcome underfetching and overfetching of data.
- GraphQL is not a replacement of REST. 
- When the project scope is bigger use GraphQL, otherwise use REST. 
- For simple APIs, REST works fine but for complex data, GraphQL really helps.

## Graph QL Query 
- A GraphQL query is used to read or fetch data. 
- In REST APIs we use GET but GraphQL uses Queries. 
```c#
query MyFirstQuery {
  viewer{
    id
    name
    avatarUrl
    login
    createdAt
  }
}

```
- To pass arguments in GraphQL queries use this 
```c#
query {
  repository(name:"graphql" owner:"facebook"){
    createdAt
    id
    description
  }
}

```
## Working with GraphQL Schema
- When we developer creates a GraphQL API, they also define a schema for the GraphQL 
- When the developer runs the GraphQL server, the GraphQL schema is generated.
- This schema provides information about the fields, their descriptions, the types they belong to and the arguments required for querying.
- GraphQL schema is like API Documentation.
- Graph QL contains 2 main root types: query and mutation. 
- Query is used for querying the data and mutation is used for adding, updating or deleting the data.
- We can use GraphQL schema to construct queries. 

## Using Aliases in GraphQL 
- Allow us to retrieve specific data from specific fields by assigning them user friendly names. 
```c#
query {
  dotnetcoreRepo: repository(name:"core" owner:"dotnet"){
    createdAt
    id
    description
  } 
  wpfrepo: repository(name:"wpf" owner:"dotnet"){
    createdAt
    id
    description
  }
}


```
## Creating Fragments in GraphQL 
- Allow us to create reusable set of fields which we can use in our query.
```c#
query {
  dotnetcoreRepo: repository(name:"core" owner:"dotnet"){
    ... RepositoryCommonFields
  } 
  wpfrepo: repository(name:"wpf" owner:"dotnet"){
    ... RepositoryCommonFields
  }
}

fragment RepositoryCommonFields on Repository{
  createdAt
  id
  description
  url
}


```

## Using Variables in GraphQL queries 
- We will pass dynamic values using variables to make our queries more flexible 
- Here $name, $owner is the name of our variables and we can pass variables to our query.
  ```c#
  query DotNetRepo($name:String! $owner:String!) {
  repository(name:$name owner:$owner){
    id 
    createdAt
    description
    url
        }
    }

  ```

  ## Modifying Data with Mutations 
  - Mutations are responsible for creating, updating and deleting the data 
  - ![alt text](image-5.png)
  ```c#
    mutation AddProject($input:CreateProjectInput!){
    createProject(input:$input){
        clientMutationId
        project{
        id
        createdAt
        url
        }
    }
    }

  ```
  - We can specify variables like this 
  ```c#
    {
	  "input":{
      "ownerId": "U_kgDOBY34TA",
      "name": "MyFirstGraphQLProject",
      "body": "First project via mutation",
      "clientMutationId": "123456789"
      }
    

  ```
   ## Integrating GraphQL with .NET Core 
  - ![alt text](image-6.png)
  - Install GraphQL.Server.Transports.AspNetCore package
  - We will also install GraphiQL package. This package transforms our web browser into a graphical playground. 
  - Helps to test our GraphQL endpoints for the web browser.
  - Please note GraphQL doesnot understand our model classes. Instead it understands types, so our GraphQL endpoints need to return data based on these types.
  - We must establish mapping between our Model Classes and GraphQL types.
  - This can be done as follows. Lets say we have the following Model Class :
  ```c#
  public class Menu
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
  }

  ```
  - Now we can define a GraphQL type like this which maps from Menu Class 
```c#
 public class MenuType : ObjectGraphType<Menu>
{
    public MenuType()
    {
        Field(x => x.Id);
        Field(x => x.Name);
        Field(x => x.Description);
        Field(x=>x.Price);
    }
}

```
- In case we have more than model class, we need to setup GraphQL types for each of the additional classes.

### Working with GraphQL queries
- Here is the mapping between GraphQL datatypes and c# data type 
```c#
![alt text](image-7.png)

```
- Graph QL queries can be written like this 
```c#
 public class MenuQuery : ObjectGraphType
{
    public MenuQuery(IMenuRepository menuRepository)
    {
        Field<ListGraphType<MenuType>>("Menus").Resolve(context =>
        {
            return menuRepository.GetAllMenus();
        });
        Field<MenuType>("Menu").Arguments(new QueryArguments(new QueryArgument<IntGraphType> {Name = "menuId" })).Resolve(context =>
        {
            return menuRepository.GetMenuById(context.GetArgument<int>("menuId"));
        });
    }
    
}


```

### GraphQL schemas 
- In GraphQL schema we have queries and mutations. 
- We need to resolve queries in GraphQL schema
- We can register our GraphQL queries within the schema like this 
```c#
 public class MenuSchema: GraphQL.Types.Schema
{
    public MenuSchema(MenuQuery menuQuery)
    {
        Query = menuQuery;
    }
}


```
### Registering GraphQL in Program.cs 
- This can be done as follows:
```c#
 builder.Services.AddControllers();
builder.Services.AddTransient<IMenuRepository,MenuRepository>();
builder.Services.AddTransient<MenuType>();
builder.Services.AddTransient<MenuQuery>();
builder.Services.AddTransient<ISchema, MenuSchema>();
//Autogenerate the schema based on specified schema interface or type
//Also we want the response in JSON format.
builder.Services.AddGraphQL(b => b.AddAutoSchema<ISchema>().AddSystemTextJson());

```
- We also need to define a graphical playground middleware to our request pipeline. Here we will pass the path that will be used to open the GraphQL playground for playing GraphQL queries inside our browser. 
- Consider the GraphQL path like a route URL .
- ![alt text](image-8.png)
```c#
 //Here on this path the graphical playground is opened
  app.UseGraphiQl("/graphql");
  app.UseGraphQL<ISchema>();

```
- ![alt text](image-9.png)

### GraphQL Mutations
- Since add Menu will contain an input of type menu we need to define a menu input type as follows: 
```c#

 public class MenuInputType: InputObjectGraphType
{
    public MenuInputType()
    {
        Field<IntGraphType>("id");
        Field<StringGraphType>("name");
        Field<StringGraphType>("description");
        Field<FloatGraphType>("price"); 

    }
}

```
- Now we need to define a class called MenuMutations 
  ```c#
   public class MenuMutation : ObjectGraphType
  {
    public MenuMutation(IMenuRepository menuRepository) 
     {
        Field<MenuType>("CreateMenu").Arguments(new QueryArguments(new QueryArgument<MenuInputType> { Name = "menu" })).Resolve(context =>
        {
            return menuRepository.AddMenu(context.GetArgument<Menu>("menu"));
        });

        Field<MenuType>("UpdateMenu").Arguments(new QueryArguments(new QueryArgument<IntGraphType> { Name = "menuId" }, new QueryArgument<MenuInputType> { Name = "menu" })).Resolve(context =>
        {
            return menuRepository.UpdateMenu(context.GetArgument<int>("menuId"),context.GetArgument<Menu>("menu"));
        });

        Field<BooleanGraphType>("DeleteMenu").Arguments(new QueryArguments(new QueryArgument<IntGraphType> { Name = "menuId" })).Resolve(context =>
        {
             menuRepository.DeleteMenu(context.GetArgument<int>("menuId"));
             return true;
        });


      }
  }


  ```
  - Now we need to register these mutations in the MenuSchema like this 
  ```c#
  public class MenuSchema: GraphQL.Types.Schema
  {
    public MenuSchema(MenuQuery menuQuery, MenuMutation menuMutation)
    {
        Query = menuQuery;
        Mutation = menuMutation;
    }
  }


  ```
  - Finally we need to register all this in the Program.cs file like this 
  ```c#
  builder.Services.AddTransient<IMenuRepository,MenuRepository>();
  builder.Services.AddTransient<MenuType>();
  builder.Services.AddTransient<MenuQuery>();
  builder.Services.AddTransient<MenuMutation>();
  builder.Services.AddTransient<MenuInputType>();
  builder.Services.AddTransient<ISchema, MenuSchema>();
  builder.Services.AddGraphQL(b => b.AddAutoSchema<ISchema>().AddSystemTextJson());

  ```
  - ![alt text](image-10.png)
  - ![alt text](image-11.png)
  - ![alt text](image-12.png)


 
  