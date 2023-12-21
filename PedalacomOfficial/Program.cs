using Microsoft.EntityFrameworkCore;
using PedalacomOfficial;
using PedalacomOfficial.Data;
using PedalacomOfficial.Models;
using PedalacomOfficial.Repositories.Implementation;
using PedalacomOfficial.Repositories.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // Qui puoi aggiungere configurazioni personalizzate per Swagger
    options.SchemaFilter<ExcludePropertiesSchemaFilter>();
});
builder.Services.AddDbContext<AdventureWorksLt2019Context>(options =>

    options.UseSqlServer(builder.Configuration.GetConnectionString("PedalacomConnectionString"))
);

builder.Services.AddScoped<IAddressRepository, AddressRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(options =>
{
    options.AllowAnyHeader();
    options.AllowAnyOrigin();
    options.AllowAnyMethod();
}
);


app.UseAuthorization();

app.MapControllers();

app.Run();
