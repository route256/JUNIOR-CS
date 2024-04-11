using Grpc.Core;

namespace WarehouseService.Interceptor;

public class ErrorInterceptor: Grpc.Core.Interceptors.Interceptor
{
    private readonly ILogger<LogInterceptor> _logger;
    
    public ErrorInterceptor(ILogger<LogInterceptor> logger)
    {
        _logger = logger;
    }
    
    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            return await continuation(request, context);
        }
        catch (RpcException e)
        {
            _logger.LogError("Ошибка выполения метода {Method}. " +
                                "Ошибка = {Message}", 
                context.Method,
                e.Message);
            throw;
        }
        catch (Exception e)
        {
            _logger.LogCritical("Ошибка выполения метода {Method}. " +
                                "Ошибка = {Message}", 
                context.Method,
                e.Message);
            throw;
        }
       
    }
}