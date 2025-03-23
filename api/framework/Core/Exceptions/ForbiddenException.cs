using System.Net;

namespace AMIS.Framework.Core.Exceptions;
public class ForbiddenException : FshException
{
    public ForbiddenException()
        : base("unauthorized", [], HttpStatusCode.Forbidden)
    {
    }
    public ForbiddenException(string message)
       : base(message, [], HttpStatusCode.Forbidden)
    {
    }
}
