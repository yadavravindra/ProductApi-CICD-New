using ProductApi.Repositories;
using ProductApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
var builder = WebApplication.CreateBuilder(args);

// =====================================================
// 1. Add Controllers
// =====================================================
builder.Services.AddControllers();


// =====================================================
// 2. Dependency Injection
// =====================================================
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();


// =====================================================
// 3. Swaggers
// =====================================================
// =====================================================
// 3. Swagger with JWT Bearer Authentication
// =====================================================
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.OpenApiInfo
    {
        Title = "ProductApi",
        Version = "v1"
    });

    options.AddSecurityDefinition("Bearer",
        new Microsoft.OpenApi.OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = Microsoft.OpenApi.SecuritySchemeType.Http,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = Microsoft.OpenApi.ParameterLocation.Header,
            Description = "Enter your JWT token."
        });

    options.AddSecurityRequirement(document =>
        new Microsoft.OpenApi.OpenApiSecurityRequirement
        {
            {
                new Microsoft.OpenApi.OpenApiSecuritySchemeReference(
                    "Bearer", document),
                new List<string>()
            }
        });
});

// =====================================================
// 4. CORS
// =====================================================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});


// =====================================================
// 5. JWT Authentication
// =====================================================
var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];

if (string.IsNullOrEmpty(jwtKey))
{
    throw new InvalidOperationException(
        "JWT Key is missing in appsettings.json."
    );
}

if (string.IsNullOrEmpty(jwtIssuer))
{
    throw new InvalidOperationException(
        "JWT Issuer is missing in appsettings.json."
    );
}

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // Validate JWT Issuer
            ValidateIssuer = true,
            ValidIssuer = jwtIssuer,

            // Validate JWT Audience
            ValidateAudience = true,
            ValidAudience = jwtIssuer,

            // Validate Token Expiration
            ValidateLifetime = true,

            // Validate Signing Key
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey)
            ),

            // Remove unnecessary clock difference
            ClockSkew = TimeSpan.Zero
        };
    });


// =====================================================
// 6. Authorization
// =====================================================
builder.Services.AddAuthorization();


// =====================================================
// 7. Build Application
// =====================================================
var app = builder.Build();


// =====================================================
// 8. Swagger
// =====================================================
 
    app.UseSwagger();
    app.UseSwaggerUI();



// =====================================================
// 9. Middleware Pipeline
// =====================================================
app.UseHttpsRedirection();

app.UseCors("AllowAll");

// IMPORTANT:
// Authentication must come before Authorization
app.UseAuthentication();

app.UseAuthorization();


// =====================================================
// 10. Map Controllers
// =====================================================
app.MapControllers();


// =====================================================
// 11. Run Application
// =====================================================
app.Run();