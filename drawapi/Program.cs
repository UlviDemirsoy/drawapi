using drawapi.Data;
using drawapi.Mappings;
using drawapi.Middlewares;
using drawapi.Repositories.Abstracts;
using drawapi.Repositories.Concretes;
using drawapi.Services.Abstracts;
using drawapi.Services.Concretes;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddDbContext<AppDbContext>(options =>
//    options.UseInMemoryDatabase("ApplicationDb"));

builder.Logging.ClearProviders(); 
builder.Logging.AddConsole();      
builder.Logging.AddDebug();


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));


builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ITeamRepository, TeamRepository>();
builder.Services.AddScoped<ITeamService, TeamService>();
builder.Services.AddScoped<IDrawService, DrawService>();
builder.Services.AddScoped<IGroupRepository, GroupRepository>();
builder.Services.AddScoped<IDrawRepository, DrawRepository>();
builder.Services.AddScoped<IGroupService, GroupService>();
builder.Services.AddScoped<IDrawService, DrawService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("Application is starting...");


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    SeedData.Initialize(context);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
