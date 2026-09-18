using Main.GateWay.Protos;
using Microsoft.AspNetCore.Mvc;
using Polly.Registry;

namespace Main.GateWay.Controllers;

[ApiController]
[Route("[controller]")]
public class CustomersController : ControllerBase
{

    private readonly Customer.CustomerClient grpcClient;
    private readonly ResiliencePipelineProvider<string> _pipelineProvider;

    public CustomersController(Customer.CustomerClient grpcClient, ResiliencePipelineProvider<string> pipelineProvider)
    {
        this.grpcClient = grpcClient;
        _pipelineProvider = pipelineProvider;
    }

    // Resilience 
    // [HttpPost] [HttpPut] [HttpDelete]
    public async Task<IActionResult> CreateCustomer()
    {
        var pipeline = _pipelineProvider.GetPipeline("def");

        var reply = await pipeline.ExecuteAsync(
            async ct =>
            {
                return await grpcClient.CreateAsync(new CreateCustomerRequest
                {
                    Age = 30
                }, cancellationToken: ct);
            }
            );

        return Ok(reply.Message);
    }

}