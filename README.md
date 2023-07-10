# WatJibts README

## Explanation
WatJibts is a planning tool for lunch sessions: - when does who eat what and where does who get what food from.

## Installation
### step 0
point with the terminal of your choice to the "src" directory

### step 1: connect your database. 
_I used mysql, you might need further changes in the  Program.cs if you want to use a different database_

create a db `wat-jibts`
change your connectionString in appsettings.json

### step 2: mirgrate
install the migration tool:

```
dotnet tool install --global dotnet-ef
```

add current models to a database migration and migrate, "InitialMigration" is just an example name

```
dotnet ef migrations add InitialMigration
dotnet ef database update
```

### step 3: email via sendgrid
add sendgrid api key, go to https://sendgrid.com and set the user-secrets as follows:

```
dotnet user-serets init
dotnet user-secrets set SendGridKey <key>
```

### step 4: run
```
dotnet run
```

### enjoy :)