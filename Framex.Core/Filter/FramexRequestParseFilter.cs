using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Framex.Core
{
    public class FramexRequestParseFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ActionArguments?.Count > 0)
            {
                var arguments = new KeyValuePair<string, object>[context.ActionArguments.Count];
                context.ActionArguments.CopyTo(arguments, 0);
                var executionContext = context.HttpContext.RequestServices.GetService(typeof(IProcessorExecutionContext)) as IProcessorExecutionContext;
                //arguments.Where(kvp => (kvp.Value is IFramexRequest))?.ToList().ForEach(kvp =>
                //executionContext.SetItem(kvp.Key.ToLower(), kvp.Value));
                foreach (KeyValuePair<string, object> kvp in arguments)
                {
                    executionContext.SetItem(kvp.Key.ToLower(), kvp.Value);
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            return;
        }
    }
}
