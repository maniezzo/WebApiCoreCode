# WebApiCoreCode
Classroom project for business analytics: visual studio code access to webapi based on asp.net core and ado.et core

## Tecnologies for Use:
* make sure you have installed [visual studio code](https://code.visualstudio.com/) (and [.NET Core SDK](https://www.microsoft.com/net/download))
* add the [C# plugin](https://marketplace.visualstudio.com/items?itemName=ms-vscode.csharp)

## FIRST STEP - Create the project from scratch
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

### Use of json instead of app.config

In general:
* In Visual Studio classic we use app.config to set all dependencies.
* In Visual Studio Core we use file 'setting.json' to resolve all dependencies. 

In our case:
* With 'connectionStrings' we define databases (SQLite, local SQLServer and Remote SQLServer). 
* With 'settings' we choose the specific database to connect and the mapping's method.

In the Controller constructor we check the current field of settings and apply the chosen method.


## SECOND STEP - Create a new WebAPI project and merge it with first project
After the creation and the editing of first project, we have worked with a second project for the realization of a Web API project. In Visual Studio Code there isn't the possibility to create immediately a new project (c#/web/ASP.NET Web Application) like in Visual Studio.  
* To create a new WebAPI project in Visul Studio Code, open a terminal and digits:
    ``` sh
    $ dotnet new webapi -o projectName
    $ code -r projectName
    ```
After this, we have merge the first project into this WebAPI project and we have worked on this.

### CORS problems
We have created an index.html file but when we launched it in localhost, we have encountered some problems. The origin of this problem is CORS:
* CORS definition: Cross-origin resource sharing (CORS) is a mechanism that allows restricted resources on a web page to be requested from another domain outside the domain from which the first resource was served. 
A web page may freely embed cross-origin images, stylesheets, scripts, iframes, and videos. Certain "cross-domain" requests, notably Ajax requests, are forbidden by default by the same-origin security policy.

In order to resolve this problem, we have added some lines of code in file Startup.cs. In particular:
* At line 28:
    ``` sh
    services.AddCors(options => options.AddPolicy("AllowAll", builder => 
                builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));
    ```
* At line 46:
    ``` sh
    app.UseCors("AllowAll");
    ```

### Controllers' managing
We have created a controller for every subpart of project (like CustomersController, ForecastingController, OptimizationController etc).
Every controller has a path with the name of the class (e.g.: ForecastingController has a path api/forecasting) and in his class we have inserted the method GET, PUT, POST and DELETE (if necessary).


### Certificates PWA problems
We have added support for PWA (Progressive Web App) but we have encountered a problem with certificates. In particular, the problem arise with browser Google Chrome. To fix this problem, we opened the certificate from browser and we set the security level in "always trusted".

## Extras

### Run SqlServer on Unix OS

* install [Docker](https://www.docker.com/get-started) and [SQL Operations Studio](https://docs.microsoft.com/en-us/sql/sql-operations-studio/download)
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



