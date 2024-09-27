using Microsoft.EntityFrameworkCore;
using RetailProductManagement.Core.Contracts.Repositories;
using RetailProductManagement.Core.Contracts.Services;
using RetailProductManagement.Infrastructure.Data;
using RetailProductManagement.Infrastructure.Repositories;
using RetailProductManagement.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddScoped<IApprovalQueueService, ApprovalQueueService>();
builder.Services.AddScoped<IApprovalQueueRepository, ApprovalQueueRepository>();

builder.Services.AddDbContext<RetailDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("RetailProductDbConnection"))
);
var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

