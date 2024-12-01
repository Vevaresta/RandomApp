//using NLog;

//namespace RandomApp.Server.Api.Middleware
//{
//    public class LoggingHttpMiddleware
//    {
//        private readonly RequestDelegate _next;
//        private readonly ISessionInfoProvider _sessionInfoProvider;
//        private readonly NLog.Logger _logger;

//        public LoggingHttpMiddleware(RequestDelegate next, ISessionInfoProvider sessionInfoProvider)
//        {
//            _next = next;
//            _sessionInfoProvider = sessionInfoProvider;
//            _logger = LogManager.GetCurrentClassLogger();
//        }

//        public async Task Invoke(HttpContext context)
//        {
//            var uriBuilder = new UriBuilder
//            {
//                Scheme = context.Request.Scheme,
//                Host = context.Request.Host.ToString(),
//                Path = context.Request.Path
//            };

//            var sessionInfo = await _sessionInfoProvider.GetSessionInfo();
//            var sessionOwner = sessionInfo.SessionOwner;

//            _logger.Info($"Endpoint is called by => user: '{sessionOwner.User}' station: '{sessionOwner.Station}' url: '{uriBuilder}'");

//            await _next(context);
//        }
//    }
//}
