using DeviantCoding.Registerly;
using DeviantCoding.Registerly.Strategies.Lifetime;
using Registerly.Samples.WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Three ways to register the WeatherService class

//builder.RegisterServicesByAttributes();

builder.Register(classes => classes
    .Where(c => c.Exactly<WeatherService>())
    .Using<Singleton>());

// The following registration is more performant:
//builder.Register(classes => classes
//    .FromAssemblyOf<WeatherService>()
//    .Where(c => c.Exactly<WeatherService>())
//    .Using<Singleton>());

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
