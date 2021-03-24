using System;
using System.Collections.Generic;
using System.Text;
using Spirebyte.Services.Projects.Core.Exceptions.Base;

namespace Spirebyte.Services.Projects.Core.Exceptions
{
    public class EmptyPermissionsException : DomainException
    {
        public override string Code { get; } = "permissions_is_empty";

        public EmptyPermissionsException() : base($"Permissions cannot be empty.")
        {
        }
    }
}
