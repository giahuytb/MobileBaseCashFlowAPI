using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Microsoft.EntityFrameworkCore;

using MobileBasedCashFlowAPI.MongoRepositories;
using MobileBasedCashFlowAPI.IMongoRepositories;
using MobileBasedCashFlowAPI.Settings;

using MobileBasedCashFlowAPI.Repositories;
using MobileBasedCashFlowAPI.IRepositories;
using MobileBasedCashFlowAPI.Models;

using MobileBasedCashFlowAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);
var secretKey = builder.Configuration["Jwt:Key"];
var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);
var myAllowSpecificOrigins = "_myAllowSpecificOrigins";
// Inject data from appsettings to MailSettings
var mailSettings = builder.Configuration.GetSection("MailSettings");

builder.Services.AddOptions();

builder.Services.AddControllers().AddJsonOptions(options =>
 {
     options.JsonSerializerOptions.PropertyNamingPolicy = null;
 });

builder.Services.AddEndpointsApiExplorer();

builder.Services.Configure<MailSettings>(mailSettings);
// Inject data from appsettings to GameDBSettings
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection(nameof(MongoDbSettings)));

// Enable Cors
builder.Services.AddCors(option =>
{
    option.AddPolicy(name: myAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
        });
});
// Add Database context To Project

builder.Services.AddDbContext<MobileBasedCashFlowGameContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerDb"));
});

// Register Service For SqlServer Database
builder.Services.AddTransient<ISendMailRepository, SendMailRepository>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IUserRoleRepository, UserRoleRepository>();

builder.Services.AddTransient<IAssetRepository, AssetRepository>();
builder.Services.AddTransient<IAssetTypeRepository, AssetTypeRepository>();
builder.Services.AddTransient<IGameMatchRepository, GameMatchRepository>();
builder.Services.AddTransient<IGameModRepository, GameModRepository>();
builder.Services.AddTransient<IGameReportRepository, GameReportRepository>();
builder.Services.AddTransient<IGameServerRepository, GameServerRepository>();
builder.Services.AddTransient<IGameRepository, GameRepository>();
builder.Services.AddTransient<IParticipantRepository, ParticipantRepository>();
builder.Services.AddTransient<IPOIRepository, POIRepository>();
builder.Services.AddTransient<IUserAssetRepository, UserAssetRepository>();

// Register Service For MongoDatabase
builder.Services.AddTransient<MongoDbSettings>(sp => sp.GetRequiredService<IOptions<MongoDbSettings>>().Value);
builder.Services.AddTransient<IDreamRepository, DreamRepository>();
builder.Services.AddTransient<IEventCardRepository, EventCardRepository>();
builder.Services.AddTransient<IGameAccountRepository, GameAccountRepository>();
builder.Services.AddTransient<IJobCardRepository, JobCardRepository>();
builder.Services.AddTransient<ITileRepository, TileRepository>();

// Register Service For Cache
builder.Services.AddMemoryCache();

// Register Redis Cache
builder.Services.AddStackExchangeRedisCache(redisOptions =>
{
    string connection = builder.Configuration.GetConnectionString("Redis");
    redisOptions.Configuration = connection;
});

builder.Services.AddDistributedMemoryCache();

// Register Global Exception Handler Midddleware
builder.Services.AddTransient<GlobalExceptionHandlingMiddleware>();



// Config Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        // Sign In token
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ClockSkew = TimeSpan.Zero,
    };
});

// Add Swagger And Bearer Authenticate To Project
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MobileBaseCashFlowGame", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 345hkbkyy'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
        {{
            new OpenApiSecurityScheme
                {
                Reference = new OpenApiReference
                    {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                    },
                Scheme = "oauth",
                Name = "Bearer",
                In = ParameterLocation.Header,
                },
            new List<string>()
         }}
        );
}
);

// lower case endpoint url
builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
});


var app = builder.Build();

// use app.Environment.IsProduction() to enable swagger after deploy
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseDeveloperExceptionPage();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Cash Flow Game Api v1");
        // Set URl as /index.html
        c.RoutePrefix = String.Empty;
    });
}

app.UseHttpsRedirection();

app.UseCors(myAllowSpecificOrigins);

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware(typeof(GlobalExceptionHandlingMiddleware));

app.MapControllers();

app.Run();
