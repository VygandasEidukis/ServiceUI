using EPS.Administration.APIAccess.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EPS.Administration.ServiceUI
{
    public static class ServicesManager
    {
        private static ServiceProvider _serviceProvider { get; set; }
        public static ServiceProvider Services
        {
            get
            {
                if (_serviceProvider == null)
                {
                    _serviceProvider = Init();
                }
                return _serviceProvider;
            }
        }

        public static ISelfService SelfService
        {
            get
            {
                return Services.GetService<ISelfService>();
            }
        }

        private static ServiceProvider Init()
        {
            var services = new ServiceCollection();
            var confBuilder = new ConfigurationBuilder();
            confBuilder.AddJsonFile("appsettings.json", optional: true);
            var config = confBuilder.Build();
            services.AddSingleton<IConfiguration>(config);
            services.AddScoped<ISelfService, SelfService>();
            return services.BuildServiceProvider();
        }
    }
}
