# framex
Template execution framework for asp.net core applications.

Framex was written in C# using .NET Core 2.0. This is targetted for to be used by asp.net core applications.

The example project `https://github.com/kakathian/framex/tree/master/Framex.Platform` shows how to consume framex.core framework.

https://github.com/kakathian/framex/blob/master/Framex.Platform/Processors/ValuesProcessor.cs shows how a processor is built using framex.core framework.

Framex is inspired by template design pattern.
https://github.com/kakathian/framex/blob/master/Framex.Core/BaseProcessor.cs class has `ProcessAsync` which actually
is called by the MVC core controller classes (check Framex.Platform/Commands/ValuesProcessor sample implemented class) 

Life cycle of ProcessAsync routine:

`SetRequest`	: 	Calls `ParseRequest` internally. If there is only one argument for the
					controller action whose type is the BaseProcessor.TRequest type, then the base routine `SetRequest` by default parses
					and sets the `Request` property.
				
`ParseRequest`	: 	Request object for the processor is built in `ParseRequest`. Controller's action arguments are accessed from the
					`IProcessorExecutionContext` by it's argument name as key.
				
`ValidateAsync`	:	Any DTO validations are implemented here. This calls `GetValidators` to collect the validators.
					Internally uses ValidationEngine which validates the request as defined by list of IValidator fed by `GetValidators`
					
`GetValidators`	:	Any DTO validators are built here. Validators are executed based on FIFO (first in first out strategy). The actual
					validation is done by the ValidationEngine internally.
					
`PreProcessAsync`	:	Any DTO to domain / model mapping or parsing is implemented here

`ProcessCoreAsync`	:	The actual execution of (CRUD operations, if any) the domain / models are done here

`PostProcessAsync`	:	Processor result (if any exists) is built here. Operations like mapping models / domains to DTOs are done here.
						The returned result by this routine is set to `Result` property by `ProcessAsync`
						
`OnProcessCompletedAsync`	:	Any notifications or tasks that need to be executed post execution are accomplished here.
								This is called only if all the above steps are succeded with no exceptions or errors.
								
`OnProcessFailedAsync`	:	This is called when any of the above execution step fails. Any exception thrown, is collected by this routine and set ar its argument 
							
Any exception araised, is available as an argument `processorException` to `OnProcessFailedAsync`.

Sample usage:

```
    [Route("api/[controller]")]
    public class SampleController : Controller
    {
        private readonly SampleProcessor _sampleProcessor;
		
        public SampleController(SampleProcessor processor)
        {
            this._sampleProcessor = processor;
        }
        
        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<string> Get(int id, string name)
        {
            // Sample processor built using framex framework
			
            await _sampleProcessor.ProcessAsync();
            return _sampleProcessor.Response;
        }
    }
```


Registering Framex Processors with MVC:

Framex works as intended only if registered in the mvc startup as shown below:

https://github.com/kakathian/framex/blob/769d2b5f77cf8d0be38e17fefd670e0bb0ebcc2a/Framex.Platform/Startup.cs#L30
and 
https://github.com/kakathian/framex/blob/769d2b5f77cf8d0be38e17fefd670e0bb0ebcc2a/Framex.Platform/Startup.cs#L33

```
public void ConfigureServices(IServiceCollection services)
    {
	    services.AddMvc(mvcOptions =>
	    {
	     //Framex: Must have this statement
		mvcOptions.Filters.Add<FramexRequestParseFilter>();
	    });

	    //Framex: Must have this statement
	    services.RegisterFramexServices();
	    
	    // Register your framex derived processors here
	    services.AddTransient<ValuesProcessor, ValuesProcessor>();
    }
```
							

							
							


				
