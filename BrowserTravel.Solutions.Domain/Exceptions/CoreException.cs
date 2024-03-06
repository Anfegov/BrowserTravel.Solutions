using System.Runtime.Serialization;

namespace BrowserTravel.Solutions.Domain.Exceptions;

[Serializable]
public class CoreException : Exception
{
    public CoreException()
    {

    }
    public CoreException(string message) : base(message)
    {
    }

    public CoreException(string message, Exception inner) : base(message, inner)
    {
    }

    protected CoreException(SerializationInfo info, StreamingContext context) 
    : base(info, context) 
    {        
    }

}