# Prueba Técnica - Microservicios .NET + SPA

Esta solución implementa la prueba técnica solicitada: dos microservicios independientes para gestión de usuarios y productos, más un cliente web que consume ambos mediante JWT. 

## Arquitectura

- **UserService** (ASP.NET Core Web API)
  - Registro de usuarios (Nombre, Email, Contraseña, Rol Admin/User).
  - Login con emisión de JWT.
  - Validaciones de email, contraseña mínima y email único.
  - Base de datos propia en PostgreSQL. 

- **ProductService** (ASP.NET Core Web API)
  - CRUD de productos (Id, Nombre, Descripción, Precio, Categoría, Estado activo/inactivo).
  - Reglas de negocio: precio ≥ 0, nombre y categoría obligatorios, estado booleano.
  - Autorización:
    - CRUD completo solo **Admin**.
    - Consultas `GET` para cualquier usuario autenticado. 
  - Base de datos propia en PostgreSQL. 

- **WebClient** (ASP.NET Core MVC, solo vistas)
  - Login / Logout contra UserService usando JWT.
  - Listado de productos, creación y edición consumiendo ProductService.
  - Validaciones en frontend y manejo visual de errores. 

---

## Requisitos previos

- .NET 7/8/9 SDK instalado.
- PostgreSQL en local (o Docker) con un usuario y contraseña configurados.

---

## UserService

### Configuración

1. Abrir la solución `UserService` en Visual Studio.
2. Ajustar `UserService.Api/appsettings.json`:

```
"ConnectionStrings": {
	"UserDatabase": "Host=localhost;Port=5432;Database=userservice_db;Username=TU_USUARIO;Password=TU_PASSWORD"
},
"Jwt": {
	"Key": "LLAVE_SECRETA_LARGA_AQUI",
	"Issuer": "UserService",
	"Audience": "UserServiceClients",
	"ExpiresInMinutes": 60
}
```

### Migraciones y base de datos

En la Consola del Administrador de paquetes (proyecto predeterminado `UserService.Infrastructure`):

Add-Migration InitialUserSchema -StartupProject UserService.Api
Update-Database -StartupProject UserService.Api


Esto crea la base `userservice_db` con la tabla `Users`. 

### Ejecución

- Ejecutar el proyecto `UserService.Api` desde Visual Studio.
- Swagger estará disponible en `https://localhost:{puerto}/swagger`. 

### Endpoints principales

- `POST /api/auth/register`
- `POST /api/auth/login`

La respuesta de login incluye un campo `token` con el JWT que luego usa ProductService y WebClient. 

---

## ProductService

### Configuración

1. Abrir la solución `ProductService` en Visual Studio.
2. Ajustar `ProductService.Api/appsettings.json`:

```
"ConnectionStrings": {
	"ProductDatabase": "Host=localhost;Port=5432;Database=productservice_db;Username=TU_USUARIO;Password=TU_PASSWORD"
},
"Jwt": {
	"Key": "LA_MISMA_LLAVE_SECRETA_QUE_EN_USERSERVICE",
	"Issuer": "UserService",
	"Audience": "UserServiceClients",
	"ExpiresInMinutes": 60
}
```

(La clave y el issuer deben coincidir con UserService para que el token sea válido). 

### Migraciones y base de datos

Consola del Administrador de paquetes (proyecto predeterminado `ProductService.Infrastructure`):

Add-Migration InitialProductSchema -StartupProject ProductService.Api
Update-Database -StartupProject ProductService.Api


### Ejecución

- Ejecutar `ProductService.Api`.
- Swagger en `https://localhost:{puerto}/swagger`. 

### Endpoints principales

- `GET /api/product` (autenticado, cualquier rol).
- `GET /api/product/{id}` (autenticado, cualquier rol).
- `POST /api/product` (solo `Admin`).
- `PUT /api/product/{id}` (solo `Admin`).
- `DELETE /api/product/{id}` (solo `Admin`). 

Swagger está configurado con esquema **Bearer**: usar `Authorize` y enviar `Bearer {token}`. 

---

## WebClient

### Configuración

1. Abrir la solución `WebClientSolution` en Visual Studio.
2. Ajustar `WebClient/appsettings.json` con las URLs reales de tus APIs:

```
"ApiUrls": {
	"UserService": "https://localhost:5001",
	"ProductService": "https://localhost:6001"
}
```

(Usar los puertos que Visual Studio asigne a `UserService.Api` y `ProductService.Api`). 

### Ejecución

- Ejecutar el proyecto `WebClient`.
- La app inicia en la página de **Login** (`/Auth/Login`). 

### Flujo de uso

1. **Registrar usuario Admin**  
   - Via Swagger de UserService:
     - `POST /api/auth/register` con body:
       ```
       {
         "name": "Admin User",
         "email": "admin@example.com",
         "password": "Admin123!",
         "role": "Admin"
       }
       ``` 

2. **Login desde WebClient**
   - Navegar a `https://localhost:{puerto-webclient}/`.
   - Iniciar sesión con el email/contraseña anteriores.
   - El WebClient envía el login a UserService, recibe el JWT y lo guarda en sesión. 

3. **Gestión de productos**
   - Al loguearse, se redirige al listado de productos.
   - Un usuario Admin puede:
     - Ver tabla de productos.
     - Crear nuevos productos (formulario Create).
     - Editar productos (formulario Edit).
     - Eliminar productos. 
   - Un usuario con rol `User` solo verá el listado, sin botones de creación/edición/eliminación (además el API aplica `[Authorize(Roles = "Admin")]`). 

---

## Postman / Swagger

- Cada microservicio expone Swagger (`/swagger`) para explorar y probar los endpoints. 
- Opcionalmente, puedes exportar las definiciones desde Swagger a una colección de Postman para incluirla en la entrega. 

---

## Notas de implementación

- Se usan patrones de repositorio, DTOs, servicios de aplicación y manejo de errores mediante excepciones controladas para mantener el código limpio y mantenible. 
- El acceso a datos se hace mediante Entity Framework Core con Npgsql, un `DbContext` independiente por microservicio y su propia base de datos en PostgreSQL, cumpliendo el patrón de autonomía de microservicios. 
- El WebClient se implementa como una SPA ligera con ASP.NET Core MVC (solo vistas), usando `HttpClient` y Session para manejar el JWT, cumpliendo los requisitos de login/logout, listado, creación y edición de productos con validaciones visuales. 

