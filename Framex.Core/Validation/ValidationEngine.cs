using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Framex.Core
{
    public class ValidationEngine : IValidationEngine
    {
        public async Task<FramexError[]> ValidateAsync(List<IValidator> validators)
        {
            if ((validators?.Count ?? 0) == 0) { return null; }

            FramexError[][] errors = await Task.WhenAll(validators.Select(validator => validator.ValidateAsync()));
            return (errors?.Any() ?? false) && errors[0] != null ? errors.SelectMany(error => error).ToArray() : null;
        }
    }
}
