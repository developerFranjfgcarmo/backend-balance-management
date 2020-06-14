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

### BalanceManagement.Api
```
├── Auth --> Servicio para generar la Autenticacion del usuario
├── Controllers
├── Extensions --> Metódo de extensión para registrar Servicios en la clase Startup
├── Filters --> Filtro que se encarga de validar los Dtos.
├── Middleware --> Middleware para capturar las excepcion.
└── Validator --> Rules de FluentValidations
```

### BalanceManagement.Contracts
```
├── Dtos
└── Mapper --> Configuración de automapper y algunos métodos extensión.
```

### BalanceManagement.Data
```
├── Configurations --> Configuraciones de las entidadesde EF con Fluent Api
├── Context. Contexto de EF. También está la clase DesignTimeDbContextFactory que permite lanzar las migraciones.
├── Entities 
├── Extensions --> Metódos de extensión para cargar las configuraciones de las entidades y los seeds
├── Migrations
└── Types. Enum con los Roles
```
### BalanceManagement.Service
```
├── IService --> Contratos
├── Attributes --> La clase TransactionAsyncAttribute, utiliza el paquete MrAdvice, el cual permite utilizar la programación orientada a aspectos.
└── Service--> Implementaciones de los servicios.
```
### BalanceManagement.Test
```
├── Mocks --> La clase DatabaseFixture permite compartir la conexión de SqlLite con todos los test.
└── Service--> Test. He añadido dos test para testear los servicio. Uno para añadir saldo a un usuario y otro para transferir saldo  a otro usuario.
```
## Getting started.
1. Sql Server. Crear la base de datos: BalanceManagement. El connectionstring  está configurado para una base de datos local(versión Developer)con windows Authentication.El connectionstring debe ser cambiado en el archivo: appsettings.json para que la api funcione y en la clase DesignTimeDbContextFactory  para lanzar las migraciones.
```
"Server=.\\;Database=BalanceManagement;Trusted_Connection=True;"
```
2. Migrations. Establecer el proyecto BalanceManagement.Data a por defecto: "Set as startup project" y lanzar el comando Updata-Database desde Package Manager Console. En el Package Manager Console, debe estar seleccionado el proyecto BalanceManagement.Data.
4. Establecer el proyecto BalanceManagement.Api a "Set as startup project" y lanzar la aplicación.
5. En el repositorio hay una carpeta con el nombre Postman(BalanceManagement.Api.postman_collection.json). Aquí están todos los end point de la api. En el título de la petición indico los roles que pueden utilizarla. También se pueden ver con swagger.
6. Crear un usuario a través del end point: "./api/user". Hay dos tipos de usuario:Admin=1 y User=2. Este método es público. Se recomienda crar un usuario administrador y un usuario normal.
7. Una vez creado los usuario hacer login en el siguiente end point: "api/login/authenticate"
8. El token está establecido como una variable global en el postman. Se recomienda generar y guardar el token de los usuarios.
 ![](src/images/token.PNG)

## Postman.
Tiene cuatro carpeta:
1. User. Gestión de Usuarios.
2. Account. Gestión de cuentas
3. Login.
4. Balance. Permite añadir, eliminar o transferir balance a la cuenta de otro usuario.

## How it work
Asp.Net core:
- JWT authentication using ASP.NET Core JWT Bearer Authentication.
- SeriLog. Cuando se enta en desarrollo se muestra los log en la ventana de output, está configurado para que en otro entorno escriba a un log.
- Automapper.
- Entity Framework Core.
- Xunit. Test
- FluentValidation. Validación de los contratos y también se documenta en swagger las validaciones.
- Sqlite.Test con EF core.
- Swagger. End points: https://localhost:44353/swagger/index.html

## Resumen.

- He creado las secciones "/User/" y "/account/". Cada una de las acciones de estos controladores, están securizada para el Rol que corresponda: Admin o User. Además en algunos Action, he establecido verificaciones, por ejemplo que el usuario sólo pueda acceder a sus cuentas.
- He documentado un poco la api con swagger. El código, sólo aquellos métodos que he visto que tienen alguna complejidad. Siempre intento poner un naming que aporte información o que me diga cual es su finalidad. 
