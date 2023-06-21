using BienenstockCorpAPI.Data;
using BienenstockCorpAPI.Helpers.Consts;
using BienenstockCorpAPI.Hubs;
using BienenstockCorpAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Dependency Injections
builder.Services.AddSqlServer<BienenstockCorpContext>(builder.Configuration.GetConnectionString("BienenstockCorpConnection"));
builder.Services.AddScoped<AuthenticationService, AuthenticationService>();
builder.Services.AddScoped<UserService, UserService>();
builder.Services.AddScoped<ProductService, ProductService>();
builder.Services.AddScoped<MessageService, MessageService>();
builder.Services.AddScoped<LogService, LogService>();
builder.Services.AddScoped<PurchaseService, PurchaseService>();
builder.Services.AddScoped<SaleService, SaleService>();
builder.Services.AddScoped<ReportService, ReportService>();
#endregion


// Cors
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      builder =>
                      {
                          builder.WithOrigins("http://localhost:3000", "http://bienenstockcorp.somee.com")
                          .SetIsOriginAllowedToAllowWildcardSubdomains()
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                      });
});

// Autentication using JWT Token
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddCookie(x =>
    {
        x.Cookie.Name = CookieName.NAME;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                context.Token = context.Request.Cookies[CookieName.NAME];
                return Task.CompletedTask;
            }
        };
    });

// Swagger Authorize
builder.Services.AddSwaggerGen(setupAction =>
{
    setupAction.AddSecurityDefinition("BienenstockCorpApiBearerAuth", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Description = "JWT Authentication",
        Scheme = "Bearer",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
    });

    setupAction.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "BienenstockCorpApiBearerAuth",
                }
            }, 
            new List<string>() 
        }
    });
});

// SignalR
builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.None,
    Secure = CookieSecurePolicy.Always
});

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthentication();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<ChatHub>("/chat");
    endpoints.MapHub<PageHub>("/page");
});

app.MapControllers();

app.Run();
