using Main.GateWay.Protos;
using Microsoft.AspNetCore.Mvc;

namespace Main.GateWay.Controllers;

[ApiController]
[Route("[controller]")]
public class CustomersController(Customer.CustomerClient grpcClient) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateCustomer()
    {
        var reply = await grpcClient.CreateAsync(new CreateCustomerRequest
        {
            Age = 30
        });

        return Ok(reply.Message);
    }
}