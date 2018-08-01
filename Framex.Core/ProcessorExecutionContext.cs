using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Framex.Core
{
    public class ProcessorExecutionContext : IProcessorExecutionContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProcessorExecutionContext(IHttpContextAccessor httpContextAccessor)
        {
            this._httpContextAccessor = httpContextAccessor;
        }

        public object GetItem(string itemKey)
        {
            return !string.IsNullOrWhiteSpace(itemKey) &&
                this._httpContextAccessor.HttpContext.Items.TryGetValue(itemKey.ToLower(), out object value) ?
                value : null;
        }

        public bool SetItem(string itemKey, object value)
        {
            return !string.IsNullOrWhiteSpace(itemKey) && value != null &&
                   this._httpContextAccessor.HttpContext.Items.TryAdd(itemKey.ToLower(), value);
        }
    }
}
