using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IGHportalAPI.DataContext;
using IGHportalAPI.Middlewares;
using IGHportalAPI.Models;
using IGHportalAPI.Services;
using IGHportalAPI.Services.Common;
using IGHportalAPI.Services.Mapping;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Microsoft.Net.Http.Headers;

namespace IGHportalAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.

        readonly string MyPolicy = "_myPolicy";
        public void ConfigureServices(IServiceCollection services)
        {

            services.Configure<EmailSettings>(Configuration.GetSection("Email"));

            services.AddTransient<IEmailSender, EmailService>();


            services.AddDbContext<DataContext_>(
                options => options.UseSqlServer(Configuration.GetConnectionString("ConnectionStr")));


            services.AddTransient<IAdministrationService, AdministrationService>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IHotelService, HotelService>();

            services.AddTransient<IEmployeeService, EmployeeService>();
            services.AddTransient<IDepartmentService, DepartmentService>();

            services.AddTransient<IPayrollReportService, PayrollReportService>();

            services.AddTransient<ILinenInventoryService, LinenInventoryService>();

            services.AddTransient<ILinenItemService, LinenItemService>();


            services.AddTransient<ISundriesShopInventoryService, SundriesShopInventoryService>();

            services.AddTransient<ISundriesShopProductService, SundriesShopProductService>();

            //services.AddTransient<IUserService, UserService>();

            services.AddTransient<IWeeklyWrapUpService, WeeklyWrapUpService>();


            services.AddTransient<IScores_IssuesService, Scores_IssuesService>();

            services.AddTransient<IDashboardService, DashboardService>();



            services.AddIdentity<User, IdentityRole>(
            options =>
            {
                options.Password.RequiredLength = 5;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireLowercase = false;
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;


            }
            ).AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<DataContext_>()
            .AddDefaultTokenProviders();




            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "IGHportalAPI", Version = "v1" });
            });


            //services.AddCors(options =>
            //{
            //    options.AddDefaultPolicy(builder =>
            //    {
            //        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
            //    });
            //});

            services.AddCors(options =>
            {
                options.AddPolicy(name: MyPolicy,
                    builder =>
                    {
                        builder
                        //.AllowAnyOrigin()
                        //.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                        .WithOrigins("https://ihg.ps-beta.com",
                                            "http://localhost:333")
                               .WithHeaders(HeaderNames.ContentType, "x-custom-header")
                               .WithMethods("PUT", "DELETE", "GET", "POST", "OPTIONS");
                    });
            });


            services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {


            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //    app.UseSwagger();
            //    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "IGHportalAPI v1"));
            //}
            app.ConfigureExcetionHandler(env);
            
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(env.ContentRootPath, "StaticFiles")),
                RequestPath = "/StaticFiles"
                //FileProvider = new PhysicalFileProvider(
                //    Path.Combine(env.ContentRootPath, "Files")),
            });

            app.UseHttpsRedirection();

            app.UseRouting();


            app.UseCors(MyPolicy);
            //app.UseCors();


            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers().RequireCors(MyPolicy);
            });
        }
    }
}
