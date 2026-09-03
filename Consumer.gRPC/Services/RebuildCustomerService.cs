using Grpc.Core;
using GrpcService2.Protos;

namespace Consumer.gRPC.Services
{
    public class RebuildCustomerService(Customer.CustomerClient customer)
        : RebuildCustomer.RebuildCustomerBase
    {
        public override Task<HelloReply> CreateCustomer(HelloRequest request, ServerCallContext context)
        {
            var res = customer.Create(
                new CreateCustomerRequest { Name = request.Name }
                );

            return Task.FromResult(new HelloReply { Message = res.Message + " from consumer ..." });
        }
    }
}
