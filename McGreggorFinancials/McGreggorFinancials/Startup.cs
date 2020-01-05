using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using McGreggorFinancials.Models.Accounts.Repositories;
using McGreggorFinancials.Models.Data;
using McGreggorFinancials.Models.Donations.Repository;
using McGreggorFinancials.Models.Expenses.Repositories;
using McGreggorFinancials.Models.Income.Repositories;
using McGreggorFinancials.Models.Targets.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace McGreggorFinancials
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
                    Configuration["Data:McgreggorFinancials:ConnectionString"]
                ));
            services.AddTransient<IIncomeCategoryRepository, IncomeCategoryRepository>();
            services.AddTransient<IIncomeEntryRespository, IncomeEntryRepository>();
            services.AddTransient<IAccountRepository, AccountRepository>();
            services.AddTransient<IAccountTypeRepository, AccountTypeRepository>();
            services.AddTransient<ITargetAmountRepository, TargetAmountRepository>();
            services.AddTransient<ITargetTypeRepository, TargetTypeRepository>();
            services.AddTransient<IExpenseRepository, ExpenseRepository>();
            services.AddTransient<IExpenseCategoryRepository, ExpenseCategoryRepository>();
            services.AddTransient<ICreditBalanceRepository, CreditBalanceRepository>();
            services.AddTransient<IPaymentMethodRepository, PaymentMethodRepository>();
            services.AddTransient<IAccountRepository, AccountRepository>();
            services.AddTransient<IAccountTypeRepository, AccountTypeRepository>();
            services.AddTransient<ICharityRepository, CharityRepository>();
            services.AddTransient<IDonationRepository, DonationRepository>();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStatusCodePages();
            app.UseStaticFiles();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Dashboard}");
            });

            SeedData.EnsurePopulated(app);
        }
    }
}
