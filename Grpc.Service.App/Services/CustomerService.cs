using Grpc.Core;
using GrpcService2.Data;
using GrpcService2.Protos;

namespace GrpcService2.Services
{
    public class CustomerService(CustomerDbContext dbcontext) : GrpcService2.Protos.Customer.CustomerBase
    {


        public override async Task<CreateCustomerReply> Create(CreateCustomerRequest request, ServerCallContext context)
        {
            await dbcontext.Customers.AddAsync(GrpcService2.Data.Domain.Customer.Create(
                request.Name,
                request.Age
            ));

            await dbcontext.SaveChangesAsync();

            return Task.FromResult(new CreateCustomerReply
            {
                Message = "Customer created successfully"
            }).Result;
        }
    }
}