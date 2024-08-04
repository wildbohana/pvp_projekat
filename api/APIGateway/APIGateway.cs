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
        public APIGateway(StatelessServiceContext context) : base(context) { }

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

                        //#region Jwt config
                        //var jwtIssuer = builder.Configuration.GetSection("Jwt:Issuer").Get<string>();
                        //var jwtKey = builder.Configuration.GetSection("Jwt:Key").Get<string>();

                        //builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
                        //{
                        //    options.TokenValidationParameters = new TokenValidationParameters
                        //    {
                        //        ValidateIssuer = true,
                        //        ValidateAudience = true,
                        //        ValidateLifetime = true,
                        //        ValidateIssuerSigningKey = true,
                        //        ValidIssuer = jwtIssuer,
                        //        ValidAudience = jwtIssuer,
                        //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
                        //    };
                        //});
                        //#endregion
                        
                        builder.Services.AddSingleton<StatelessServiceContext>(serviceContext);
                        builder.WebHost
                                    .UseKestrel()
                                    .UseContentRoot(Directory.GetCurrentDirectory())
                                    .UseServiceFabricIntegration(listener, ServiceFabricIntegrationOptions.None)
                                    .UseUrls(url);
                        builder.Services.AddControllers();
                        builder.Services.AddEndpointsApiExplorer();
                        builder.Services.AddSwaggerGen();

                        // cors pt.1
                        // alt: policy.WithOrigins("http://localhost:3000")
                        builder.Services.AddCors(policyBuilder =>
                            policyBuilder.AddDefaultPolicy(policy =>
                                policy.WithOrigins("*").AllowAnyHeader().AllowAnyMethod())
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
                        app.MapControllers();

                        return app;

                    }))
            };
        }
        #endregion Create listeners
    }
}
