using BienenstockCorpAPI.Data;
using BienenstockCorpAPI.Services;

var builder = WebApplication.CreateBuilder(args);

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Dependency Injections
builder.Services.AddSqlServer<BienenstockCorpContext>(builder.Configuration.GetConnectionString("BienenstockCorpConnection"));
builder.Services.AddScoped<UserService, UserService>();
builder.Services.AddScoped<ProductService, ProductService>();
builder.Services.AddScoped<NoteService, NoteService>();
#endregion

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      builder =>
                      {
                          builder.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost")
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                      });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();
