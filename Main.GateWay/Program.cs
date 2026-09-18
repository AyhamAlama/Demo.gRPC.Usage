using Grpc.Core;
using Grpc.Net.Client.Configuration;
using Main.GateWay.ActionFilters;
using Main.GateWay.Protos;
using Polly;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddResiliencePipeline("def", x =>
{
    x.AddRetry(new Polly.Retry.RetryStrategyOptions
    {
        ShouldHandle = new PredicateBuilder().Handle<Exception>(
            ex => ex is RpcException || ex is IOException),
        Delay = TimeSpan.FromMilliseconds(200),
        BackoffType = DelayBackoffType.Exponential,
        MaxRetryAttempts = 3,
        UseJitter = true
    }).AddTimeout(TimeSpan.FromSeconds(5));
});

builder.Services.AddControllers(o =>
{
    o.Filters.Add<HttpResponseExceptionFilter>();
});

builder.Services.AddGrpcClient<Customer.CustomerClient>(o =>
{
    o.Address = new Uri("http://localhost:5259");
}).ConfigureChannel(o =>
{
    o.ServiceConfig = new Grpc.Net.Client.Configuration.ServiceConfig
    {
        MethodConfigs = {
            new Grpc.Net.Client.Configuration.MethodConfig
            {
                Names = {MethodName.Default},
                RetryPolicy = new RetryPolicy
                {
                    MaxAttempts = 3,
                    InitialBackoff = TimeSpan.FromMilliseconds(200),
                    MaxBackoff = TimeSpan.FromSeconds(2),
                    BackoffMultiplier = 2,
                    RetryableStatusCodes =
                    {
                        Grpc.Core.StatusCode.Unavailable
                    }
                }

            }
        }
    };
});

var app = builder.Build();

app.MapControllers();

app.Run();