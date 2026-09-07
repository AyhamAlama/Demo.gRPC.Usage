using Calzolari.Grpc.AspNetCore.Validation;
using GrpcService2.Services;

using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc(options =>
{
    options.EnableMessageValidation();
});

builder.Services.AddGrpcValidation();

builder.Services.AddValidators();

builder.Services.AddDbContext<GrpcService2.Data.CustomerDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

app.MapGrpcService<CustomerService>();

app.Run();