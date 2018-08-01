using System;
using System.Collections.Generic;
using System.Text;

namespace Framex.Core
{
    public interface IProcessorExecutionContext
    {
        /// <summary>
        /// Gets the shared item of the current execution context
        /// </summary>
        /// <param name="itemKey">If the requested object is action parameter, then
        /// action parameter name must match this argument name</param>
        /// <returns>Returns the requested object, null if there is no match</returns>
        object GetItem(string itemKey);

        /// <summary>
        /// Sets the item to be shared across models of this execution context
        /// </summary>
        /// <param name="itemKey">If the requested object is action parameter, then
        /// action parameter name must match this argument name</param>
        /// <param name="value">Item to be shared</param>
        /// <returns>True if successfully saves the item, otherwise false</returns>
        bool SetItem(string itemKey, object value);
    }
}
