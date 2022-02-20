﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Events.Api.Authorization;
using Events.Api.Logging;
using Events.Api.Models.APTs;
using Events.Api.Models.General;
using Events.Api.Models.Incidents;
using Events.Api.Models.Tasks;
using Events.Api.Models.UserManagement;
using Events.Core.Models;
using Events.Core.Models.APTs;
using Events.Core.Models.DPE;
using Events.Core.Models.Employees;
using Events.Core.Models.General;
using Events.Core.Models.Incidents;
using Events.Core.Models.NewsBlog;
using Events.Core.Models.Notifications;
using Events.Core.Models.Reports;
using Events.Core.Models.Services;
using Events.Core.Models.StaticIP;
using Events.Core.Models.Tasks;
using Events.Core.Models.UserManagement;
using Events.Data;
using Events.Service.Files;
using Events.Service.Mappers;
using Events.Service.Service;
using Events.Service.Service.DataServices;
using EventsServerSide;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using UserManagment.services;
using UserManagment.Services;

namespace Events.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(Configuration["DefaultConnection"]),ServiceLifetime.Transient);
            //services.AddScoped((_) => new DbContextImpl(Configuration["DefaultConnection"]));


            // step 2 Add Identity Core service
            services.AddIdentity<EUser, ERole>(o =>
                {
                    o.Password.RequireDigit = false;
                    o.Password.RequireLowercase = false;
                    o.Password.RequireUppercase = false;
                    o.Password.RequireNonAlphanumeric = false;
                    o.Password.RequiredLength = 6;
                })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            services.AddTransient<IAuthorizationPolicyProvider, CalimAuthProvider>();
            services.AddScoped<IAuthorizationHandler, ClaimAuthHandler>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            var key = Encoding.ASCII.GetBytes(Configuration["Secret"]);
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddScoped(typeof(IQuery<Organization, Organization>), typeof(OrganizationQuery));
            services.AddScoped(typeof(IQuery<GeneralReport, GeneralReportDataView>), typeof(GeneralReportsQuery));
            services.AddScoped(typeof(IQuery<IncidentCategory, IncidentCategory>), typeof(IncidentCategoryQuery));
            services.AddScoped(typeof(IQuery<Incident, IncidentView>), typeof(IncidentQuery));
            services.AddScoped(typeof(IQuery<DepartmentRecords, DepartmentRecords>), typeof(DepartmentRecordsQuery));
            services.AddScoped(typeof(IQuery<NotificationOwner, NotificationOwner>), typeof(NotificationOwnersQuery));
            services.AddScoped(typeof(IQuery<GReportEntityAssignment, GReportEntityAssignment>), typeof(ReportAssignmentsQuery));
            services.AddScoped(typeof(IQuery<LateTopics, LateTopics>), typeof(LateTopicsQuery));
            services.AddScoped(typeof(IQuery<SectionRecords, SectionRecords>), typeof(SectionRecordsQuery));
            services.AddScoped(typeof(IQuery<Task, Taskview>), typeof(TaskQuery));
            services.AddScoped(typeof(IQuery<Vulnerability, VulnerabilityView>), typeof(VulnerabilitiesQuery));
            services.AddScoped(typeof(IQuery<Exploit, ExploitView>), typeof(ExploitQuery));
            services.AddScoped(typeof(IQuery<DPE, DPEview>), typeof(DPEQuery));
            services.AddScoped(typeof(IQuery<APT, AptView>), typeof(APTQuery));
            services.AddScoped(typeof(IQuery<StaticIP, StaticIP>), typeof(StaticIPsQuery));
            services.AddScoped(typeof(IQuery<Attachment, Attachment>), typeof(AttachmentQuery));
            services.AddScoped(typeof(IQuery<Inbox, Inbox>), typeof(InboxQuery));
            services.AddScoped(typeof(IQuery<Comment, Comment>), typeof(CommentQuery));
            services.AddScoped(typeof(IQuery<Notification, NotificationView>), typeof(NotificationQuery));
            services.AddScoped(typeof(IQuery<EntityAssignment, EntityAssignment>), typeof(EntityAssignmentQuery));
            services.AddScoped(typeof(IQuery<IpAddress, IpAddress>), typeof(IpAddressQuery));
            services.AddScoped(typeof(IUserService), typeof(UserService));
            services.AddScoped(typeof(IQuery<Status, Status>), typeof(StatusQuery));
            services.AddScoped(typeof(IQuery<TaskEntityAssignment, TaskEntityAssignment>), typeof(TaskAssignmentsQuery));
            services.AddScoped(typeof(IMapper<GeneralReport, GeneralReportDataView>), typeof(GeneralReportMapper));
            services.AddScoped(typeof(IFileHandler), typeof(FileHandler));
            
            
            services.AddScoped(typeof(IServiceFactory), typeof(ServiceFactory));
            services.Configure<SysConfiguration>(Configuration.GetSection(SysConfiguration.Name));

            services.AddScoped<LogAction>();
            services.AddScoped<IUserService, UserService>();
            services.AddAuthorization(auth =>
            {
                //auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                //.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme‌​)
                //.RequireAuthenticatedUser().Build());
            });
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                }) //Adding Jwt Bearer
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidAudience = Configuration["Jwt:ValidAudience"],
                        ValidIssuer = Configuration["JWT:ValidIssuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"]))
                    };
                });


            services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                );
            services.ConfigureCors();
            services.AddHttpContextAccessor();
            services.ConfigureIISService();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseExceptionHandler(appBuilder =>
            {
                appBuilder.Use(async (context, next) =>
                {
                    var error = context.Features[typeof(IExceptionHandlerFeature)] as IExceptionHandlerFeature;

                    if (error != null && error.Error is SecurityTokenExpiredException)
                    {
                        context.Response.StatusCode = 405;
                        context.Response.ContentType = "application/json";

                        await context.Response.WriteAsync(JsonConvert.SerializeObject(new
                        {
                            State = "Unauthorized",
                            Msg = "token expired"
                        }));
                    }
                    else if (error != null && error.Error! is SecurityTokenExpiredException)
                    {
                        context.Response.StatusCode = 410;
                        context.Response.ContentType = "application/json";

                        await context.Response.WriteAsync(JsonConvert.SerializeObject(new
                        {
                            State = "Error",
                            Msg = error.Error.Message
                        }));
                    }
                    else await next();
                });
            });

            app.UseStaticFiles();
            app.UseCors("CorsPolicy");
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.All
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseMiddleware<JwtMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}