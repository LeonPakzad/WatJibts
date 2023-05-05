# WatJibts
WatJibts is a planning tool for lunch sessions: - when does who eat what and where does who get what food from

## Installation
###
firstly connect your database. We used mysql, you might need to change your db connection in Program.cs and change your connectionString in appsettings.json

### install migration tool
```
dotnet tool install --global dotnet-ef
```

### migrate
```
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### run
```
dotnet run
```

### enjoy