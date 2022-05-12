using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using NLog;

namespace Webhallen
{
    public class LoggingHandler : DelegatingHandler
    {
        private readonly bool _logResponseContent;
        private readonly Logger _logger;

        public LoggingHandler(
            Logger logger,
            bool logResponseContent,
            HttpMessageHandler handler)
            : base(handler)
        {
            _logResponseContent = logResponseContent;
            _logger = logger;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            if (request.Content is not null)
            {
                string requestContent = await request.Content.ReadAsStringAsync(cancellationToken);
                _logger.Debug(requestContent);
            }

            HttpResponseMessage? response = await base.SendAsync(request, cancellationToken);

            if (_logResponseContent && response?.Content is not null)
            {
                string responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
                _logger.Debug(responseContent);
            }

            return response;
        }
    }
}
