# WatJibts
WatJibts is a planning tool for lunch sessions: - when does who eat what and where does who get what food from

## Installation
###
firstly connect your database. 

_I used mysql, you might need further changes in the  Program.cs if you want to use a different database_

change your connectionString in appsettings.json

### install migration tool
```
dotnet tool install --global dotnet-ef
```

### migrate
add current models to a database migration and migrate, "InitialMigration" is just an example name
```
dotnet ef migrations add InitialMigration
dotnet ef database update
```

### email via sendgrid
add sendgrid api key, go to https://sendgrid.com and set the user-secrets as follows::
```
dotnet user-serets init
dotnet user-secrets set SendGridKey <key>
```

### run
```
dotnet run
```

### enjoy