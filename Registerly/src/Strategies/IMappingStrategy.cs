using Microsoft.Extensions.DependencyInjection;

namespace DeviantCoding.Registerly.Strategies;

/// <summary>
/// Defines a strategy for mapping implementation types to service descriptors.
/// </summary>
public interface IMappingStrategy
{
    /// <summary>
    /// Maps the specified implementation types to a collection of <see cref="ServiceDescriptor"/> instances using the provided lifetime strategy.
    /// </summary>
    /// <param name="implementationTypes">The implementation types to map.</param>
    /// <param name="lifetimeStrategy">The lifetime strategy to use.</param>
    /// <returns>A collection of <see cref="ServiceDescriptor"/> instances.</returns>
    /// <example>
    /// <code>
    /// public class MyMappingStrategy : IMappingStrategy
    /// {
    ///     public IEnumerable&lt;ServiceDescriptor&gt; Map(IEnumerable&lt;Type&gt; implementationTypes, ILifetimeStrategy lifetimeStrategy)
    ///     {
    ///         foreach (var type in implementationTypes)
    ///         {
    ///             yield return new ServiceDescriptor(type, type, lifetimeStrategy.Map(type));
    ///         }
    ///     }
    /// }
    /// </code>
    /// </example>
    IEnumerable<ServiceDescriptor> Map(IEnumerable<Type> implementationTypes, ILifetimeStrategy lifetimeStrategy);
}
