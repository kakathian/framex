using System.Threading.Tasks;

namespace Framex.Core
{
    public interface IValidator
    {
        Task<FramexError[]> ValidateAsync();
    }
}
