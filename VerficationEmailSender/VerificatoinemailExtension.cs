using Microsoft.Extensions.DependencyInjection;
using VerficationEmailSender.SendGrid;

namespace VerficationEmailSender
{
    public static class VerificatoinemailExtension
    {
        public static IServiceCollection AddVerficationEmailSender(this IServiceCollection services)
        {
            services.AddMySendGrindEmail();
            services.AddScoped<IVerificationEmail, VerificationEmail>();
            return services;
        }
    }
}
