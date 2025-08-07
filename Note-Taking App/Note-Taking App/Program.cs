using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Note_Taking_App.Applcation.CQRS.Handlers;
using Note_Taking_App.Applcation.Mapping;
using Note_Taking_App.Applcation.Services;
using Note_Taking_App.Core.Entities;
using Note_Taking_App.Core.Interfaces.Repositories;
using Note_Taking_App.Core.Interfaces.Services;
using Note_Taking_App.Infrastructure.DBContext;
using Note_Taking_App.Infrastructure.Repositories;
using Note_Taking_App.Middlewares;
using Note_Taking_App.ResponseModel;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using System.Text;
var builder = WebApplication.CreateBuilder(args);


builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Note-Taking App", Version = "v1" });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer {token}'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
{
    {
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            }
        },
        Array.Empty<string>()
    }
});

});
builder.Services.AddDbContext<AppDbContext>(opt =>
               opt.UseSqlServer(builder.Configuration.GetConnectionString("MyConn"))
           );
builder.Services.AddDbContext<NotesDbContext>(opt =>
               opt.UseSqlServer(builder.Configuration.GetConnectionString("MyConn"))
           );

builder.Services.AddScoped<INoteRepository, NoteRepository>();

builder.Services.AddIdentityCore<ApplicationUser>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 3;

    options.User.RequireUniqueEmail = true;
}).AddEntityFrameworkStores<AppDbContext>();


builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
         
            }).AddJwtBearer(options =>
                {
                    var keyFromConfig = builder.Configuration.GetValue<string>("Jwt:Key")!;
                    var keyInBytes = Encoding.ASCII.GetBytes(keyFromConfig);
                    var key = new SymmetricSecurityKey(keyInBytes);

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        IssuerSigningKey = key
                    };
                });
builder.Services.AddAuthorization();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
    typeof(Program).Assembly,
    typeof(CreateNoteCommandHandler).Assembly
));
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin();
        policy.AllowAnyMethod();
        policy.AllowAnyHeader();
    });
});

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = actionContext =>
    {
        return new BadRequestObjectResult(
            ApiResponse<object>.ErrorResponse(actionContext.ModelState.Values.SelectMany(x => x.Errors)
            .Select(x => x.ErrorMessage).ToList()));
    };
});
Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
                .WriteTo.Console()
                .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Infinite, shared: true)
                .WriteTo.MSSqlServer(
                    connectionString: builder.Configuration.GetConnectionString("MyConn"),
                    sinkOptions: new MSSqlServerSinkOptions { TableName = "Logs", AutoCreateSqlTable = true },
                    columnOptions: new ColumnOptions())
                .CreateLogger();


builder.Host.UseSerilog();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});




var app = builder.Build();
app.UseCors("AllowAll");
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseMiddleware<RequestLoggingMiddleware>();



app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();
app.Use(async (context, next) =>
{
    Console.WriteLine($"Request: {context.Request.Path}");
    await next();
    Console.WriteLine($"Response: {context.Response.StatusCode}");
});
app.Run();
