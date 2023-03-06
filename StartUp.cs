using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using Web_API_Bee_Haak.Data;

namespace Web_API_Bee_Haak;

public class StartUp
    {
        public StartUp(IConfiguration configuration)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            Configuration = configuration;
        }

        public IConfiguration Configuration {get;}

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
            services.AddDbContext<AplicationdbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("defaultConnection")));

            services.AddResponseCaching();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(option => option.TokenValidationParameters = new TokenValidationParameters{
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration ["KeyJwt"])),
                ClockSkew = TimeSpan.Zero
            });

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c => 
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "API_Bee_Haak", Version ="v1"});

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme{
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In =ParameterLocation.Header,
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement{
                    {
                        new OpenApiSecurityScheme{
                            Reference = new OpenApiReference{
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });

            services.AddAutoMapper(typeof(StartUp));

            services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AplicationdbContext>().AddDefaultTokenProviders();

            services.AddAuthorization(options => {
                options.AddPolicy("Admin", polyce => polyce.RequireClaim("Rol", "Admin"));
                options.AddPolicy("User", polyce => polyce.RequireClaim("Rol", "User"));
            });


            services.AddCors(option => {
                option.AddDefaultPolicy(builder => {
                    builder.WithOrigins("https://apirequest.io").AllowAnyMethod().AllowAnyHeader()
                    .WithExposedHeaders(new string[] {"TotalCuantityRegisters"});
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors(
                builder => {
                    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                }
            );
            app.UseAuthorization();
            app.UseEndpoints(endpoints => 
            {
                endpoints.MapControllers();
            }
            );
        }
    }