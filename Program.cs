using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Serilog;
using Studex.Domain;
using Studex.Domain.Extensions;
using Studex.Middlewares;
using Studex.Repositories;
using Studex.Services;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();

var services = builder.Services;

var connectionString = builder.Configuration.GetValue<string>("Database:ConnectionString");
var poolSize = builder.Configuration.GetValue<int>("Database:PoolSize");

services.AddDbContextPool<StudexContext>(options =>
{
    options
        .UseNpgsql(connectionString)
        .ConfigureWarnings(w => w.Throw(RelationalEventId.MultipleCollectionIncludeWarning));
}, poolSize);

var domain = builder.Configuration["Auth0:Domain"];
var audience = builder.Configuration["Auth0:Audience"];

services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.Authority = $"https://{domain}/";
        options.Audience = audience;

        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true
        };
    });

services.AddAuthorization();

services.AddScopedServices();
services.AddScoped(typeof(ICrudRepository<>), typeof(RepositoryBase<>));

services.AddDateOnlyTimeOnlyStringConverters();

services.AddFluentValidationAutoValidation();
services.AddValidatorsFromAssemblyContaining<Program>();

services
    .AddControllers()
    .AddNewtonsoftJson(options =>
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

services.AddEndpointsApiExplorer();
services.AddSwaggerGen(options => options.UseDateOnlyTimeOnlyStringConverters());

services.AddHttpClient<IAIContentGenerator, HuggingFaceContentGenerator>();
services.AddScoped<ILectureService, LectureService>();
services.AddScoped<ITestService, TestService>();
services.AddScoped<ICourseService, CourseService>();

var app = builder.Build();

app.UseSerilogRequestLogging();

// if (app.Environment.IsDevelopment())
// {
app.UseSwagger();
app.UseSwaggerUI();
// }

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();