

using System;

namespace Framework.GameManager.Core
{
    public class WorldSourceAttribute : Attribute
    {
        public readonly Type WorldType;
    
        public WorldSourceAttribute(Type worldType) 
        {
            this.WorldType = worldType;
        }
    }
}
