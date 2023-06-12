using System.Diagnostics;
using Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Common.Behaviours;

public class PerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse> {
    private readonly Stopwatch _timer;
    private readonly ICoralogixLogger<TRequest> _logger;

    public PerformanceBehaviour(
        ICoralogixLogger<TRequest> logger
    ) {
        _logger = logger;
        _timer = new Stopwatch();
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken) {
        _timer.Start();
        var response = await next();
        _timer.Stop();

        var elapsedMilliseconds = _timer.ElapsedMilliseconds;

        if (elapsedMilliseconds > 500) {
            var requestName = typeof(TRequest).Name;
            _logger.Warning(
                string.Format("Too Long Running Request: Name {0} ElapsedMilliseconds:({1} milliseconds) Request: {2}",
                    requestName, elapsedMilliseconds, request)
            );
        }

        return response;
    }
}