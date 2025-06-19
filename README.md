Documentación .NET
Esta documentación, no es estrictamente relacional/aplicada a este proyecto, si no una introduccion a los conceptos basicos de .Net, EntityFrameworkCore y el patron repository, que si es implementado por este proyecto. 
La clave de esta documentación, es comprender el flujo practico/teorico del patrón implementado. 

---

## **1. Estructura del Proyecto**
Aquí está la estructura del proyecto (ejemplo/conceptual) que utilizaremos:

```
- MyApp/
  - MyApp.Domain/          # Entidades y lógica de negocio
  - MyApp.Application/     # Servicios, DTOs y mappers
  - MyApp.Infrastructure/  # Repositorios, DbContext, y configuraciones
  - MyApp.API/             # Controladores, middleware, y configuración de la API
  - MyApp.Shared/          # Utilidades compartidas (configuraciones, dependencias, etc.)
  - MyApp.Tests/           # Pruebas unitarias y de integración
```

---

## **2. Configuraciones Avanzadas**

### **2.1. Archivo `appsettings.json`**
El archivo `appsettings.json` es el lugar central para almacenar configuraciones como cadenas de conexión, claves JWT, y otros ajustes de la aplicación.

#### **Ejemplo de `appsettings.json`**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=MyAppDb;User Id=sa;Password=YourPassword;TrustServerCertificate=True;"
  },
  "Jwt": {
    "Key": "YourSuperSecretKeyForJWTTokenEncryption",
    "Issuer": "MyApp",
    "Audience": "MyAppUsers",
    "ExpiryInMinutes": 30
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

#### **Uso de Configuraciones en el Proyecto**
Para acceder a estas configuraciones, utilizamos el objeto `IConfiguration` inyectado en las clases que lo necesiten.

```csharp
// Ejemplo de uso en una clase
public class AuthService
{
    private readonly IConfiguration _configuration;

    public AuthService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(User user)
    {
        var key = _configuration["Jwt:Key"];
        var issuer = _configuration["Jwt:Issuer"];
        var audience = _configuration["Jwt:Audience"];
        var expiry = int.Parse(_configuration["Jwt:ExpiryInMinutes"]);

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: new[] { new Claim(ClaimTypes.Name, user.Username) },
            expires: DateTime.Now.AddMinutes(expiry),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
```

---

### **2.2. Configuración de Entity Framework Core**
Entity Framework Core se configura en el `Program.cs` o en un archivo de configuración separado.

#### **Configuración en `Program.cs`**
```csharp
var builder = WebApplication.CreateBuilder(args);

// Configuración de DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();
```

#### **Configuración de Migraciones**
Para crear y aplicar migraciones, usa los siguientes comandos en la terminal:
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

---

### **2.3. Configuración de JWT**
La autenticación JWT se configura en el `Program.cs` y se utiliza en los controladores para proteger los endpoints.

#### **Configuración en `Program.cs`**
```csharp
var builder = WebApplication.CreateBuilder(args);

// Configuración de JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Middleware de autenticación y autorización
app.UseAuthentication();
app.UseAuthorization();
```

---

### **2.4. Configuración de AutoMapper**
AutoMapper se utiliza para mapear entre entidades y DTOs. Se configura en un perfil y se registra en el contenedor de dependencias.

#### **Perfil de AutoMapper**
```csharp
// MyApp.Shared/Mappings/AutoMapperProfile.cs
public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Product, ProductDTO>().ReverseMap();
        CreateMap<User, UserDTO>().ReverseMap();
    }
}
```

#### **Registro en `Program.cs`**
```csharp
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
```

---

### **2.5. Configuración de Dependencias**
Para mantener el `Program.cs` limpio, movemos la configuración de dependencias a un archivo separado.

#### **Archivo de Configuración de Dependencias**
```csharp
// MyApp.Shared/DependencyInjection.cs
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Configuración de DbContext
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        // Repositorios
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        return services;
    }

    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // AutoMapper
        services.AddAutoMapper(typeof(AutoMapperProfile));

        // Servicios
        services.AddScoped<AuthService>();
        services.AddScoped<ProductService>();

        return services;
    }
}
```

#### **Uso en `Program.cs`**
```csharp
var builder = WebApplication.CreateBuilder(args);

// Configuración de dependencias
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

var app = builder.Build();
```

---

## **3. Ejemplo de Flujo Completo**

### **3.1. Crear un Producto**
1. **Controlador (API)**:
   ```csharp
   [ApiController]
   [Route("api/[controller]")]
   public class ProductsController : ControllerBase
   {
       private readonly ProductService _productService;
       private readonly IMapper _mapper;

       public ProductsController(ProductService productService, IMapper mapper)
       {
           _productService = productService;
           _mapper = mapper;
       }

       [HttpPost]
       public async Task<IActionResult> CreateProduct(ProductDTO productDTO)
       {
           var product = _mapper.Map<Product>(productDTO);
           await _productService.AddAsync(product);
           return Ok();
       }
   }
   ```

2. **Servicio (Application)**:
   ```csharp
   public class ProductService
   {
       private readonly IRepository<Product> _productRepository;

       public ProductService(IRepository<Product> productRepository)
       {
           _productRepository = productRepository;
       }

       public async Task AddAsync(Product product)
       {
           await _productRepository.AddAsync(product);
       }
   }
   ```

3. **Repositorio (Infrastructure)**:
   ```csharp
   public class Repository<T> : IRepository<T> where T : class
   {
       private readonly ApplicationDbContext _context;
       private readonly DbSet<T> _dbSet;

       public Repository(ApplicationDbContext context)
       {
           _context = context;
           _dbSet = _context.Set<T>();
       }

       public async Task AddAsync(T entity)
       {
           await _dbSet.AddAsync(entity);
           await _context.SaveChangesAsync();
       }
   }
   ```

---

## **4. Configuraciones Adicionales**

### **4.1. Logging**
El logging se configura en `appsettings.json` y se puede personalizar para diferentes niveles de verbosidad.

#### **Ejemplo de Logging en un Servicio**
```csharp
public class ProductService
{
    private readonly ILogger<ProductService> _logger;

    public ProductService(ILogger<ProductService> logger)
    {
        _logger = logger;
    }

    public async Task AddAsync(Product product)
    {
        _logger.LogInformation("Adding a new product: {ProductName}", product.Name);
        await _productRepository.AddAsync(product);
    }
}
```

---

### **4.2. Middleware Personalizado**
Puedes crear middleware personalizado para manejar excepciones globales, validaciones, etc.

#### **Ejemplo de Middleware**
```csharp
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsync("An error occurred: " + ex.Message);
        }
    }
}
```

#### **Registro en `Program.cs`**
```csharp
app.UseMiddleware<ExceptionHandlingMiddleware>();
```

---
