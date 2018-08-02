using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Framex.Core.Anamoly;
using Microsoft.Extensions.Logging;

namespace Framex.Core
{
    /// <summary>
    /// Template for processors. Any domain levels processors extend from this to leverage centralized processing,
    /// exception handling and logging.
    /// <see cref="IProcessorExecutionContext">The request item key, action argument and request type names
    /// must all match for the request item to be properly read from processor execution context</see>
    /// </summary>
    /// <typeparam name="TRequest">Request type of the request. Use <see cref="IEmptyRequestType"/>
    /// if no request is required. Routines using this request must match their argument
    /// names with this type name</typeparam>
    /// <typeparam name="TResponse">Response item type. Use <see cref="IEmptyresponseType"/> </typeparam>
    public abstract class BaseProcessor<TRequest, TResponse> : IProcessor<TRequest, TResponse>
    {
        public TResponse Response { get; private set; }
        public abstract string Name { get; }
        public IServiceProvider ServiceProvider { get; }
        protected IProcessorExecutionContext ProcessorContext { get; }
        protected ILogger<BaseProcessor<TRequest, TResponse>> Logger { get; private set; }
        protected TRequest Request { get; private set; }

        protected IValidationEngine ValidationEngine { get; }

        protected BaseProcessor(
            ILogger<BaseProcessor<TRequest, TResponse>> logger,
            IServiceProvider serviceProvider,
            IValidationEngine validationEngine,
            IProcessorExecutionContext executionContext)
        {
            this.Logger = logger;
            this.ValidationEngine = validationEngine;
            this.ServiceProvider = serviceProvider;
            this.ProcessorContext = executionContext;
        }

        /// <summary>
        /// Executes the processor with the given request. Parameterized <see cref="ProcessAsync(TRequest)"/> is used
        /// to execute with a given request TRequest>
        /// </summary>
        /// <param name="request">The request to be executed</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task ProcessAsync()
        {
            ProcessorException processorException = null;

            try
            {
                this.SetRequest();

                await this.ValidateAsync();

                await this.PreProcessAsync();

                await this.ProcessCoreAsync();

                this.Response = await this.PostProcessAsync();

                await this.OnProcessCompletedAsync();
            }
            catch (Exception exception)
            {
                this.Logger.LogError(exception, this.Name);
                processorException = new ProcessorException("Request processesing failed", exception);
            }
            finally
            {
                if (null != processorException)
                {
                    await this.OnProcessFailedAsync(processorException);
                }
            }
        }

        /// <summary>
        /// Request is set from the processor execution context.
        /// The request item key, action argument and type name must match
        /// </summary>
        protected void SetRequest()
        {
            if (!(typeof(TRequest) is IEmptyRequestType))
            {
                this.Request = ParseRequest();
            }
        }

        protected virtual TRequest ParseRequest()
        {
            return (TRequest)this.ProcessorContext.GetItem(typeof(TRequest).Name);
        }

        /// <summary>
        /// Any DTO validations are implemented here. Validators are executed based on FIFO
        /// </summary>
        /// <returns>Validators which are used to validate the DTOs as dictated by the validation rules</returns>
        protected virtual List<IValidator> GetValidators() => new List<IValidator>();

        /// <summary>
        /// Any DTO validations are implemented here
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected async Task<bool> ValidateAsync()
        {
            FramexError[] errors = await this.ValidationEngine.ValidateAsync(this.GetValidators());
            if (errors?.Length > 0)
            {
                throw new ValidationException($"Validation failed for: '{this.Name}'", errors);
            }

            return true;
        }

        /// <summary>
        /// Any DTO to domain / model mapping or parsing is implemented here
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected abstract Task PreProcessAsync();

        /// <summary>
        /// The actual execution of (CRUD operations, if any) the domain / models are done here
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected abstract Task ProcessCoreAsync();

        /// <summary>
        /// Responses (if any exists) are built here. Operations like mapping models / domains to DTOs are done here
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected virtual Task<TResponse> PostProcessAsync() => Task.FromResult(default(TResponse));

        /// <summary>
        /// Any notifications or tasks that need to be executed post execution are accomplished here
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected virtual async Task OnProcessCompletedAsync() => await Task.FromResult(true);

        
        /// <summary>
        /// Any notifications or tasks that need to be executed when execution is failed
        /// </summary>
        /// <param name="executionException"></param>
        ///// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected virtual async Task OnProcessFailedAsync(ProcessorException executionException) => await Task.FromResult(true);
    }
}