using FluentValidation;
using GrpcService2.Protos;

namespace Grpc.Main.Validators.Customers;

public class CreateCustomerValidator : AbstractValidator<CreateCustomerRequest>
{
    public CreateCustomerValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}