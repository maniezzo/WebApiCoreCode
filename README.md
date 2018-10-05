# WebApiCoreCode
Classroom project for business analytics: visual studio code access to webapi based on asp.net core and ado.et core

## Tecnologies for Use:
* make sure you have installed [visual studio code](https://code.visualstudio.com/) (and [.NET Core SDK](https://www.microsoft.com/net/download))
* add the [C# plugin](https://marketplace.visualstudio.com/items?itemName=ms-vscode.csharp)

## Steps to create the project from scratch:
* open a terminal:
    ```sh
    $ mkdir myproject
    $ cd my project
    $ dotnet new console
    ```
* open the file .csproj and add, into the ItemGroup section, the dependencies that you want to.
In this example:
    ><PackageReference Include="Newtonsoft.Json" Version="11.0.2" />
	><PackageReference Include="Microsoft.EntityFrameworkCore.Sqlite" Version="1.1.2" />
	><PackageReference Include="System.Data.SQLite" Version="1.0.105" />
	><PackageReference Include="System.Data.SQLite.Mac" Version="1.0.104.2" />
* you can restore your project through vs code or opening the terminal and type:
    ```sh
    $ dotnet restore
    ```
* write your code, debug it, and run with:
    ```sh
    $ dotnet run
    ```
## Extras

### Run SqlServer on Unix OS

* install [Docker](https://www.docker.com/get-started) and [SQL Operations Studio](https://database.guide/what-is-sql-operations-studio-sqlops/)
* increase docker memory: 
    * select Preferences from the little Docker icon in the top menu
    * slide the memory slider up to at least 4GB
    * click Apply & Restart
* download SQL Server opening a terminal and write:
    ```sh
    $ docker pull microsoft/mssql-server-linux
    ```
* launch the docker image:
    ```sh
    $ docker run -d --name sql_server_demo -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=reallyStrongPwd123' -p 1433:1433 microsoft/mssql-server-linux
    ```
* install sql-cli:
    ```sh
    $ npm install -g sql-cli
    ```
* connect to SQL Server
    ```sh
    $ mssql -u sa -p reallyStrongPwd123
    ```
* open SQL Ops and use:
    >Server: 127.0.0.1
    >User name: sa
    >Password: reallyStrongPwd123
* for the next times, make sure that the container is running, otherwise:
    ```sh
    $ docker container ls --all
    $ docker container start CONTAINERID
    ```
    
### Browse SQLite Db
* Install [DB Browser for SQLite](http://sqlitebrowser.org/)
=======
Classroom project for business analytics: visual studio code access to webapi based on asp.net core and ado.net core
