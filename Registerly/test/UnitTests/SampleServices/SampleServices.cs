using DeviantCoding.Registerly.SelfRegistration;
using DeviantCoding.Registerly.Strategies.Mapping;

namespace DeviantCoding.Registerly.UnitTests.SampleServices;

[Singleton<AsSelf>]
public class TestServiceDependency { }

public interface IScopedService { }

[Scoped]
public class TestScopedService(TestServiceDependency serviceDependency) : IScopedService
{
#pragma warning disable IDE0052 // Remove unread private members
    private readonly TestServiceDependency _serviceDependency = serviceDependency;
#pragma warning restore IDE0052 // Remove unread private members
}

public interface ITransientService { }

[Transient]
public class TestTransientService(TestServiceDependency serviceDependency) : ITransientService
{
#pragma warning disable IDE0052 // Remove unread private members
    private readonly TestServiceDependency _serviceDependency = serviceDependency;
#pragma warning restore IDE0052 // Remove unread private members
}

public interface ISingletonService { }

[Singleton]
public class TestSingletonService(TestServiceDependency serviceDependency) : ISingletonService
{
#pragma warning disable IDE0052 // Remove unread private members
    private readonly TestServiceDependency _serviceDependency = serviceDependency;
#pragma warning restore IDE0052 // Remove unread private members
}