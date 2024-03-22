using System;

public class NoMatchingBuildingMaterialSOException : Exception
{
    public NoMatchingBuildingMaterialSOException()
    {}
    
    public NoMatchingBuildingMaterialSOException(string message)
        : base(message)
    {}
}
