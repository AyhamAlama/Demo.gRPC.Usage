using Grpc.Core;
using Grpc.Core.Interceptors;

namespace Consumer.gRPC.Interceptors;

public class ExceptionHandlingInterceptor : Interceptor
{
    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            return await continuation(request, context);
        }
        catch (Exception ex)
        {
            throw ex switch
            {
                RpcException rpcEx => new RpcException(rpcEx.Status, rpcEx.Message),
                _ => new RpcException(new Status(StatusCode.Internal, "An unexpected error occurred."), ex.Message),
            };
        }
    }
}