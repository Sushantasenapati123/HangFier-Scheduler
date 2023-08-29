

using Hangfier.Irepo.Email;
using Hangfier.Irepo.Factory;
using Hangfier.Repo.Email;
using Hangfier.Repo.Factory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace HangFierSS.Web.DiContaner
{
    public static class DiContainer
    {
        public static void AddCustomContainer(this IServiceCollection services, IConfiguration configuration)
        {
            IConnectionFactory connectionFactory = new ConnectionFactory(configuration.GetConnectionString("DefaultConnection"));
            services.AddSingleton(connectionFactory);
           
            services.AddScoped<IMailService, MailService>();



        }
    }
}
