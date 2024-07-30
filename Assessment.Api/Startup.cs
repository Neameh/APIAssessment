using Assessment.Core;
using Assessment.Core.Interfaces;
using Assessment.Core.Logic.Beneficiaries.Command;
using Assessment.Core.Logic.Beneficiaries.Queries;
using Assessment.Core.Logic.Topups;
using Assessment.Infrastructure.Data;
using Assessment.Infrastructure.Repositories;
using Assessment.Infrastructure.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Assessment.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {


            services.AddDbContext<ApplicationDbContext>(options =>
         options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IBeneficiaryRepository, BeneficiaryRepository>();
            services.AddScoped<ITopUpTransactionRepository, TopUpTransactionRepository>();
            services.AddScoped<IExternalBalanceService, ExternalBalanceService>();

            services.AddHttpClient<IExternalBalanceService, ExternalBalanceService>(client =>
            {
                client.BaseAddress = new Uri(Configuration["ExternalService:BaseUrl"]);
            });

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(AddBeneficiaryCommandHandler).Assembly));

            services.AddControllers();
            services.AddSwaggerGen();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Assessment.API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
