using Framex.Core;
using Framex.Core.Anamoly;
using Framex.Platform.Models;
using Framex.Platform.Validators;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Framex.Platform.Processor
{
    /// <summary>
    /// Thsi is sample processor built using the framex execution framework
    /// </summary>
    public class ValuesProcessor : BaseProcessor<GetValueRequest, string>
    {
        private IEnumerable<IValidator> _validators;
        private string _response;
        public override string Name => nameof(ValuesProcessor);

        public ValuesProcessor(
            ILogger<ValuesProcessor> logger,
            IServiceProvider services,
            IValidationEngine validationEngine,
            IProcessorExecutionContext executionContext,
            IEnumerable<IValidator> validators)
            : base(logger, services, validationEngine, executionContext)
        {
            this._validators = validators;
        }

        /// <summary>
        /// Parse the request parameters available from the execution context
        /// into the input type of type TRequest in <see cref="IProcessor{TRequest, TResult}"/>
        /// </summary>
        /// <returns></returns>
        protected override GetValueRequest ParseRequest()
        {
            return new GetValueRequest()
            {
                Id = Convert.ToInt32(this.ProcessorContext.GetItem("id")),
                Name = Convert.ToString(this.ProcessorContext.GetItem("name"))
            };
        }

        /// <summary>
        /// Return any built validators that validates the request.
        /// <see cref="ValueRequestValidator"/> and <seealso cref="IValidator"/>
        /// </summary>
        /// <returns></returns>
        protected override List<IValidator> GetValidators()
        {
            return new List<IValidator>() { { new ValueRequestValidator(this.Request) } };
        }

        /// <summary>
        /// Build the code that requires to be occamplished before the
        /// actual execution <see cref="BaseProcessor.ProcessCoreAsync"/>.
        /// For example, check if a student exists in a university in the
        /// database (or StudentRepository class); or build any
        /// requests objects of other domains that might be used in later steps. 
        /// May be by <see cref="BaseProcessor.ProcessCoreAsync"/>
        /// </summary>
        /// <returns></returns>
        protected async override Task PreProcessAsync()
        {
            await Task.FromResult(true).ConfigureAwait(false);
        }

        /// <summary>
        /// Build the code that does the actual execution.
        /// For example, fetch the enrolled courses by the student what
        /// this processor actually meant to be doing. Here I just named the processor as
        /// ValuesProcessor for simplicity
        /// </summary>
        /// <returns></returns>
        protected async override Task ProcessCoreAsync()
        {
            _response = await Task.FromResult("value").ConfigureAwait(false);
        }

        /// <summary>
        /// Build the code that parses any intermitten results obtained
        /// in <see cref="BaseProcessor.ProcessCoreAsync"/> 
        /// to the expected result output type of <see cref="IProcessor.Result"/>
        /// </summary>
        /// <returns></returns>
        protected async override Task<string> PostProcessAsync()
        {
            return await Task.FromResult(_response).ConfigureAwait(false);
        }

        /// <summary>
        /// This is the last routine called by the Framex framework if
        /// all steps in the execution are succeded. If any step failed,
        /// <see cref="BaseProcessor{TRequest, TResponse}.OnProcessFailedAsync"/>
        /// is called
        /// </summary>
        /// <returns></returns>
        protected override Task OnProcessCompletedAsync()
        {
            return base.OnProcessCompletedAsync();
        }

        /// <summary>
        /// If any process step failed, framex calls
        /// <see cref="BaseProcessor{TRequest, TResponse}.OnProcessFailedAsync"/>.
        /// For example, build any code that sends alert emails if any operation fails
        /// in the above steps.
        /// <param name="processException"></param>
        /// <returns></returns>
        /// </summary>
        protected override Task OnProcessFailedAsync(ProcessorException processException)
        {
            return base.OnProcessFailedAsync(processException);
        }
    }
}
