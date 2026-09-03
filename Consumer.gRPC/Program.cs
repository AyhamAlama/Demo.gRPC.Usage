using Consumer.gRPC.Services;
using GrpcService2.Protos;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc(option =>
{
    option.Interceptors.Add<Consumer.gRPC.Interceptors.ExceptionHandlingInterceptor>();
});

builder.Services.AddGrpcClient<Customer.CustomerClient>(o =>
{
    o.Address = new Uri("http://localhost:5259");
});

var app = builder.Build();

app.MapGrpcService<RebuildCustomerService>();

app.Run();
