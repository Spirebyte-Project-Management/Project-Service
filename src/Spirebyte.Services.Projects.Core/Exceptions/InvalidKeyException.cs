using System;
using System.Collections.Generic;
using System.Text;
using Spirebyte.Services.Projects.Core.Exceptions.Base;

namespace Spirebyte.Services.Projects.Core.Exceptions
{
    public class InvalidKeyException : DomainException
    {
        public override string Code { get; } = "invalid_key";

        public InvalidKeyException(string key) : base($"Invalid key: {key}.")
        {
        }
    }
}
