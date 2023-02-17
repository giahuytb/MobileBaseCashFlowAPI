using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using MobileBasedCashFlowAPI.Settings;
using MobileBasedCashFlowAPI.IServices;
using MobileBasedCashFlowAPI.Se;
using MobileBasedCashFlowAPI.Models;
using Microsoft.AspNetCore.Diagnostics;
using static System.Net.Mime.MediaTypeNames;
using MobileBasedCashFlowAPI.MongoServices;
using MobileBasedCashFlowAPI.IMongoServices;

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
builder.Services.AddTransient<ISendMailService, SendMailService>();

builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IItemService, ItemService>();
builder.Services.AddTransient<IBoardService, BoardService>();
builder.Services.AddTransient<IDreamService, DreamService>();
builder.Services.AddTransient<IEventCardService, EventCardService>();

// Register Service For MongoDatabase
builder.Services.AddTransient<MongoDbSettings>(sp => sp.GetRequiredService<IOptions<MongoDbSettings>>().Value);
builder.Services.AddTransient<IMgEventCardService, MgEventCardService>();
builder.Services.AddTransient<IMgTileService, MgTileService>();
builder.Services.AddTransient<IMgJobCardService, MgJobCardService>();
builder.Services.AddTransient<IMgDreamService, MgDreamService>();
builder.Services.AddTransient<IMgFinancialReportService, MgFinancialReportService>();

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
    app.UseExceptionHandler(exceptionHandlerApp =>
    {
        exceptionHandlerApp.Run(async context =>
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            // using static System.Net.Mime.MediaTypeNames;
            context.Response.ContentType = Text.Plain;

            await context.Response.WriteAsync("An exception was thrown.");

            var exceptionHandlerPathFeature =
                context.Features.Get<IExceptionHandlerPathFeature>();

            if (exceptionHandlerPathFeature?.Error is FileNotFoundException)
            {
                await context.Response.WriteAsync(" The file was not found.");
            }

            if (exceptionHandlerPathFeature?.Path == "/")
            {
                await context.Response.WriteAsync(" Page: Home.");
            }
        });
    });
}

app.UseHttpsRedirection();

app.UseCors(myAllowSpecificOrigins);

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
