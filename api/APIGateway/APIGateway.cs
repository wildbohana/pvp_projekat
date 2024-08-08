using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.ServiceFabric.Services.Communication.AspNetCore;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using System.Fabric;
using System.Text;

// port je 8102

namespace APIGateway
{
    internal sealed class APIGateway : StatelessService
    {
        private IConfiguration _configuration;
        public APIGateway(StatelessServiceContext context) : base(context) 
        {
            _configuration = new ConfigurationManager();
        }

        #region Create listeners
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return new ServiceInstanceListener[]
            {
                new ServiceInstanceListener(serviceContext =>
                    new KestrelCommunicationListener(serviceContext, "ServiceEndpoint", (url, listener) =>
                    {
                        ServiceEventSource.Current.ServiceMessage(serviceContext, $"Starting Kestrel on {url}");

                        var builder = WebApplication.CreateBuilder();

                        builder.Services.AddSingleton<StatelessServiceContext>(serviceContext);
                        builder.WebHost
                                    .UseKestrel()
                                    .UseContentRoot(Directory.GetCurrentDirectory())
                                    .UseServiceFabricIntegration(listener, ServiceFabricIntegrationOptions.None)
                                    .UseUrls(url);
                        builder.Services.AddControllers();
                        builder.Services.AddEndpointsApiExplorer();
                        builder.Services.AddSwaggerGen();

                        // Load configuration from appsettings.json
                        //var configuration = new ConfigurationBuilder()
                        //    .SetBasePath(builder.Environment.ContentRootPath)
                        //    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                        //    .Build();
                        // Add JWT authentication
                        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                        .AddJwtBearer(options =>
                        {
                            options.TokenValidationParameters = new TokenValidationParameters
                            {
                                ValidateIssuer = true,
                                //ValidateAudience = true,
                                ValidateLifetime = true,
                                ValidateIssuerSigningKey = true,
                                ValidIssuer = _configuration["JwtSettings:ValidIssuer"],
                                //ValidAudience = _configuration["JwtSettings:ValidAudience"],
                                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]))
                            };
                        });

                        // cors pt.1
                        builder.Services.AddCors(policyBuilder =>
                            policyBuilder.AddDefaultPolicy(policy =>
                                policy.WithOrigins("http://localhost:3000").AllowAnyHeader().AllowAnyMethod())
                        );

                        var app = builder.Build();
                        
                        // cors pt.2
                        app.UseCors();

                        if (app.Environment.IsDevelopment())
                        {
                            app.UseSwagger();
                            app.UseSwaggerUI();
                        }
                        app.UseAuthorization();
                        app.UseAuthentication();
                        app.MapControllers();

                        return app;

                    }))
            };
        }
        #endregion Create listeners
    }
}
