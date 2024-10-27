using DeviantCoding.Registerly.SelfRegistration;
using DeviantCoding.Registerly.Strategies.Mapping;

namespace DeviantCoding.Registerly.UnitTests.SampleServices;

[Singleton<AsSelf>]
public class TestServiceDependency { }

public interface IScopedService { }

[Scoped]
public class TestScopedService(TestServiceDependency serviceDependency) : IScopedService
{
    private readonly TestServiceDependency _serviceDependency = serviceDependency;
}

public interface ITransientService { }

[Transient]
public class TestTransientService(TestServiceDependency serviceDependency) : ITransientService
{
    private readonly TestServiceDependency _serviceDependency = serviceDependency;
}

public interface ISingletonService { }

[Singleton]
public class TestSingletonService(TestServiceDependency serviceDependency) : ISingletonService
{
    private readonly TestServiceDependency _serviceDependency = serviceDependency;
}