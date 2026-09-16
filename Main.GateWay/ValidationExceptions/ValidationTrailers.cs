using Grpc.Core;
using System.Text;
using static Grpc.Core.Metadata;

namespace Main.GateWay.ValidationExceptions;

public class ValidationTrailers
{
    public string? PropertyName { get; set; }

    public string? ErrorMessage { get; set; }

    public string? AttemptedValue { get; set; }
}

public static class ValidationTrailersExtensions
{
    public static List<ValidationTrailers> GetValidationTrailers(this RpcException rpcException)
     => rpcException.Trailers.FirstOrDefault(x => x.Key == "validation-errors-text")
        ?.GetFromBase64()
        ?.GetValidationTrailers() ?? [];

    private static string? GetFromBase64(this Entry entry)
    => Encoding.UTF8.GetString(Convert.FromBase64String(entry.Value));

    private static List<ValidationTrailers>? GetValidationTrailers(this string json)
     => System.Text.Json.JsonSerializer.Deserialize<List<ValidationTrailers>>(json);


    public static ServiceProblemDetails? GetProblemDetails(this RpcException rpcException)
    => rpcException.Trailers.FirstOrDefault(x => x.Key == "problem-details-bin")
       ?.GetFromBytes()
       ?.GetJsonFromBytes();

    private static string? GetFromBytes(this Entry entry)
   => Encoding.UTF8.GetString(entry.ValueBytes);

    private static ServiceProblemDetails? GetJsonFromBytes(this string json)
     => System.Text.Json.JsonSerializer.Deserialize<ServiceProblemDetails>(json);

}

public class ServiceProblemDetails
{
    public required string Title { get; init; }
    public required string Type { get; init; }
    public string? Detail { get; init; }
    public string? Instance { get; init; }
    public IDictionary<string, string?> Extensions { get; init; } = new Dictionary<string, string?>(StringComparer.Ordinal);
}