using Azure.Storage.Blobs;
using DikkeTennisLijst.Core.Entities;
using DikkeTennisLijst.Core.Equatables;
using DikkeTennisLijst.Core.Interfaces.Repositories;
using DikkeTennisLijst.Core.Interfaces.Services;
using DikkeTennisLijst.Core.Services;
using DikkeTennisLijst.Core.Shared.Helpers;
using DikkeTennisLijst.Infrastructure.Data;
using DikkeTennisLijst.Infrastructure.Repositories;
using DikkeTennisLijst.WebAPI.Filters.Action;
using DikkeTennisLijst.WebAPI.Filters.Exception;
using DikkeTennisLijst.WebAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace DikkeTennisLijst.WebAPI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(ValidationFilter));
                options.Filters.Add(typeof(ExceptionFilter));
            });

            services.AddScoped<ValidationFilter>()
                    .AddScoped<ExceptionFilter>();

            services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("TennisAppConnectionString"),
                    config => config.EnableRetryOnFailure(
                        maxRetryCount: 6,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null)
                    )
                );

            services.AddScoped<DbContext, ApplicationContext>();

            services.AddSingleton(new BlobServiceClient(
                Configuration.GetConnectionString("StorageConnectionString")));

            AddRepositoriesAndServices(services);

            services.AddAutoMapper(typeof(Startup));

            services.AddIdentity<Player, IdentityRole>()
                    .AddEntityFrameworkStores<ApplicationContext>()
                    .AddDefaultTokenProviders();

            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.Configure<IdentityOptions>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            });

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.ASCII.GetBytes(Configuration.GetValue<string>("AppSettings:JwtSecretKey"))),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        }

        public static void AddRepositoriesAndServices(IServiceCollection services)
        {
            // Todo: Could probably just as well use the SQL repository for this.
            services.AddScoped<IObjectRepository<EmailTemplate>, BlobObjectRepository<EmailTemplate>>();
            services.AddScoped<ITextFileRepository, TextFileRepository>()
                    .AddScoped<IEmailRepository, SendGridRepository>();

            services.AddScoped(typeof(IEntityRepository<>), typeof(SQLEntityRepository<>));

            services.AddScoped<IPlayerService, PlayerService>()
                    .AddScoped<IAccountService, AccountService>()
                    .AddScoped<IFollowingService, FollowingService>()
                    .AddScoped<IEloRankedService, EloRankedService>()
                    .AddScoped<IEloCasualService, EloCasualService>()
                    .AddScoped<IMatchService, MatchService>()
                    .AddScoped<IClubService, ClubService>()
                    .AddScoped<ISurfaceService, SurfaceService>();

            services.AddScoped<IEmailService, EmailService>();
        }

        public static void Configure(IApplicationBuilder app)
        {
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers().RequireAuthorization();
                endpoints.MapFallbackToFile("/index.html");
            });
        }
    }
}