using System.Diagnostics;
using AMIS.Framework.Core.Paging;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AMIS.Framework.Infrastructure.Behaviours;

public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger _logger = logger;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var sw = Stopwatch.StartNew();

        try
        {
            if (request is PaginationFilter pf)
            {
                _logger.LogInformation(
                    "Handling {RequestName}: PageNumber={PageNumber}, PageSize={PageSize}, HasKeyword={HasKeyword}, HasAdvancedSearch={HasAdvancedSearch}, HasAdvancedFilter={HasAdvancedFilter}",
                    requestName,
                    pf.PageNumber,
                    pf.PageSize,
                    !string.IsNullOrWhiteSpace(pf.Keyword),
                    pf.AdvancedSearch is not null,
                    pf.AdvancedFilter is not null);
            }
            else
            {
                _logger.LogInformation("Handling {RequestName}", requestName);
            }

            var response = await next();

            sw.Stop();
            _logger.LogInformation("Handled {RequestName} in {ElapsedMs}ms", requestName, sw.ElapsedMilliseconds);

            return response;
        }
        catch (Exception ex)
        {
            sw.Stop();
            _logger.LogError(ex, "Error handling {RequestName} after {ElapsedMs}ms", requestName, sw.ElapsedMilliseconds);
            throw;
        }
    }
}
