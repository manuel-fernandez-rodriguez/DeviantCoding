using DeviantCoding.Registerly;
using DeviantCoding.Registerly.AttributeRegistration;
using DeviantCoding.Registerly.Strategies.Lifetime;

var builder = WebApplication.CreateBuilder(args);

// Two ways to register service classes.
// You can use one, another, or even both, like in this sample.

// - Marking the class with an attribute
//   (see GreeterService below)
builder.RegisterServicesByAttributes();

// - Selecting classes to register
builder.Register(classes => classes
    .FromAssemblyOf<FarewellerService>()
    .Where(c => c.Exactly<FarewellerService>()));

// We haven't specified any .Using() clause, so we will
// use the 3 default strategies, which is equivalent to
// .Using<Scoped, AsImplementedInterfaces, Add>()

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGet("/chatterbox/greet", (IGreeterService service) => service.Greet());
app.MapGet("/chatterbox/farewell", (IFarewellerService service) => service.Farewell());

app.Run();

public interface IGreeterService
{
    string Greet();
}

public interface IFarewellerService
{
    string Farewell();
}

[Singleton] // Only required if using RegisterServicesByAttributes
public class GreeterService : IGreeterService
{
    public string Greet() => "Hello, World!";
}

public class FarewellerService : IFarewellerService
{
    public string Farewell() => "Goodbye, World!";
}