using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebApiBanco.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using System.Security.Principal;
using Microsoft.AspNetCore.Authorization;

namespace WebApiBanco
{
    public class Startup
    {
        public SymmetricSecurityKey signingKey;
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsEnvironment("Development"))
            {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);
            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseInMemoryDatabase("bancoDB"));
            services.AddMvc().AddJsonOptions(ConfigureJson);   
                    
            services.AddMvc(config =>
            {
                foreach (var formatter in config.InputFormatters)
                {
                    if (formatter.GetType() == typeof(JsonInputFormatter))
                        ((JsonInputFormatter)formatter).SupportedMediaTypes.Add(
                            MediaTypeHeaderValue.Parse("text/plain"));
                }
            });
            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme‌​)
                    .RequireAuthenticatedUser().Build());
            });
        }
        private void ConfigureJson(MvcJsonOptions obj)
        {
            obj.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        }
        private void ConfigureAuth(IApplicationBuilder app)
        {

            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("TokenAuthentication:SecretKey").Value));

            var tokenProviderOptions = new TokenProviderOptions
            {
                Path = Configuration.GetSection("TokenAuthentication:TokenPath").Value,
                Audience = Configuration.GetSection("TokenAuthentication:Audience").Value,
                Issuer = Configuration.GetSection("TokenAuthentication:Issuer").Value,
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256),
                IdentityResolver = GetIdentity
            };

            var tokenValidationParameters = new TokenValidationParameters
            {
                // The signing key must match!
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                // Validate the JWT Issuer (iss) claim
                ValidateIssuer = true,
                ValidIssuer = Configuration.GetSection("TokenAuthentication:Issuer").Value,
                // Validate the JWT Audience (aud) claim
                ValidateAudience = true,
                ValidAudience = Configuration.GetSection("TokenAuthentication:Audience").Value,
                // Validate the token expiry
                ValidateLifetime = true,
                // If you want to allow a certain amount of clock drift, set that here:
                ClockSkew = TimeSpan.Zero
            };


            app.UseJwtBearerAuthentication(new JwtBearerOptions
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                TokenValidationParameters = tokenValidationParameters
            });

            app.UseMiddleware<TokenProviderMiddleware>(Options.Create(tokenProviderOptions));
        }
        private Task<ClaimsIdentity> GetIdentity(string username, string password)
        {
            // DEMO CODE, DON NOT USE IN PRODUCTION!!!
            if (username == "TEST" && password == "TEST123")
            {
                return Task.FromResult(new ClaimsIdentity(new GenericIdentity(username, "Token"), new Claim[] { }));
            }

            // Account doesn't exists
            return Task.FromResult<ClaimsIdentity>(null);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory,ApplicationDbContext context)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseApplicationInsightsRequestTelemetry();

            app.UseApplicationInsightsExceptionTelemetry();
            app.UseMvc();

            app.UseJwtBearerAuthentication(new JwtBearerOptions());

            if (!context.Monedas.Any())
            {
                context.Monedas.AddRange(new List<Moneda>()
                {
                    new Moneda() {nombre="Soles"},
                    new Moneda() {nombre="Dólares"}
                });
                context.SaveChanges();
            }
            if (!context.Estados.Any())
            {
                context.Estados.AddRange(new List<Estado>()
                {
                    new Estado() {nombre="Pagada"},
                    new Estado() {nombre="Declinada"},
                    new Estado() {nombre="Fallida"},
                    new Estado() {nombre="Anulada"}
                });
                context.SaveChanges();
            }
            if (!context.Bancos.Any())
            {
                context.Bancos.AddRange(new List<Banco>()
                {
                    new Banco() {nombre="BBVA",direccion="Calle 123 - Miraflores,Lima",fechareg=new DateTime(2006, 6, 6, 18, 32, 0),
                                Sucursales=new List<Sucursal>() {
                                new Sucursal() {nombre="BBVA-LM",direccion="Calles 456 - La Molina,Lima",fechareg=new DateTime(2007, 6, 6, 18, 32, 0)}
                                }},
                    new Banco() {nombre="BCP" ,direccion="Calle 687 - La Molina,Lima",fechareg=new DateTime(2007, 6, 6, 6, 32, 0),
                                Sucursales=new List<Sucursal>() {
                                new Sucursal() {nombre="BCP-SM",direccion="Calles 789 - San Miguel,Lima",fechareg=new DateTime(2008, 6, 6, 18, 32, 0)},
                                new Sucursal() {nombre="BCP-SI",direccion="Calles 956 - San Isidro,Lima",fechareg=new DateTime(2009, 6, 6, 18, 32, 0)}
                                }},
                    new Banco() {nombre="MiBanco" ,direccion="Calle 234 - Surquillo,Lima",fechareg=new DateTime(2008, 6, 6, 6, 32, 0),
                                Sucursales=new List<Sucursal>() {
                                new Sucursal() {nombre="MB-SM",direccion="Calles 78 - San Miguel,Lima",fechareg=new DateTime(2009, 6, 6, 18, 32, 0)},
                                new Sucursal() {nombre="MB-SI",direccion="Calles 112 - San Isidro,Lima",fechareg=new DateTime(2010, 6, 6, 18, 32, 0)}
                                }
                    } }
                );
                context.SaveChanges();
            }
            if (!context.OPagos.Any())
            {
                context.OPagos.AddRange(new List<OPago>()
                {
                    new OPago() {monto=100.5,MonedaId=1,EstadoId=1,fechapag=new DateTime(2015, 7, 6, 18, 32, 0),SucursalId=2},
                    new OPago() {monto=200,MonedaId=2,EstadoId=2,fechapag=new DateTime(2017, 8, 6, 18, 32, 0),SucursalId=2},
                    new OPago() {monto=399,MonedaId=1,EstadoId=3,fechapag=new DateTime(2016, 9, 6, 18, 32, 0),SucursalId=5},
                    new OPago() {monto=99,MonedaId=1,EstadoId=4,fechapag=new DateTime(2015, 3, 6, 18, 32, 0),SucursalId=2},
                    new OPago() {monto=45,MonedaId=2,EstadoId=1,fechapag=new DateTime(2014, 2, 6, 18, 32, 0),SucursalId=4},
                    new OPago() {monto=33,MonedaId=2,EstadoId=2,fechapag=new DateTime(2012, 1, 6, 18, 32, 0),SucursalId=2}
                });
                context.SaveChanges();
            }
        }
    }
}
