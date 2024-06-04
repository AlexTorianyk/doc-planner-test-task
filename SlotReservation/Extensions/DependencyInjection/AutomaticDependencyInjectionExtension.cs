using System.Reflection;

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
