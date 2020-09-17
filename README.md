# About the project
A simple application written in C# (.NET Core) that stores information about companies in Azure SQL Database, including a list of employees and the possibility of management via RESTful Web API. The service starts in self-host mode (in the console).
# How to run the application
Just run the console app and go to https://localhost:5001/company/list
# Supported HTTP queries
Soon I will write about this here.
# Use your own SQL Server database
1. Modify the connection string
2. add a migration with .NET CLI: dotnet ef migrations add MyMigration
3. Update your SQL Server database: dotnet ef database update
# Author
Sebastian Bobrowski

