using System.Reflection;

// Scutor is a library which adds classes to DI based on the interfaces they implement. I like this approach because you have the information about the dependencies in the classes themselves instead of having to go to the Program.cs file to see what is being added to the DI container.
public static class AutomaticDependencyInjectionExtension
    {
        public static void AddDependencies(this IServiceCollection services)
        {
            var assemblies = (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                from referencedAssembly in assembly.GetReferencedAssemblies()
                where referencedAssembly.Name != null && referencedAssembly.Name.StartsWith("google")
                select Assembly.Load(referencedAssembly)).ToList();

            assemblies.Add(Assembly.GetCallingAssembly());

            services.Scan(scan => scan
                .FromAssemblies(assemblies)
                .AddClasses(classes => classes.AssignableTo<ITransient>())
                .AsSelfWithInterfaces()
                .WithTransientLifetime()
                .AddClasses(classes => classes.AssignableTo<IScoped>())
                .AsSelfWithInterfaces()
                .WithScopedLifetime()
                .AddClasses(classes => classes.AssignableTo<ISingleton>())
                .AsSelfWithInterfaces()
                .WithSingletonLifetime());
        }
    }
