using Microsoft.Extensions.DependencyInjection;

namespace VerficationEmailSender.SendGrid
{
    public static class SendGrindExtention
    {
        public static IServiceCollection AddMySendGrindEmail(this IServiceCollection services)
        {
            services.AddScoped<ISendgrindEmailSender, SendgrindEmailSender>();
            return services;
        }
    }
}
