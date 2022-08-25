# .NET Crud Api

This project was built in ASP.NET Core API for netcoreapp3.1.
A Test project is also included and can/may be run within the Visual Studio.

Open the project in Visual Studio 2019 or 2022.

Initially this project uses memory list strategy as data storage, to persist the data, we have to change the IDataContextFactory from InMemoryDataContextFactory to EfDataContextFactory in the Startup.cs.
The EfDataContextFactory requires a connection string to the target database (SqlServer). Included here is a simple schema (CrudApiDatabase.sql) to create the tables for this project.

Please bare with the lengthy codes of EfDataContextFactory and InMemoryDataContextFactory. I have copied them from my CrudDatastore github repository.


