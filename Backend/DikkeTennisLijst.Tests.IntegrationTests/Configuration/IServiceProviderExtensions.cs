namespace DikkeTennisLijst.Tests.IntegrationTests.Configuration
{
    public static class IServiceProviderExtensions
    {
        /// <summary>
        /// Gets the <typeparamref name="T"/> as a scoped service.
        /// The scope is disposed of after all the tests have run.
        /// </summary>
        public static T GetRequiredScopedService<T>(this IServiceProvider services)
        {
            var scope = services.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<T>();
            return service;
        }

        /// <summary>
        /// Gets the <typeparamref name="T"/> as a scoped service, and its scope.
        /// The scope is disposed of after all the tests have run, if it is not disposed of earlier.
        /// </summary>
        public static (T, IServiceScope) GetRequiredScopedServiceWithScope<T>(this IServiceProvider services)
        {
            var scope = services.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<T>();
            return (service, scope);
        }
    }
}