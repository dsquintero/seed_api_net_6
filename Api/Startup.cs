using Api.Filters.Action;
using Api.Filters.Exception;
using Api.Models;
using Api.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace Api
{

    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Configuración de servicios
            // disabled default entity validation
            services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);

            // add Hosted Service
            //services.AddHostedService<IntervalTaskHostedService>();

            services.AddCors(options =>
            {
                options.AddPolicy(name: "CorsPolicy", builder =>
                {
                    builder.AllowAnyOrigin();
                    builder.AllowAnyMethod();
                    builder.AllowAnyHeader();
                });
            });

            // Add services to the container.
            services.AddControllers(o =>
            {
                o.Filters.Add<GlobalExceptionFilter2>();
                o.Filters.Add<ValidationAttributeActionFilter>();
            });

            //add response caching
            services.AddResponseCaching();

            // add object environment
            EnvironmentConfig env = new EnvironmentConfig();
            _configuration.Bind(env);
            services.AddSingleton(env);

            //add object app
            AppConfiguration appConfiguration = new AppConfiguration();
            _configuration.Bind("App", appConfiguration);
            services.AddSingleton(appConfiguration);

            // add AutoMapper
            services.AddAutoMapper(typeof(Startup));

            //add Authentication JWT Bearer
            //Generate a random JWT secret

            //node -e "console.log(require('crypto').randomBytes(32).toString('hex'))"

            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //    .AddJwtBearer(o =>
            //    {
            //        o.TokenValidationParameters = new TokenValidationParameters()
            //        {
            //            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(env.JWT_AccessTokenSecret)),
            //            ValidIssuer = env.JWT_Issuer,
            //            ValidAudience = env.JWT_Audience,
            //            ValidateIssuerSigningKey = true,
            //            ValidateIssuer = true,
            //            ValidateAudience = true,
            //            ClockSkew = TimeSpan.Zero
            //        };
            //    });


            // add sevices Extensions
            services.addServices();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = appConfiguration.Title, Version = appConfiguration.Version, Description = appConfiguration.Description });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                var JwtSchema = CreateSecurityScheme("Authorization", "JWT authorization", "JWT authorization using the Bearer scheme. Example: \"Authorization: Bearer {token}\"", ParameterLocation.Header, SecuritySchemeType.Http);
                JwtSchema.Scheme = "bearer"; // Especifica el esquema "bearer"

                var ApiKeySchema = CreateSecurityScheme("x-api-key", "API Key Authorization", "API Key Authorization header. Example: \"x-api-key: your_api_key_here\"", ParameterLocation.Header, SecuritySchemeType.ApiKey);

                c.AddSecurityDefinition("JWT authorization", JwtSchema);
                c.AddSecurityDefinition("API Key Authorization", ApiKeySchema);

                c.AddSecurityRequirement(CreateSecurityRequirement(JwtSchema, "Bearer"));
                c.AddSecurityRequirement(CreateSecurityRequirement(ApiKeySchema, "api_key"));
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Configuración de la aplicación
            // Configure the HTTP request pipeline.          
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            //add response caching
            app.UseResponseCaching();

            app.UseRouting();

            // global cors policy
            app.UseCors("CorsPolicy");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        private static OpenApiSecurityScheme CreateSecurityScheme(string name, string reference, string description, ParameterLocation location, SecuritySchemeType schemeType)
        {
            return new OpenApiSecurityScheme
            {
                Name = name,
                Description = description,
                In = location,
                Type = schemeType,
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = reference
                }
            };
        }

        private static OpenApiSecurityRequirement CreateSecurityRequirement(OpenApiSecurityScheme scheme, string name)
        {
            return new OpenApiSecurityRequirement
            {
                { scheme, new[] { name } }
            };
        }
    }
}
