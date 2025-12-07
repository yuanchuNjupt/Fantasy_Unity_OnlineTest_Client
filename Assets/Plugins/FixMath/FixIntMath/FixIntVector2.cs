using System;
using System.Collections.Generic;
using System.Text;

namespace FixMath
{
    public struct FixIntVector2 
    {
        #region Property
        private static readonly FixIntVector2 zeroVector = new FixIntVector2(0, 0);

        private static readonly FixIntVector2 oneVector = new FixIntVector2(1, 1);

        private static readonly FixIntVector2 upVector = new FixIntVector2(0, 1);

        private static readonly FixIntVector2 downVector = new FixIntVector2(0, -1);

        private static readonly FixIntVector2 leftVector = new FixIntVector2(-1, 0);

        private static readonly FixIntVector2 rightVector = new FixIntVector2(1, 0);

        /// <summary>
        ///   <para>Returns the length of this vector (Read Only).</para>
        /// </summary>
        public FixInt magnitude => FixIntMath.Sqrt(x * x + y * y);
        /// <summary>
        ///   <para>Returns the squared length of this vector (Read Only).</para>
        /// </summary>
        public FixInt sqrMagnitude => x * x + y * y;
        /// <summary>
        ///   <para>Shorthand for writing Vector2(0, 0).</para>
        /// </summary>
        public static FixIntVector2 zero => zeroVector;

        /// <summary>
        ///   <para>Shorthand for writing Vector2(1, 1).</para>
        /// </summary>
        public static FixIntVector2 one => oneVector;

        /// <summary>
        ///   <para>Shorthand for writing Vector2(0, 1).</para>
        /// </summary>
        public static FixIntVector2 up => upVector;

        /// <summary>
        ///   <para>Shorthand for writing Vector2(0, -1).</para>
        /// </summary>
        public static FixIntVector2 down => downVector;

        /// <summary>
        ///   <para>Shorthand for writing Vector2(-1, 0).</para>
        /// </summary>
        public static FixIntVector2 left => leftVector;

        /// <summary>
        ///   <para>Shorthand for writing Vector2(1, 0).</para>
        /// </summary>
        public static FixIntVector2 right => rightVector;

        /// <summary>
        ///   <para>Returns this vector with a magnitude of 1 (Read Only).</para>
        /// </summary>
        public FixIntVector2 normalized
        {
            get
            {
                FixIntVector2 result = new FixIntVector2(x, y);
                result.Normalize();
                return result;
            }
        }

        public FixInt x;

        public FixInt y;

        #endregion

        #region Constructor
        public FixIntVector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
        public FixIntVector2(FixInt x, FixInt y)
        {
            this.x = x;
            this.y = y;
        }
        #endregion

        #region Public Method
        public void Normalize()
        {
            FixInt n = x * x + y * y;
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
        }
        /// <summary>
        ///   <para>Linearly interpolates between vectors a and b by t.</para>
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="t"></param>
        public static FixIntVector2 Lerp(FixIntVector2 a, FixIntVector2 b, float t)
        {
            FixInt time = new FixInt((long)(t * 1024));
            return new FixIntVector2(a.x + (b.x - a.x) * time, a.y + (b.y - a.y) * time);
        }

        /// <summary>
        ///   <para>Returns the distance between a and b.</para>
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public static FixInt Distance(FixIntVector2 a, FixIntVector2 b)
        {
            FixInt num = a.x - b.x;
            FixInt num2 = a.y - b.y;
            return (FixInt)FixIntMath.Sqrt(num * num + num2 * num2);
        }

        /// <summary>
        ///   <para>Returns a copy of vector with its magnitude clamped to maxLength.</para>
        /// </summary>
        /// <param name = "vector" ></ param >
        /// < param name="maxLength"></param>
        public static FixIntVector2 ClampMagnitude(FixIntVector2 vector, int maxLength)
        {
            FixInt sqrMagnitude = vector.sqrMagnitude;
            if (sqrMagnitude > maxLength * maxLength)
            {
                FixInt num = FixIntMath.Sqrt(sqrMagnitude);
                FixInt num2 = vector.x / num;
                FixInt num3 = vector.y / num;
                return new FixIntVector2(num2 * maxLength, num3 * maxLength);
            }
            return vector;
        }

        public static FixInt SqrMagnitude(FixIntVector2 a)
        {
            return a.x * a.x + a.y * a.y;
        }

        public FixInt SqrMagnitude()
        {
            return x * x + y * y;
        }

        /// <summary>
        /// 两个向量点乘得到一个标量 ，数值等于两个向量长度相乘后再乘以二者夹角的余弦值 。如果两个向量a,b均 为单位(Normal) 向量 ,那么a.b等于向量b在向量a方向上的投影的长度。
        /// </summary>
        /// <param name="f1"></param>
        /// <param name="f2"></param>
        /// <returns>若结果==0 则两向量互垂直，若结果<0 则两向量夹角>90度 ，若结果>0 则两向量夹角<90度。</returns>
        public static FixInt Dot(FixIntVector2 f1, FixIntVector2 f2)
        {
            return f1.x * f2.x + f1.y * f2.y;
        }
        /// <summary>
        ///   <para>Returns a vector that is made from the smallest components of two vectors.</para>
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        public static FixIntVector2 Min(FixIntVector2 lhs, FixIntVector2 rhs)
        {
            return new FixIntVector2(FixIntMath.Min(lhs.x, rhs.x), FixIntMath.Min(lhs.y, rhs.y));
        }

        /// <summary>
        ///   <para>Returns a vector that is made from the largest components of two vectors.</para>
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        public static FixIntVector2 Max(FixIntVector2 lhs, FixIntVector2 rhs)
        {
            return new FixIntVector2(FixIntMath.Max(lhs.x, rhs.x), FixIntMath.Max(lhs.y, rhs.y));
        }

        public override int GetHashCode()
        {
            return x.GetHashCode() + y.GetHashCode();
        }
        public override string ToString()
        {
            return string.Format("({0},{1})", x, y);
        }
        public string ToStringFloat()
        {
            return string.Format("x:{0} y:{1}", x.RawFloat, y.RawFloat);
        }
        #endregion

        #region Operator
        public static FixIntVector2 operator +(FixIntVector2 a,FixIntVector2 b)
        {
            return new FixIntVector2(a.x+b.x,a.y+b.y);
        }
        public static FixIntVector2 operator -(FixIntVector2 a, FixIntVector2 b)
        {
            return new FixIntVector2(a.x - b.x, a.y - b.y);
        }
        public static FixIntVector2 operator *(FixIntVector2 a, FixInt b)
        {
            return new FixIntVector2(a.x * b, a.y * b);
        }
        public static FixIntVector2 operator /(FixIntVector2 a, FixInt b)
        {
            return new FixIntVector2(a.x / b, a.y / b);
        }
        public static FixIntVector2 operator *(FixInt a, FixIntVector2 b)
        {
            return new FixIntVector2(a * b.x, a * b.y);
        }
        public static FixIntVector2 operator /(FixInt a, FixIntVector2 b)
        {
            return new FixIntVector2(a / b.x, a / b.y);
        }
        public static bool operator ==(FixIntVector2 a, FixIntVector2 b)
        {
            return a.x == b.x && a.y == b.y;
        }
        public static bool operator !=(FixIntVector2 a, FixIntVector2 b)
        {
            return a.x != b.x || a.y != b.y;
        }
        public override bool Equals(object obj)
        {
            return obj is FixIntVector2 && ((FixIntVector2)obj) == this;
        }
        #endregion

    }

}
