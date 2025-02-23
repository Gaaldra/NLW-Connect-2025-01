﻿using System.Net;

namespace TechLibrary.Exception;

public class ErrorOnValidationException(List<string> errorMessages) : TechLibraryException
{
    private readonly List<string> _errors = errorMessages;

    public override List<string> GetErrorMessages() => _errors;

    public override HttpStatusCode GetStatusCode() => HttpStatusCode.BadRequest;
}
