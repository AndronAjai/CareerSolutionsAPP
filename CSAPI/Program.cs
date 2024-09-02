using CSAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using CSAPI.Controllers;
using CSAPI.Areas.EmployerArea.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("V1", new OpenApiInfo
    {
        Version = "V1",
        Title = "WebAPI",
        Description = "Career Solutions WebAPI"
    });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "Bearer Authentication with JWT Token",
        Type = SecuritySchemeType.Http
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
    });
});

// Register services and repositories
builder.Services.AddDbContext<CareerSolutionsDB>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("CnString")));
builder.Services.AddTransient<IUserRepo, UserRepo>();  // Register UserRepo
builder.Services.AddTransient<ILogin, LoginRepo>();
builder.Services.AddTransient<IJobsRepo, JobsRepo>();
builder.Services.AddTransient<IJobSeekerRepo, JobSeekerRepo>();
builder.Services.AddTransient<IEmployerRepo, EmployerRepo>();
builder.Services.AddTransient<IBranchOfficeRepo, BranchOfficesRepo>();
builder.Services.AddTransient<IApplicationRepo, ApplicationRepo>();
builder.Services.AddTransient<IEmployerAreaRepo, EmployerAreaRepo>();
builder.Services.AddTransient<INotificationRepo, NotificationRepo>();
//builder.Services.AddTransient<IJobStatusNotificationRepo, JobStatusNotificationRepo>();

// Configure Authentication with JWT and Role-Based Authorization
builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
    };
});

// Ensure that role-based authorization is configured
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
    options.AddPolicy("Employer", policy => policy.RequireRole("Employer"));
    options.AddPolicy("JobSeeker", policy => policy.RequireRole("JobSeeker"));
});

var app = builder.Build();

app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{ 
    app.UseSwagger();
    app.UseSwaggerUI(options => {
        options.SwaggerEndpoint("/swagger/V1/swagger.json", "Career Solutions WebAPI");
    });
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Define endpoint routing with role-based policies
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );

    endpoints.MapControllerRoute(
      name: "default",
      pattern: "{controller=LoginView}/{action=Index}/{id?}"
    );

    endpoints.MapControllers();
});

app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.Run();