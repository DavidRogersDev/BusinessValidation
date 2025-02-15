using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BusinessValidation
{
    public static class ServiceCollectionExtensions
    {        
        public static void AddBusinessValidation(this IServiceCollection services, IEnumerable<Assembly> assemblies, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            var types = assemblies.SelectMany(t => t.GetExportedTypes()
                .Where(t => t.IsClass &&
                t.GetInterfaces().Select(i => i.GetGenericTypeDefinition()).Any(t => t.Equals(typeof(IBusinessValidator<>)))
                ).Select(t => new
                {
                    Abstraction = t.GetInterfaces().Where(i => i.GetGenericTypeDefinition().Equals(typeof(IBusinessValidator<>))).First(),
                    Implementation = t
                }));

            foreach (var type in types)
            {
                services.Add(ServiceDescriptor.Describe(type.Abstraction, type.Implementation, serviceLifetime));
            }
        }

        public static void AddBusinessValidation(this IServiceCollection services, Assembly assembly, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            var types = assembly.GetExportedTypes()
                .Where(t => t.IsClass &&                 
                t.GetInterfaces().Select(i => i.GetGenericTypeDefinition()).Any(t => t.Equals(typeof(IBusinessValidator<>)))                
                ).Select(t => new
                {
                    Abstraction = t.GetInterfaces().Where(i => i.GetGenericTypeDefinition().Equals(typeof(IBusinessValidator<>))).First(),
                    Implementation = t
                });

            foreach (var type in types)
            {                
                services.Add(ServiceDescriptor.Describe(type.Abstraction, type.Implementation, serviceLifetime));
            }
        }
    }
}
