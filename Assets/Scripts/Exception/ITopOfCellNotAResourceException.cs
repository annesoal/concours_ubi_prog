using System;

public class ITopOfCellNotAResourceException : Exception
{
    public ITopOfCellNotAResourceException()
    {}
    
    public ITopOfCellNotAResourceException(string message)
        : base(message)
    {}
}
