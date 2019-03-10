using Microsoft.Extensions.DependencyInjection;
using VerificationEmailSender.SendGrid;

namespace VerificationEmailSender
{
    public static class VerificatoinemailExtension
    {
        public static IServiceCollection AddVerificationEmailSender(this IServiceCollection services)
        {
            services.AddMySendGrindEmail();
            services.AddScoped<IVerificationEmail, VerificationEmail>();
            return services;
        }
    }
}
