using System.Diagnostics;
using Application.Interfaces;
using MediatR;

namespace Application.Common.Behaviours;

public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull {
    private readonly ICoralogixLogger<TRequest> _logger;

    public LoggingBehaviour(ICoralogixLogger<TRequest> logger) {
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    ) {
        var requestName = typeof(TRequest).Name;
        string uniqueId = Guid.NewGuid().ToString();

        _logger.Info($"Begin Request: Id:{uniqueId}, request name: {requestName}");
        var timer = new Stopwatch();
        timer.Start();

        var response = await next();
        timer.Stop();
        _logger.Info(
            $"Begin Request Id:{uniqueId}, request name:{requestName}, total request time:{timer.ElapsedMilliseconds}");
        return response;
    }
}