using BienenstockCorpAPI.Data;
using BienenstockCorpAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSqlServer<BienenstockCorpContext>(builder.Configuration.GetConnectionString("BienenstockCorpConnection"));
builder.Services.AddScoped<UserService, UserService>();
builder.Services.AddScoped<ProductService, ProductService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
