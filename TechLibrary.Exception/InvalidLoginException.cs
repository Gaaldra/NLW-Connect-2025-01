using System.Net;

namespace TechLibrary.Exception;

public class InvalidLoginException : TechLibraryException
{
    public override List<string> GetErrorMessages() => new List<string> { "Email e/ou senha inválidos" };
    public override HttpStatusCode GetStatusCode() => HttpStatusCode.Unauthorized;
}
