using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Linq;
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class,
AllowMultiple = false, Inherited = true)]
public sealed class ApiValidateAntiForgeryTokenAttribute :
FilterAttribute, IAuthorizationFilter
{
    public Task<HttpResponseMessage> ExecuteAuthorizationFilterAsync(
    HttpActionContext actionContext,
    CancellationToken cancellationToken,
    Func<Task<HttpResponseMessage>> continuation)
    {
        Task<HttpResponseMessage> ret = null;

        try
        {
            // Validating
            ValidateToken(actionContext.Request);

            // All good, continuing
            ret = continuation();
        }
        catch (System.Web.Mvc.HttpAntiForgeryException)
        {
            // Initiating a 403 "Forbidden" response
            actionContext.Response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Forbidden,
                RequestMessage = actionContext.ControllerContext.Request
            };

            var source = new TaskCompletionSource<HttpResponseMessage>();
            source.SetResult(actionContext.Response);

            ret = source.Task;
        }

        return ret;
    }


    private void ValidateToken(HttpRequestMessage request)
    {
        string[] tokens = null;
        IEnumerable<string> headers = null;

        if (request.Headers.TryGetValues("RequestVerificationToken", out headers))
        {
            tokens = headers.First().Split(':');
            AntiForgery.Validate(tokens[0], tokens[1]);
        }
        else
            AntiForgery.Validate();
    }
}