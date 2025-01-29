using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BusinessValidation
{
    public static class ServiceCollectionExtensions
    {
        public static void AddBusinessValidation(this IServiceCollection services, Type[] types)
        {
            // TODO
        }
        
        public static void AddBusinessValidation(this IServiceCollection services, Type[] types, ServiceLifetime serviceLifetime)
        {
            // TODO
        }
        
        public static void AddBusinessValidation(this IServiceCollection services, IEnumerable<Assembly> assemblies)
        {
            services.AddBusinessValidation(assemblies, ServiceLifetime.Singleton);
        }
        
        public static void AddBusinessValidation(this IServiceCollection services, IEnumerable<Assembly> assemblies, ServiceLifetime serviceLifetime)
        {
            var types = assemblies.SelectMany(t => t.GetExportedTypes()
                .Where(t => t.IsClass &&
                    !t.IsAbstract &&
                    typeof(BusinessValidation.BusinessValidator).IsAssignableFrom(t)
                ));

            foreach (var type in types)
            {
                services.Add(ServiceDescriptor.Describe(type, type, serviceLifetime));                

                var validatorType = typeof(IBusinessValidator<>).MakeGenericType(type);
                var serviceType = typeof(BusinessValidator<>).MakeGenericType(type);
                var validatorType2 = typeof(IBusinessValidator<>).MakeGenericType(type);
                var serviceType2 = typeof(BusinessValidator<>).MakeGenericType(type);

                services.Add(ServiceDescriptor.Describe(validatorType, serviceType, serviceLifetime));
                services.Add(ServiceDescriptor.Describe(validatorType2, serviceType2, serviceLifetime));
            }
        }
        
        //public static void AddBusinessValidation(this IServiceCollection services, Assembly assembly, ServiceLifetime serviceLifetime)
        //{
        //    var types = assembly.GetExportedTypes()
        //        .Where(t => t.IsClass &&
        //        !t.IsAbstract &&
        //        typeof(BusinessValidation.BusinessValidator).IsAssignableFrom(t)
        //        );

        //    foreach (var type in types)
        //    {
        //        services.AddSingleton(type);
        //        services.AddSingleton(type);

        //        var validatorType = typeof(IBusinessValidator<>).MakeGenericType(type);
        //        var serviceType = typeof(BusinessValidator<>).MakeGenericType(type);
        //        var validatorType2 = typeof(IBusinessValidator<>).MakeGenericType(type);
        //        var serviceType2 = typeof(BusinessValidator<>).MakeGenericType(type);

        //        services.AddSingleton(validatorType, serviceType);
        //        services.AddSingleton(validatorType2, serviceType2);
        //        services.Add(ServiceDescriptor.Describe(validatorType2, serviceType2, ServiceLifetime));
        //    }
        //}
    }
}
