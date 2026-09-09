using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcService2.Data;
using GrpcService2.Protos;

namespace GrpcService2.Services
{
    public class CustomerService(CustomerDbContext dbcontext) : GrpcService2.Protos.Customer.CustomerBase
    {


        public override async Task<CreateCustomerReply> Create(CreateCustomerRequest request,
            ServerCallContext context)
        {
            var id = context.RequestHeaders.Single(e => e.Key == "id-bin");

            var userData = UserData.Parser.ParseFrom(id.ValueBytes);

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Name is required"));
            }

            await dbcontext.Customers.AddAsync(GrpcService2.Data.Domain.Customer.Create(
                request.Name,
                request.Age
            ), context.CancellationToken);

            await dbcontext.SaveChangesAsync(context.CancellationToken);

            return Task.FromResult(new CreateCustomerReply
            {
                Message = "Customer created successfully"
            }).Result;
        }

        public override async Task GetCustomer(CreateCustomerRequest request,
            IServerStreamWriter<CreateCustomerReply> responseStream,
            ServerCallContext context)
        {
            /*await foreach*/
            for (int i = 0; i < 100; i++) // getall users
            {
                if (context.CancellationToken.IsCancellationRequested)
                    break;

                await responseStream.WriteAsync(new CreateCustomerReply
                {
                    Message = $"Customer {i + 1}",
                    CreatedAt = Timestamp.FromDateTime(DateTime.UtcNow)
                });

                await Task.Delay(1000); // simulate some delay
            }
        }
    }
}