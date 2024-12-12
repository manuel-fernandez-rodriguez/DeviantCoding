using DeviantCoding.Registerly;
using DeviantCoding.Registerly.AttributeRegistration;
using DeviantCoding.Registerly.Strategies.Lifetime;
using Microsoft.AspNetCore.Mvc;

// Start the application and use the provided http file to test it.

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Two ways to register service classes:
// - Marking the class with an attribute
// - Manually registering a class

builder.RegisterServicesByAttributes();

builder.Register(classes => classes
    .FromAssemblyOf<FarewellerService>()
    .Where(c => c.Exactly<FarewellerService>())
    .Using<Singleton>());

var app = builder.Build();

app.MapControllers();

app.Run();
    


[ApiController]
[Route("chatterbox")]
public class ChatterboxController(IGreeterService greeterService, IFarewellerService farewellerService) : ControllerBase
{
    [HttpGet("greet")]
    public string Greet() => greeterService.Greet();

    [HttpGet("farewell")]
    public string Farewell() => farewellerService.Farewell();
}


public interface IGreeterService
{
    string Greet();
}

[Singleton]
public class GreeterService : IGreeterService
{
    public string Greet() => "Hello, World!";
}

public interface IFarewellerService
{
    string Farewell();
}

public class FarewellerService : IFarewellerService
{
    public string Farewell() => "Goodbye, World!";
}
