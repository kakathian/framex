using System.Threading.Tasks;

namespace Framex.Core
{
    /// <summary>
    /// Template for processors. Any domain levels processors etend from this to leverage centralized processing,
    /// exception handling and logging.
    /// <see cref="IProcessorExecutionContext">The request item key, action argument and request type names
    /// must all match for the request item to be properly read from processor execution context</see>
    /// </summary>
    /// <typeparam name="TRequest">Request type of the request. Use <see cref="IEmptyRequestType"/>
    /// if request is not applicable. Routines using this request must match their argument
    /// names with this type name</typeparam>
    /// <typeparam name="TResponse">Response item type. Use <see cref="IEmptyResponseType"/> if 
    /// response is not applicable</typeparam>
    public interface IProcessor<TRequest, TResponse>
    {
        /// <summary>
        /// Processes the processor with the given request. Parameterized <see cref="ExecuteAsync(TRequest)"/> is used
        /// to execute with a given request TRequest>
        /// </summary>
        /// <param name="request">The request to be executed</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task ProcessAsync();

        /// <summary>
        /// Name of the processor
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Result of the processor is saved to this property after execution.
        /// Returns null if the response type is <see cref="IEmptyResponseType"/>
        /// </summary>
        TResponse Response { get; }
    }
}
