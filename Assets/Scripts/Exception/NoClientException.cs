using System;

/**
 * Thrown when no client is connected. (only the host)
 */
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
