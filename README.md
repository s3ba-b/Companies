# About the project
A simple application written in C# (.NET Core) that stores information about companies in Azure SQL Database, including a list of employees and the possibility of management via RESTful Web API. The service starts in self-host mode (in the console).
# How to run the application
Just run the console app and go to https://localhost:5001/company/list
# Supported HTTP queries
https://localhost:5001/company/create
```json
{
        "name": "First company",
        "establishmentYear": 1995,
        "employees": [
            {
                "firstName": "Sebastian",
                "lastName": "Bobrowski",
                "dateOfBirth": "1997-09-12T22:18:26.625Z",
                "jobTitle": 1,
            }
        ]
    }
```
https://localhost:5001/company/search
```json
{
    "Keyword": null,
    "EmployeeDateOfBirthFrom": "1995-09-12T22:18:26.625",
    "EmployeeDateOfBirthTo": "2000-09-12T22:18:26.625",
    "EmployeeJobTitles": null
}
```
https://localhost:5001/company/update/1
```
{
    "Name": "Updated",
    "EstablishmentYear": 2000,
    "Employees": []
}
```
https://localhost:5001/company/delete/1
# Apply own SQL Server instance database
1. Update connection string
2. Optional, on updating models: dotnet ef migrations add InitialCreate
2. Run: dotnet ef database update
# Authentication headers
Import Companies API request collection.postman_collection.json to your Postman app. These requests contain authentication headers. 
# Author
Sebastian Bobrowski

