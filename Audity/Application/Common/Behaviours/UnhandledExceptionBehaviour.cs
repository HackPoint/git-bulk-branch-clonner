using System.Diagnostics.CodeAnalysis;
using System.Text;
using Application.Interfaces;
using MediatR;

namespace Application.Common.Behaviours;

public class UnhandledExceptionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse> {
    private readonly ICoralogixLogger<TRequest> _logger;

    public UnhandledExceptionBehaviour(ICoralogixLogger<TRequest> logger) {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken) {
        try {
            return await next();
        }
        catch (Exception ex) {
            var requestName = typeof(TRequest).Name;
            string message = string.Format("Request: Unhandled Exception for Request {0} {1}, Error: {2}",
                requestName, request, ex);
            
            _logger.Error(message, "unhandled error", requestName);
            throw;
        }
    }
}
