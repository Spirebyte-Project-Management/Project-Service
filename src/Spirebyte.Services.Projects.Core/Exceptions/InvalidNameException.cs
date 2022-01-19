﻿using Spirebyte.Services.Projects.Core.Exceptions.Base;

namespace Spirebyte.Services.Projects.Core.Exceptions;

public class InvalidNameException : DomainException
{
    public InvalidNameException(string name) : base($"Invalid name: {name}.")
    {
    }

    public override string Code { get; } = "invalid_name";
}