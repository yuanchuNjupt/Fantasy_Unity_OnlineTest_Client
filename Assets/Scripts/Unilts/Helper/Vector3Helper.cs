using Fantasy;
using UnityEngine;

namespace Helper
{
    public static class Vector3Helper
    {
        public static Vector3 ToVector3(this CSVector3 self)
        {
            return new Vector3()
            {
                x = self.x,
                y = self.y,
                z = self.z
            };
        }
        
        public static CSVector3 ToCSVector3(this Vector3 self)
        {
            return new CSVector3()
            {
                x = self.x,
                y = self.y,
                z = self.z
            };
        }
    }
}