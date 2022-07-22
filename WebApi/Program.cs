using System.Reflection;

using AutoMapper;
using CommonFunctions;
using CommonFunctions.Interfaces;
using Domain.Models;
using MediatR;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using PCP.Application.EmailObservablePattern.Classes;
using PCP.Application.EmailObservablePattern.Interfaces;
using PCP.Application.Entities.Course.Commands.DateConverter;
using PCP.Application.Entities.Student.Commands.AddStudentToCourse;
using PCP.Application.Entities.User.Queries.GetAllUsers;
using PCP.Application.Mapper;
using Persistence;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;
using ILogger = Serilog.ILogger;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped(typeof(UmsContext), typeof(UmsContext));

builder.Services.AddMediatR(typeof(GetAllUsersQuery).GetTypeInfo().Assembly);
//add Service Type Mediator of sendEmail
builder.Services.AddScoped<SendEmail>();
builder.Services.AddScoped<ISendEmail, SendEmail>();
builder.Services.AddScoped<ISubject, Subject>();

//add Service Type Mediator of Subject
builder.Services.AddScoped<Subject>();

ConfigureLogs(); //Implement seriolog Config with elastic search


var mapperConfig = new MapperConfiguration(mc =>
{
    
    mc.AddProfile(new CourseMapper());
    mc.AddProfile(new ClassMapper());
    mc.AddProfile(new UserMapper());
    
});

IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);


builder.Services.AddMvc();

builder.Services
    .AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new DateOnlyConverter()));

//Add OData
static IEdmModel GetEdmModel()
{
    ODataConventionModelBuilder builder = new();
    builder.EntitySet<User>("Users");
    return builder.GetEdmModel();
}

builder.Services.AddControllers().AddOData(opt => 
    opt.AddRouteComponents("v1", GetEdmModel()).Filter().Select().Expand().OrderBy());



builder.Host.UseSerilog();




var app = builder.Build();
//Migration
using var scope = builder.Services.BuildServiceProvider().CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<UmsContext>();
dbContext.Database.SetConnectionString($"Host=localhost;Port=5432;Database=UMSWithMig;Username=postgres;Password=123456");
    
RelationalDatabaseCreator databaseCreator = 
    (RelationalDatabaseCreator) dbContext.Database.GetService<IDatabaseCreator>();
try
{
       
    databaseCreator.CreateTables();
}
catch (Exception e)
{
    //A SqlException will be thrown if tables already exist. So simply ignore it.
    Console.WriteLine("Tables already exists");
}
    

dbContext.Database.Migrate();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// For Multitanency project, working fine but not implemented in this project
/*
List<Tanent> TanentList = new List<Tanent>();
Tanent VIP1 = new Tanent(1, "Client1", "VIP");
Tanent Normal1 = new Tanent(2, "Client2", "Normal");
Tanent VIP2 = new Tanent(3, "Client3", "VIP");
Tanent Normal2 = new Tanent(4, "Client4", "Normal");
TanentList.Add(VIP1);
TanentList.Add(Normal1);
TanentList.Add(VIP2);
TanentList.Add(Normal2);

foreach (Tanent t in TanentList)
{
    string name = null;
    if (t.Subscription == "VIP")
    {
        //set the name of the connection string as the name of the client
        name = t.Name;
    }
    else if(t.Subscription == "Normal")
    {
        //set the name of a db that is the same for all normal tanent
        name = "Normal";
    }
    //create connection string and migrate
    //"Host=localhost;Port=5432;Database={name};Username=postgres;Password=123456"
    builder.Services.AddDbContext<UmsContext>();
    
    using var scope = builder.Services.BuildServiceProvider().CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<UmsContext>();
    dbContext.Database.SetConnectionString($"Host=localhost;Port=5432;Database={name};Username=postgres;Password=123456");
    
    RelationalDatabaseCreator databaseCreator = 
        (RelationalDatabaseCreator) dbContext.Database.GetService<IDatabaseCreator>();
    try
    {
       
        databaseCreator.CreateTables();
    }
    catch (Exception e)
    {
        //A SqlException will be thrown if tables already exist. So simply ignore it.
        Console.WriteLine("Tables already exists");
    }
    

    dbContext.Database.Migrate();
    
}
//We should afterward erase the scaffold config in the UMSContext to let it work
//Please note that Migration is working fine and tables are created for the Multitanency
*/
app.UseAuthorization();

app.MapControllers();

app.Run();


#region helper
void ConfigureLogs()
{
    // Get the environment which the application is running on
    var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
    
    // Get the configuration 
    var configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .Build();

    // Create Logger
    Log.Logger = new LoggerConfiguration()
        .Enrich.FromLogContext()
        .Enrich.WithExceptionDetails() // Adds details exception
        .WriteTo.Debug()
        .WriteTo.Console()
        .WriteTo.Elasticsearch(ConfigureELS(configuration, env))
        .CreateLogger();
}

ElasticsearchSinkOptions ConfigureELS(IConfigurationRoot configuration, string env)
{
    return new ElasticsearchSinkOptions(new Uri(configuration["ELKConfiguration:Uri"]))
    {
        AutoRegisterTemplate = true,
        IndexFormat = $"{Assembly.GetExecutingAssembly().GetName().Name.ToLower()}-{env.ToLower().Replace(".","-")}-{DateTime.UtcNow:yyyy-MM}"
    };
}
#endregion