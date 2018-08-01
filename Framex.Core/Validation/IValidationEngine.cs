using System.Collections.Generic;
using System.Threading.Tasks;

namespace Framex.Core
{
    public interface IValidationEngine
    {
        Task<FramexError[]> ValidateAsync(List<IValidator> validators);
    }
}
