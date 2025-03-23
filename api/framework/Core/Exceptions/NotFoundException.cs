using System.Collections.ObjectModel;
using System.Net;

namespace AMIS.Framework.Core.Exceptions;
public class NotFoundException : FshException
{
    public NotFoundException(string message)
        : base(message, new Collection<string>(), HttpStatusCode.NotFound)
    {
    }
}
