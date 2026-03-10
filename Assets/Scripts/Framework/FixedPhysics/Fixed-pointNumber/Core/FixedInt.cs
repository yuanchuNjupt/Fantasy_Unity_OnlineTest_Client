using System;
using System.Globalization;
using FixedPhysics.Fixed_pointNumber.Interfaces;

namespace FixedPhysics.Fixed_pointNumber.Core
{
    public readonly struct FixedInt : IFixedInt , IEquatable<FixedInt> , IComparable<FixedInt>
    {
        
        public const Int64 MaxValue = 9223372036854775807;
        public const Int64 MinValue = -9223372036854775808;

        public const Single NegativeInfinity = -1F / 0F;
        public const Single PositiveInfinity = 1F / 0F;
        public const Single Epsilon = 1F / 0F;

        // public const FixedInt One = 1;
        // public const  FixedInt Zero = new FixedInt(0);
        
        
        
        public long Magnification { get; }
        
        /// <summary>
        /// 渲染数据，精度为两位，用于渲染显示，不能用于计算
        /// </summary>
        public float RenderFloat => (float)Math.Round(Magnification / 1024.0f * 100) / 100;
        
        public int RenderInt => (int)(Magnification >> Shift);

        /// <summary>
        /// 移位次数
        /// </summary>
        public const int Shift = 10;

        /// <summary>
        /// 放大倍率
        /// </summary>
        public const int Multiple = 1 << Shift;

        #region 构造函数

        public FixedInt(float value)
        {
            this.Magnification = (long)Math.Round((value) * Multiple);
        }
        
        public FixedInt(double value)
        {
            this.Magnification = (long)Math.Round((value) * Multiple);
            
        }

        public FixedInt(int value)
        {
            this.Magnification = value << Shift;

        }

        public FixedInt(long value)
        {
            this.Magnification = value << Shift;
        }

        private FixedInt(long value , bool isDirectly)
        {
            this.Magnification = value;
        }
        
        
        /// <summary>
        /// 从放大后的值构造FixedInt实例
        /// </summary>
        /// <param name="magnification">放大后的结果</param>
        /// <returns></returns>
        public static FixedInt ConstructFromMagnification(long magnification)
        {
            return new FixedInt(magnification , true);
        }

        #endregion

        #region 类型转换

        public static implicit operator FixedInt(float value)
        {
            return new FixedInt(value);
        }
        
        public static implicit operator FixedInt(double value)
        {
            return new FixedInt(value);
        }
        
        public static implicit operator FixedInt(int value)
        {
            return new FixedInt(value);
        }
        
        public static implicit operator FixedInt(long value)
        {
            return new FixedInt(value);
        }

        
        
        public static explicit operator float(FixedInt value)
        {
            return value.RenderFloat;
        }
        
        public static explicit operator int(FixedInt value)
        {
            return value.RenderInt;
        }
        
        public static explicit operator double(FixedInt value)
        {
            return value.RenderFloat;
        }
        
        public static explicit operator long(FixedInt value)
        {
            return value.Magnification;
        }

        #endregion

        #region 运算符重载

        // ======================== 加法运算符 ========================
        
        public static FixedInt operator +(FixedInt value1 , FixedInt value2)
        {
            return new FixedInt(value1.Magnification + value2.Magnification, true);
        }
        
        public static FixedInt operator +(FixedInt value1 , int value2)
        {
            return value1 + (FixedInt)value2;
        }
        
        public static FixedInt operator +(FixedInt value1 , float value2)
        {
            return value1 + (FixedInt)value2;
        }
        
        public static FixedInt operator +(FixedInt value1 , double value2)
        {
            return value1 + (FixedInt)value2;
        }
        
        // ======================== 减法运算符 ========================
        
        public static FixedInt operator -(FixedInt value1 , FixedInt value2)
        {
            return new FixedInt(value1.Magnification - value2.Magnification, true);
        }
        
        public static FixedInt operator -(FixedInt value1 , int value2)
        {
            return value1 - (FixedInt)value2;
        }
        
        public static FixedInt operator -(FixedInt value1 , float value2)
        {
            return value1 - (FixedInt)value2;
        }
        
        public static FixedInt operator -(FixedInt value1 , double value2)
        {
            return value1 - (FixedInt)value2;
        }
        
        // ======================== 乘法运算符 ========================
        
        public static FixedInt operator *(FixedInt value1 , FixedInt value2)
        {
            return new FixedInt((value1.Magnification * value2.Magnification) >> Shift, true);
        }
        
        public static FixedInt operator *(FixedInt value1 , int value2)
        {
            return value1 * (FixedInt)value2;
        }
        
        public static FixedInt operator *(FixedInt value1 , float value2)
        {
            return value1 * (FixedInt)value2;
        }
        
        public static FixedInt operator *(FixedInt value1 , double value2)
        {
            return value1 * (FixedInt)value2;
        }
        
        // ======================== 除法运算符 ========================
        
        public static FixedInt operator /(FixedInt value1 , FixedInt value2)
        {
            return new FixedInt((value1.Magnification << Shift) / value2.Magnification, true);
        }
        
        public static FixedInt operator /(FixedInt value1 , int value2)
        {
            return value1 / (FixedInt)value2;
        }
        
        public static FixedInt operator /(FixedInt value1 , float value2)
        {
            return value1 / (FixedInt)value2;
        }
        
        public static FixedInt operator /(FixedInt value1 , double value2)
        {
            return value1 / (FixedInt)value2;
        }
        
        // ======================== 比较运算符 ========================
        
        public static bool operator ==(FixedInt value1, FixedInt value2)
        {
            return value1.Magnification == value2.Magnification;
        }
        
        public static bool operator !=(FixedInt value1, FixedInt value2)
        {
            return value1.Magnification != value2.Magnification;
        }
        
        public static bool operator >(FixedInt value1, FixedInt value2)
        {
            return value1.Magnification > value2.Magnification;
        }
        
        public static bool operator <(FixedInt value1, FixedInt value2)
        {
            return value1.Magnification < value2.Magnification;
        }
        
        public static bool operator >=(FixedInt value1, FixedInt value2)
        {
            return value1.Magnification >= value2.Magnification;
        }
        
        public static bool operator <=(FixedInt value1, FixedInt value2)
        {
            return value1.Magnification <= value2.Magnification;
        }
        
        // ======================== 取模运算符 ========================
        
        public static FixedInt operator %(FixedInt value1, FixedInt value2)
        {
            return new FixedInt(value1.Magnification % value2.Magnification, true);
        }
        
        public static FixedInt operator %(FixedInt value1, int value2)
        {
            return value1 % (FixedInt)value2;
        }
        
        public static FixedInt operator %(FixedInt value1, float value2)
        {
            return value1 % (FixedInt)value2;
        }
        
        public static FixedInt operator %(FixedInt value1, double value2)
        {
            return value1 % (FixedInt)value2;
        }
        
        // ======================== 一元运算符 ========================
        
        /// <summary>
        /// 取反运算符（负号）
        /// </summary>
        public static FixedInt operator -(FixedInt value)
        {
            return new FixedInt(-value.Magnification, true);
        }
        
        // ======================== 位移运算符 ========================
        
        /// <summary>
        /// 左移运算符（相当于乘以 2^n）
        /// </summary>
        public static FixedInt operator <<(FixedInt value, int shift)
        {
            return new FixedInt(value.Magnification << shift, true);
        }
        
        /// <summary>
        /// 右移运算符（相当于除以 2^n）
        /// </summary>
        public static FixedInt operator >>(FixedInt value, int shift)
        {
            return new FixedInt(value.Magnification >> shift, true);
        }

        #endregion
        
        
        
        
        

        public override string ToString()
        {
            return RenderFloat.ToString(CultureInfo.InvariantCulture);
        }

        public bool Equals(FixedInt other)
        {
            return this.Magnification == other.Magnification;
        }

        public int CompareTo(FixedInt other)
        {
            return this.Magnification.CompareTo(other.Magnification);
        }

        public override bool Equals(object obj)
        {
            if (obj is FixedInt fixedInt)
            {
                return this.Magnification == fixedInt.Magnification;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Magnification.GetHashCode();
        }

    }
}
