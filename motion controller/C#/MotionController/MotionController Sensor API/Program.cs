using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using MotionController.Extensions.DependencyInjection;
using Serilog;
using VictorKrogh.Extensions.Autofac;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true);

var motionOptions = builder.Configuration.GetSection(MotionOptions.Motion).Get<MotionOptions>();

// Add services to the container.
builder.Services.AddMotion(builder.Configuration);

builder.Services.AddControllers(controller =>
{
    controller.Filters.Add<UnitOfWorkAttribute>();
});
builder.Services.AddOpenApiDocument(c =>
{
    c.Version = "1.0.0";
    c.Description = "API interface for all internal sensor operations.";
    c.Title = "Internal MotionController Sensor API";
});

builder.Host.UseSerilog((context, loggerConfiguration) => loggerConfiguration.ReadFrom.Configuration(context.Configuration));
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterMotionController(motionOptions)
        .WithMQTTClientBackgroundService();

    containerBuilder.RegisterUnitOfWorkFactory();
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseOpenApi();
app.UseSwaggerUi3();
app.UseReDoc();

app.UseHttpsRedirection();

app.UseSerilogRequestLogging();

app.UseAuthorization();

app.MapControllers();

app.Run();
