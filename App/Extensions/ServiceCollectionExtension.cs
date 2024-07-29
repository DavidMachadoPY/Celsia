using System.Reflection;

namespace ServeBooks.App.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void AddServices(this IServiceCollection services, Assembly assembly)
        {
            // Gets all types in the assembly ending with "Service"
            var types = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.Name.EndsWith("Repository"))
                .ToList();

            foreach (var type in types)
            {
                // Searches for an interface that matches the name of the class prefixed with 'I'
                var interfaceType = type.GetInterface($"I{type.Name}");
                if (interfaceType != null)
                {
                    // Records the implementation in the DI container
                    services.AddScoped(interfaceType, type);
                }
            }
        }
    }
}