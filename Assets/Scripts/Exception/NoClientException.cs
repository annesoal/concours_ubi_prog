using System;

public class NoClientException : Exception
{
    public NoClientException()
    {
    }

    public NoClientException(string message)
        : base(message)
    {
        
    }
}
