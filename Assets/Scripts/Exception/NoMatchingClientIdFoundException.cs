using System;

/**
 * Thrown when no matching client id, in all the connected client ids available, is found.
 */
public class NoMatchingClientIdFoundException : Exception
{
    public NoMatchingClientIdFoundException()
    {
    }

    public NoMatchingClientIdFoundException(string message)
        : base(message)
    {
        
    }
}
