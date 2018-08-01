using Framex.Core;
using Framex.Platform.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Framex.Platform.Validators
{
    public class ValueRequestValidator : IValidator
    {
        private readonly GetValueRequest _request;
        public ValueRequestValidator(GetValueRequest request)
        {
            this._request = request;
        }

        public async Task<FramexError[]> ValidateAsync()
        {
            if(this._request.Id <= 0)
            {
                return await Task.FromResult(new FramexError[1] { new FramexError { ErrorCode = "1000", ResourceCode = "R1000", ErrorMessage = "Invalid value id" } });
            }

            return null;
        }
    }
}
