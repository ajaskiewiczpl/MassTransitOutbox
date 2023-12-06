using System.Reflection;
using MassTransit;
using MassTransitOutbox;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration["ConnectionStrings:Sql"]);
});

builder.Services.AddMassTransit(busConfigurator =>
{
    busConfigurator.AddConsumers(Assembly.GetExecutingAssembly());

    busConfigurator.SetKebabCaseEndpointNameFormatter();

    busConfigurator.UsingRabbitMq((context, configurator) =>
    {
        configurator.Host(new Uri(builder.Configuration["MessageBroker:Host"]!), h =>
        {
            h.Username(builder.Configuration["MessageBroker:Username"]);
            h.Password(builder.Configuration["MessageBroker:Password"]);
        });

        configurator.ConfigureEndpoints(context);
    });

    busConfigurator.AddEntityFrameworkOutbox<AppDbContext>(options =>
    {
        options.UsePostgres();
        options.UseBusOutbox();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using var scope = app.Services.CreateScope();
    var appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    appDbContext.Database.Migrate();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
