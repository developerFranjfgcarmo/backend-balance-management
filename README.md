# Balance Management

## Estructura

```
fullstack-owner-property-management
├── BalanceManagement.Api --> Web Api.
├── BalanceManagement.Contracts --> Contratos con la  WebApi
├── BalanceManagement.Data --> Capa de persistencia a datos.
├── BalanceManagement.Service --> Servicios
└── BalanceManagement.Test --> Test
```
## Getting started.
1. Sql Server. Crear la base de datos: BalanceManagement. El connectionstring  está configurado para una base de datos local(versión Developer)con windows Authentication.El connectionstring debe ser cambiado en el archivo: appsettings.json para que la api funcione y en la clase DesignTimeDbContextFactory  para que las migraciones funciones.
```
"Server=.\\;Database=BalanceManagement;Trusted_Connection=True;"
```
2. Migrations. Establecer el proyecto BalanceManagement.Data a por defecto: "Set as startup project" y lanzar el comando Updata-Database desde Package Manager Console. En el Package Manager Console, debe estar seleccionado el proyecto BalanceManagement.Data.
4. Establecer el proyecto BalanceManagement.Api a "Set as startup project" y lanzar la aplicación.
5. En el repositorio hay una carpeta con el nombre Postman. Aquí están todos los end point de la api. También se pueden ver con swagger.
6. Crear un usuario con el método: "./api/user". Hay dos tipos de usuario:Admin=1 y User=2. Este método es público.
7. Una vez creado los usuario hacer login: "api/login/authenticate"
8. El token está establecido como una variable local.
 ![](src/images/token.PNG)

## How it work
Asp.Net core:
- JWT authentication using ASP.NET Core JWT Bearer Authentication.
- SeriLog.
- Automapper.
- Entity Framework Core.
- Xunit. Test
- FluentValidation. Validación de los contratos.
- Sqlite.Test con EF core.
- Swagger. End points: https://localhost:44311/swagger/index.html

## Resumen.

- He creado las secciones "/User/" y "/account/". Cada una de las acciones de estos controladores, están securizada para el Rol que corresponda: Admin o User. Además en algunos Action, he establecido verificaciones, por ejemplo que el usuario sólo pueda acceder a sus cuentas.
- He documentado un poco la api con swagger, el código no lo he visto necesario, porque no hay ningún proceso que lo requiera. Siempre intento poner un 
