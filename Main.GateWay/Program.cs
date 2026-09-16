using Main.GateWay.ActionFilters;
using Main.GateWay.Protos;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(o =>
{
    o.Filters.Add<HttpResponseExceptionFilter>();
});

builder.Services.AddGrpcClient<Customer.CustomerClient>(o =>
{
    o.Address = new Uri("http://localhost:5259");
});

var app = builder.Build();

app.MapControllers();

app.Run();