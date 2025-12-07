using System;
using System.Collections.Generic;
using System.Text;

namespace FixMath
{
    public struct FixIntVector3 
    {
        #region Property
        private static readonly FixIntVector3 zeroVector = new FixIntVector3(0, 0, 0);

        private static readonly FixIntVector3 oneVector = new FixIntVector3(1, 1, 1);

        private static readonly FixIntVector3 upVector = new FixIntVector3(0, 1, 0);

        private static readonly FixIntVector3 downVector = new FixIntVector3(0, -1, 0);

        private static readonly FixIntVector3 leftVector = new FixIntVector3(-1, 0, 0);

        private static readonly FixIntVector3 rightVector = new FixIntVector3(1, 0, 0);

        private static readonly FixIntVector3 forwardVector = new FixIntVector3(0, 0f, 1f);

        private static readonly FixIntVector3 backVector = new FixIntVector3(0f, 0f, -1f);

        /// <summary>
        ///   <para>Returns the length of this vector (Read Only).</para>
        /// </summary>
        public FixInt magnitude => FixIntMath.Sqrt(x * x + y * y+z*z);
        /// <summary>
        ///   <para>Returns the squared length of this vector (Read Only).</para>
        /// </summary>
        public FixInt sqrMagnitude => x * x + y * y+z*z;
        /// <summary>
        ///   <para>Shorthand for writing Vector2(0, 0).</para>
        /// </summary>
        public  static FixIntVector3 zero => zeroVector;

        /// <summary>
        ///   <para>Shorthand for writing Vector2(1, 1).</para>
        /// </summary>
        public static FixIntVector3 one => oneVector;
        
        /// <summary>
        ///   <para>Shorthand for writing Vector2(0, 1).</para>
        /// </summary>
        public static FixIntVector3 up => upVector;

        /// <summary>
        ///   <para>Shorthand for writing Vector2(0, -1).</para>
        /// </summary>
        public static FixIntVector3 down => downVector;

        /// <summary>
        ///   <para>Shorthand for writing Vector2(-1, 0).</para>
        /// </summary>
        public static FixIntVector3 left => leftVector;

        /// <summary>
        ///   <para>Shorthand for writing Vector2(1, 0).</para>
        /// </summary>
        public static FixIntVector3 right => rightVector;

        /// <summary>
        ///   <para>Shorthand for writing Vector3(0, 0, 1).</para>
        /// </summary>
        public static FixIntVector3 forward => forwardVector;

        /// <summary>
        ///   <para>Shorthand for writing Vector3(0, 0, -1).</para>
        /// </summary>
        public static FixIntVector3 back => backVector;

        /// <summary>
        ///   <para>Returns this vector with a magnitude of 1 (Read Only).</para>
        /// </summary>
        public FixIntVector3 normalized
        {
            get
            {
                FixIntVector3 result = new FixIntVector3(x, y , z);
                result.Normalize();
                return result;
            }
        }

        public FixInt x;

        public FixInt y;

        public FixInt z;

        #endregion

        #region Constructor
        public FixIntVector3(float x, float y,float z)
        {
            this.x = new FixInt(x);
            this.y = new FixInt(y);
            this.z = new FixInt(z);
        }
        public FixIntVector3(FixInt x, FixInt y, FixInt z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public FixIntVector3(UnityEngine.Vector3 v)
        {
            this.x = v.x;
            this.y = v.y;
            this.z = v.z;
        }
        #endregion

        #region Public Method
        public void Normalize()
        {
            FixInt n = x * x + y * y+z*z;
            if (n == FixInt.Zero)
                return;

            n = FixIntMath.Sqrt(n);

            if (n < (FixInt)0.0001)
            {
                return;
            }

            n = 1 / n;
            x *= n;
            y *= n;
            z *= n;
        }

 

        /// <summary>
        ///   <para>Linearly interpolates between vectors a and b by t.</para>
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="t"></param>
        public static FixIntVector3 Lerp(FixIntVector3 a, FixIntVector3 b, float t)
        {
            FixInt time = new FixInt((long)(t * 1024));
            return new FixIntVector3(a.x + (b.x - a.x) * time, a.y + (b.y - a.y) * time, a.z + (b.z - a.z) * time);
        }

        /// <summary>
        ///   <para>Returns the distance between a and b.</para>
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public static FixInt Distance(FixIntVector3 a, FixIntVector3 b)
        {
            FixInt num = a.x - b.x;
            FixInt num2 = a.y - b.y;
            FixInt num3 = a.z - b.z;
            return (FixInt)FixIntMath.Sqrt(num * num + num2 * num2 + num3 * num3);

            //FixInt num = a.x - b.x;
            //FixInt num2 = a.y - b.y;
            //FixInt num3= a.z - b.z;
            //return FixIntMath.Sqrt(num * num + num2 * num2+ num3* num3);
        }

        /// <summary>
        ///   <para>Returns a copy of vector with its magnitude clamped to maxLength.</para>
        /// </summary>
        /// <param name = "vector" ></ param >
        /// < param name="maxLength"></param>
        public static FixIntVector3 ClampMagnitude(FixIntVector3 vector, int maxLength)
        {
            FixInt sqrMagnitude = vector.sqrMagnitude;
            if (sqrMagnitude > maxLength * maxLength)
            {
                FixInt num = FixIntMath.Sqrt(sqrMagnitude);
                FixInt num2 = vector.x / num;
                FixInt num3 = vector.y / num;
                FixInt num4 = vector.z / num;
                return new FixIntVector3(num2 * maxLength, num3 * maxLength,num4*maxLength);
            }
            return vector;
        }

        public static FixInt SqrMagnitude(FixIntVector3 a)
        {
            return a.x * a.x + a.y * a.y +a.z*a.z;
        }

        public FixInt SqrMagnitude()
        {
            return x * x + y * y+z*z;
        }

        /// <summary>
        /// 两个向量点乘得到一个标量 ，数值等于两个向量长度相乘后再乘以二者夹角的余弦值 。如果两个向量a,b均 为单位(Normal) 向量 ,那么a.b等于向量b在向量a方向上的投影的长度。
        /// </summary>
        /// <param name="f1"></param>
        /// <param name="f2"></param>
        /// <returns>若结果==0 则两向量互垂直，若结果<0 则两向量夹角>90度 ，若结果>0 则两向量夹角<90度。</returns>
        public static FixInt Dot(FixIntVector3 f1, FixIntVector3 f2)
        {
            return f1.x * f2.x + f1.y * f2.y+f1.z*f2.z;
        }

        /// <summary>
        ///   <para>Cross Product of two vectors.</para>
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        public static FixIntVector3 Cross(FixIntVector3 lhs, FixIntVector3 rhs)
        {
            return new FixIntVector3(lhs.y * rhs.z - lhs.z * rhs.y, lhs.z * rhs.x - lhs.x * rhs.z, lhs.x * rhs.y - lhs.y * rhs.x);
        }
        /// <summary>
        ///   <para>Returns a vector that is made from the smallest components of two vectors.</para>
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        public static FixIntVector3 Min(FixIntVector3 lhs, FixIntVector3 rhs)
        {
            return new FixIntVector3(FixIntMath.Min(lhs.x, rhs.x), FixIntMath.Min(lhs.y, rhs.y), FixIntMath.Min(lhs.z, rhs.z));
        }

        /// <summary>
        ///   <para>Returns a vector that is made from the largest components of two vectors.</para>
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        public static FixIntVector3 Max(FixIntVector3 lhs, FixIntVector3 rhs)
        {
            return new FixIntVector3(FixIntMath.Max(lhs.x, rhs.x), FixIntMath.Max(lhs.y, rhs.y), FixIntMath.Max(lhs.z, rhs.z));
        }

        public override int GetHashCode()
        {
            return x.GetHashCode() + y.GetHashCode();
        }
        public override string ToString()
        {
            return string.Format("({0},{1},{2})", x, y,z);
        }
        public UnityEngine.Vector3 ToVector3()
        {
            return new  UnityEngine.Vector3(x.RawFloat,y.RawFloat,z.RawFloat);
        }
        public string ToStringFloat()
        {
            return string.Format("({0},{1},{2})", x.RawFloat, y.RawFloat, z.RawFloat);
        }
        #endregion

        #region Operator
        public static FixIntVector3 operator +(FixIntVector3 a,FixIntVector3 b)
        {
            return new FixIntVector3(a.x+b.x,a.y+b.y,a.z+b.z);
        }
        public static FixIntVector3 operator -(FixIntVector3 a, FixIntVector3 b)
        {
            return new FixIntVector3(a.x - b.x, a.y - b.y,a.z-b.z);
        }
        public static FixIntVector3 operator *(FixIntVector3 a, FixInt b)
        {
            return new FixIntVector3(a.x * b, a.y * b,a.z*b);
        }
        public static FixIntVector3 operator /(FixIntVector3 a, FixInt b)
        {
            return new FixIntVector3(a.x / b, a.y / b,a.z/b);
        }
        public static FixIntVector3 operator *(FixInt a, FixIntVector3 b)
        {
            return new FixIntVector3(a * b.x, a * b.y,a*b.z);
        }
        public static FixIntVector3 operator /(FixInt a, FixIntVector3 b)
        {
            return new FixIntVector3(a / b.x, a / b.y,a/b.z);
        }
        public static bool operator ==(FixIntVector3 a, FixIntVector3 b)
        {
            return a.x == b.x && a.y == b.y&&a.z==b.z;
        }
       
        public static bool operator !=(FixIntVector3 a, FixIntVector3 b)
        {
            return a.x != b.x || a.y != b.y||a.z!=b.z;
        }
       
        public override bool Equals(object obj)
        {
            return obj is FixIntVector2 && ((FixIntVector3)obj) == this;
        }
        #endregion

    }

}
