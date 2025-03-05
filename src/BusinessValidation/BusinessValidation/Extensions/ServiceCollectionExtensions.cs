using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BusinessValidation.Extensions
{
    /// <summary>
    /// Static class containing extension methods for the <see cref="IServiceCollection"/> abstraction which registers all BusinessValidation abstractions in the Microsoft IOC class.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Extension method to register all BusinessValidation abstractions in a collection of assemblies with the Microsoft IOC class. The default service lifetime is scoped.
        /// </summary>
        /// <param name="services">Type: <see cref="IServiceCollection"/>. The abstraction for Microsoft's IOC implementation.</param>
        /// <param name="assemblies">Type: <see cref="IEnumerable&lt;Assembly&gt"/>. The enumerable collection of assemblies to scan.</param>
        /// <param name="serviceLifetime">Type: <see cref="ServiceLifetime"/>. The lifetime for the registration with the IOC.</param>
        /// <returns>A <see cref="IServiceCollection "/>, being the object which has been extended.</returns>
        public static IServiceCollection AddBusinessValidation(this IServiceCollection services, IEnumerable<Assembly> assemblies, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            var types = assemblies.SelectMany(t => t.GetExportedTypes()
                .Where(t => t.IsClass && !t.IsAbstract && !t.IsGenericTypeDefinition &&
                t.GetInterfaces().Where(i => i.IsGenericType).Select(i => i.GetGenericTypeDefinition()).Any(t => t.Equals(typeof(IBusinessValidator<>)))
                ).Select(t => new
                {
                    Abstraction = t.GetInterfaces().Where(i => i.GetGenericTypeDefinition().Equals(typeof(IBusinessValidator<>))).First(),
                    Implementation = t
                }));

            foreach (var type in types)
            {
                services.Add(ServiceDescriptor.Describe(type.Abstraction, type.Implementation, serviceLifetime));
            }

            return services;
        }

        /// <summary>
        /// Extension method to register all BusinessValidation abstractions in an assembly with the Microsoft IOC class. The default service lifetime is scoped.
        /// </summary>
        /// <param name="services">Type: <see cref="IServiceCollection"/>. The abstraction for Microsoft's IOC implementation.</param>
        /// <param name="assemblies">Type: <see cref="Assembly"/>. The assembly to scan.</param>
        /// <param name="serviceLifetime">Type: <see cref="ServiceLifetime"/>. The lifetime for the registration with the IOC.</param>
        /// /// <returns>A <see cref="IServiceCollection "/>, being the object which has been extended.</returns>
        public static IServiceCollection AddBusinessValidation(this IServiceCollection services, Assembly assembly, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            var types = assembly.GetExportedTypes()
                .Where(t => t.IsClass && !t.IsAbstract && !t.IsGenericTypeDefinition &&
                t.GetInterfaces().Where(i => i.IsGenericType).Select(i => i.GetGenericTypeDefinition()).Any(t => t.Equals(typeof(IBusinessValidator<>)))).Select(t => new
                {
                    Abstraction = t.GetInterfaces().Where(i => i.GetGenericTypeDefinition().Equals(typeof(IBusinessValidator<>))).First(),
                    Implementation = t
                });

            foreach (var type in types)
            {
                services.Add(ServiceDescriptor.Describe(type.Abstraction, type.Implementation, serviceLifetime));
            }

            return services;
        }
    }
}
