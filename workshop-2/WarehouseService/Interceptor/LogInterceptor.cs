using Grpc.Core;

namespace WarehouseService.Interceptor;

public class LogInterceptor: Grpc.Core.Interceptors.Interceptor
{
    private readonly ILogger<LogInterceptor> _logger;

    public LogInterceptor(ILogger<LogInterceptor> logger)
    {
        _logger = logger;
    }

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        //Log Request
        _logger.LogInformation("Начало вызова метода {Method} c реквестом {request},", 
            context.Method, 
            request);
        var response = await continuation(request, context);
        _logger.LogInformation("Конец {Method} c реквестом =  {request}, респонс = {response}", 
            context.Method, 
            request,response);
        return response;
    }
}