

using System;

public class WorldSourceAttribute : Attribute
{
    public readonly Type WorldType;
    
    public WorldSourceAttribute(Type worldType)
    {
        this.WorldType = worldType;
    }
}
