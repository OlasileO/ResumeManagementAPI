using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ResumeManagementAPI.IRepository;
using ResumeManagementAPI.Models.Data;
using ResumeManagementAPI.Repository;
using Serilog;
using Serilog.Events;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ResumeContext>(options => options.UseSqlServer(
builder.Configuration.GetConnectionString("conn")));
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddControllers().AddJsonOptions(x =>
{
x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.AddScoped<ICompanyRepo, CompanyRepo>();
builder.Services.AddScoped<ICandidateRepo, CandidateRepo>();
builder.Services.AddScoped<IJobRepo, JobRepo>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
      a => a.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());

});

var _logger = new LoggerConfiguration()
    .WriteTo.File(
     path: "C:\\ResumeManagement\\logs\\log-.txt",
     outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} {Level:u3}] {Username} {Message:lj}{NewLine}{Exception}",
     rollingInterval: RollingInterval.Day,
     restrictedToMinimumLevel: LogEventLevel.Information
    ).CreateLogger();
builder.Logging.AddSerilog(_logger);
try
{
    Log.Information("Application is Starting");

}
catch (Exception ex)
{

    Log.Fatal("Application fail to start");
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowAll");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
