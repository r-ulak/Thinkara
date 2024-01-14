using Elmah;
using System;
using System.Web;

public static class ExceptionLogging
{
    /// <summary>
    /// Log error to Elmah
    /// </summary>
    public static void LogError(Exception ex, string contextualMessage = null)
    {
        if (HttpContext.Current == null)
        {
            var annotatedException = new Exception(contextualMessage, ex);
            ErrorLog.GetDefault(null).Log(new Error(annotatedException));
            return;
        }
        else
        {
            ErrorLog.GetDefault(HttpContext.Current).Log(new Elmah.Error(ex));
        }
        try
        {
            // log error to Elmah
            if (contextualMessage != null)
            {
                // log exception with contextual information that's visible when 
                // clicking on the error in the Elmah log
                var annotatedException = new Exception(contextualMessage, ex);
                ErrorSignal.FromCurrentContext().Raise(annotatedException, HttpContext.Current);
            }
            else
            {
                ErrorSignal.FromCurrentContext().Raise(ex, HttpContext.Current);
            }


        }
        catch (Exception error)
        {
            ErrorLog.GetDefault(null).Log(new Error(error));
        }
    }
}