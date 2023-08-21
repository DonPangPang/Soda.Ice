using Soda.Ice.Client.Services;

namespace Soda.Ice.Client.Extensions
{
    public static class GlobalSetupsExtensions
    {
        public static void AddIce(this IServiceCollection services)
        {
            services.AddServices();
        }

        private static void AddServices(this IServiceCollection services)
        {
            services.AddSingleton<IIceHttpClient, IceHttpClient>();
            services.AddScoped<AuthorizationApiService>();
            services.AddScoped<UserApiService>();
            services.AddScoped<BlogApiService>();
            services.AddScoped<BlogGroupApiService>();
            services.AddScoped<BlogTagApiService>();
            services.AddScoped<BlogViewLogApiService>();
            services.AddScoped<FileResourceApiService>();
            services.AddScoped<HistoryLogApiService>();
            services.AddScoped<LoginLogApiService>();
        }
    }
}