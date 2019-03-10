using Microsoft.Extensions.DependencyInjection;

namespace VerificationEmailSender.SendGrid
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
