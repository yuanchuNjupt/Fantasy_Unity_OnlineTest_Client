using System;

namespace FixMath
{
    //strut 值类型数据结构 值类型的实例在栈上分配内存 ：一般单纯的用作储存各种数据类型的相关的结构        (栈内存：由系统自行分配、释放。速度较快。一般存储临时\小型数据)
    //class 引用类型  引用类型的实例在堆上分配内存 ：         一般用来处理游戏各种逻辑、数据，用处比较广       (堆内存：Unity中又称托管堆。堆中内存由程序通过New分配内存，由程序释放内存,若程序不去释放，则有系统通过GC去释放，速度较慢，但使用方便。一般储存中大型数据)

    public struct FixInt : IEquatable<FixInt>, IComparable<FixInt>
    {
        public const long MinValue = -9223372036854775808L;
        public const long MaxValue = 9223372036854775807L;

        private const int SHIFT = 10; //位移运算相较于乘除运算速度更快、效率更高。 这里使用扩大1024倍数的方式是因为1024扩大之后的数值计算中更加准确，而1000倍则在数值过大或者精度较高的情况下会损失精度，造成误差。
        private const int MUTIPLE = 1024;
        public static readonly FixInt One = new FixInt(1);
        public static readonly FixInt Zero = new FixInt(0);

        private readonly long value;
        public float RawFloat { get { return (float)Math.Round(value / 1024.0f * 100)  / 100; } }//精度为2位的小数
        public  long Value { get { return value; } }

        public int IntValue { get { return (int)value; } }
        public int RawInt { get { return (int)Math.Round((double)(value >> SHIFT)); } }
        public long RawLong { get { return value >> SHIFT; } }
        //public long RawLong { get { return value /MUTIPLE; } }


        public double RawDouble { get { return value / 1024.0d; } }
        public float SinCosFloat { get { return value*1.0f / 10000.0f; } }

        public FixInt(float value)
        {
            this.value = (long)Math.Round((value * MUTIPLE));
        }
        public FixInt(double value)
        {
            this.value = (long)Math.Round((value * MUTIPLE));
        }
        public FixInt(int value)
        {
            this.value = value << SHIFT;
        }
        public FixInt(long value)
        {
            this.value = value;
        }


        //强制转换

        //implicit(隐式类型转换运算符)  转换目标类型一般是自定义类型 如 int float double 

        public static implicit operator FixInt(float v)
        {
            return new FixInt(v);
        }
        public static implicit operator FixInt(double v)
        {
            return new FixInt(v);
        }
        public static implicit operator FixInt(int v)
        {
            return new FixInt(v);
        }
        public static implicit operator FixInt(long v)
        {
            return new FixInt(v);
        }

        //explicit(显示类型转换运算符)  转换目标类型一般是自定义类型 如 Fxint
        public static explicit operator float(FixInt v)
        {
            return v.RawFloat;
        }
        public static explicit operator double(FixInt v)
        {
            return v.RawFloat;
        }
        //public static explicit operator int(FixInt v)
        //{
        //    return v.RawInt;
        //}
        //public static explicit operator long(FixInt v)
        //{
        //    return v.RawLong;
        //}

        //隐式转换与显示转换的区别就是  显示转换需要加(int) (long) 等类似的转换符号，而隐式转换则可以直接通过=进行赋值。转换的过程被隐藏了起来，故称之为隐式转换。


        //operator 重载预定义C#运算符

        //  + - * /  += -= >= <= >> <<
        public static FixInt operator +(FixInt f1, FixInt f2)
        {
            return new FixInt(f1.value + f2.value);
        }
        public static FixInt operator +(int f1, FixInt f2)
        {
            return (FixInt)f1 + f2;
        }
        public static FixInt operator +(FixInt f1, int f2)
        {
            return f1 + (FixInt)f2;
        }
        public static FixInt operator +(float f1, FixInt f2)
        {
            return (FixInt)f1 + f2;
        }
        public static FixInt operator +(FixInt f1, float f2)
        {
            return f1 + (FixInt)f2;
        }
        public static FixInt operator +(double f1, FixInt f2)
        {
            return (FixInt)f1 + f2;
        }
        public static FixInt operator +(FixInt f1, double f2)
        {
            return f1 + (FixInt)f2;
        }
        public static FixInt operator +(long f1, FixInt f2)
        {
            return (FixInt)f1 + f2;
        }
        public static FixInt operator +(FixInt f1, long f2)
        {
            return f1 + (FixInt)f2;
        }


        public static FixInt operator -(FixInt f1, FixInt f2)
        {
            return new FixInt(f1.value - f2.value);
        }
        public static FixInt operator -(int f1, FixInt f2)
        {
            return (FixInt)f1 - f2;
        }
        public static FixInt operator -(FixInt f1, int f2)
        {
            return f1 - (FixInt)f2;
        }
        public static FixInt operator -(float f1, FixInt f2)
        {
            return (FixInt)f1 - f2;
        }
        public static FixInt operator -(FixInt f1, float f2)
        {
            return f1 - (FixInt)f2;
        }
        public static FixInt operator -(double f1, FixInt f2)
        {
            return (FixInt)f1 - f2;
        }
        public static FixInt operator -(FixInt f1, double f2)
        {
            return f1 - (FixInt)f2;
        }
        public static FixInt operator -(long f1, FixInt f2)
        {
            return (FixInt)f1 - f2;
        }
        public static FixInt operator -(FixInt f1, long f2)
        {
            return f1 - (FixInt)f2;
        }

        public static FixInt operator *(FixInt f1, FixInt f2)
        {
            //return new FixInt((f1.value * f2.value) >> SHIFT);
            return new FixInt((f1.value * f2.value) /MUTIPLE);
        }
        public static FixInt operator *(int f1, FixInt f2)
        {
            return (FixInt)f1 * f2;
        }
        public static FixInt operator *(FixInt f1, int f2)
        {
            return f1 * (FixInt)f2;
        }
        public static FixInt operator *(float f1, FixInt f2)
        {
            return (FixInt)f1 * f2;
        }
        public static FixInt operator *(FixInt f1, float f2)
        {
            return f1 * (FixInt)f2;
        }
        public static FixInt operator *(double f1, FixInt f2)
        {
            return (FixInt)f1 * f2;
        }
        public static FixInt operator *(FixInt f1, double f2)
        {
            return f1 * (FixInt)f2;
        }
        public static FixInt operator *(long f1, FixInt f2)
        {
            return (FixInt)f1 * f2;
        }
        public static FixInt operator *(FixInt f1, long f2)
        {
            return f1 * (FixInt)f2;
        }

        public static FixInt operator /(FixInt f1, FixInt f2)
        {
           // Console.WriteLine("(f1.value << SHIFT) / f2.value:" + (f1.value << SHIFT) / f2.value);
            FixInt a = (f1.value << SHIFT);
            FixInt b = a.value / f2.value ;  
            return b;//两个long类型相除结果是 long类型的Fixint 因long类型FixInt构造函数不会进行移位操作，故分母进行移位操作后在除
        }
        public static FixInt operator /(int f1, FixInt f2)
        {
            return (FixInt)f1 / f2;
        }
        public static FixInt operator /(FixInt f1, int f2)
        {
            return f1 / (FixInt)f2;
        }
        public static FixInt operator /(float f1, FixInt f2)
        {
            return (FixInt)f1 / f2;
        }
        public static FixInt operator /(FixInt f1, float f2)
        {
            return f1 / (FixInt)f2;
        }
        public static FixInt operator /(double f1, FixInt f2)
        {
            return (FixInt)f1 / f2;
        }
        public static FixInt operator /(FixInt f1, double f2)
        {
            return f1 / (FixInt)f2;
        }
        public static FixInt operator /(long f1, FixInt f2)
        {
            return (FixInt)f1 / f2;
        }
        public static FixInt operator /(FixInt f1, long f2)
        {
            return f1 / (FixInt)f2;
        }

        public static bool operator >(FixInt f1, FixInt f2)
        {
            return f1.value > f2.value;
        }
        public static bool operator >(FixInt f1, int f2)
        {
            return f1 > (FixInt)f2;
        }
        public static bool operator >(int f1, FixInt f2)
        {
            return (FixInt)f1 > f2;
        }
        public static bool operator >(FixInt f1, float f2)
        {
            return f1 > (FixInt)f2;
        }
        public static bool operator >(float f1, FixInt f2)
        {
            return (FixInt)f1 > f2;
        }
        public static bool operator >(FixInt f1, double f2)
        {
            return f1 > (FixInt)f2;
        }
        public static bool operator >(double f1, FixInt f2)
        {
            return (FixInt)f1 > f2;
        }
        public static bool operator >(FixInt f1, long f2)
        {
            return f1 > (FixInt)f2;
        }
        public static bool operator >(long f1, FixInt f2)
        {
            return (FixInt)f1 > f2;
        }

        public static bool operator <(FixInt f1, FixInt f2)
        {
            return f1.value < f2.value;
        }
        public static bool operator <(FixInt f1, int f2)
        {
            return f1 < (FixInt)f2;
        }
        public static bool operator <(int f1, FixInt f2)
        {
            return (FixInt)f1 < f2;
        }
        public static bool operator <(FixInt f1, float f2)
        {
            return f1 < (FixInt)f2;
        }
        public static bool operator <(float f1, FixInt f2)
        {
            return (FixInt)f1 < f2;
        }
        public static bool operator <(FixInt f1, double f2)
        {
            return f1 < (FixInt)f2;
        }
        public static bool operator <(double f1, FixInt f2)
        {
            return (FixInt)f1 < f2;
        }
        public static bool operator <(FixInt f1, long f2)
        {
            return f1 < (FixInt)f2;
        }
        public static bool operator <(long f1, FixInt f2)
        {
            return (FixInt)f1 < f2;
        }

        public static bool operator >=(FixInt f1, FixInt f2)
        {
            return f1.value >= f2.value;
        }
        public static bool operator >=(FixInt f1, int f2)
        {
            return f1 >= (FixInt)f2;
        }
        public static bool operator >=(int f1, FixInt f2)
        {
            return (FixInt)f1 >= f2;
        }
        public static bool operator >=(FixInt f1, float f2)
        {
            return f1 >= (FixInt)f2;
        }
        public static bool operator >=(float f1, FixInt f2)
        {
            return (FixInt)f1 >= f2;
        }
        public static bool operator >=(FixInt f1, double f2)
        {
            return f1 >= (FixInt)f2;
        }
        public static bool operator >=(double f1, FixInt f2)
        {
            return (FixInt)f1 >= f2;
        }
        public static bool operator >=(FixInt f1, long f2)
        {
            return f1 >= (FixInt)f2;
        }
        public static bool operator >=(long f1, FixInt f2)
        {
            return (FixInt)f1 >= f2;
        }

        public static bool operator <=(FixInt f1, FixInt f2)
        {
            return f1.value <= f2.value;
        }
        public static bool operator <=(FixInt f1, int f2)
        {
            return f1 <= (FixInt)f2;
        }
        public static bool operator <=(int f1, FixInt f2)
        {
            return (FixInt)f1 <= f2;
        }
        public static bool operator <=(FixInt f1, float f2)
        {
            return f1 <= (FixInt)f2;
        }
        public static bool operator <=(float f1, FixInt f2)
        {
            return (FixInt)f1 <= f2;
        }
        public static bool operator <=(FixInt f1, double f2)
        {
            return f1 <= (FixInt)f2;
        }
        public static bool operator <=(double f1, FixInt f2)
        {
            return (FixInt)f1 <= f2;
        }
        public static bool operator <=(FixInt f1, long f2)
        {
            return f1 <= (FixInt)f2;
        }
        public static bool operator <=(long f1, FixInt f2)
        {
            return (FixInt)f1 <= f2;
        }

        public static bool operator !=(FixInt f1, FixInt f2)
        {
            return f1.value != f2.value;
        }
        public static bool operator !=(FixInt f1, int f2)
        {
            return f1 != (FixInt)f2;
        }
        public static bool operator !=(int f1, FixInt f2)
        {
            return (FixInt)f1 != f2;
        }
        public static bool operator !=(FixInt f1, float f2)
        {
            return f1 != (FixInt)f2;
        }
        public static bool operator !=(float f1, FixInt f2)
        {
            return (FixInt)f1 != f2;
        }
        public static bool operator !=(FixInt f1, double f2)
        {
            return f1 != (FixInt)f2;
        }
        public static bool operator !=(double f1, FixInt f2)
        {
            return (FixInt)f1 != f2;
        }
        public static bool operator !=(FixInt f1, long f2)
        {
            return f1 != (FixInt)f2;
        }
        public static bool operator !=(long f1, FixInt f2)
        {
            return (FixInt)f1 != f2;
        }

        public static bool operator ==(FixInt f1, FixInt f2)
        {
            return f1.value == f2.value;
        }
        public static bool operator ==(FixInt f1, int f2)
        {
            return f1 != (FixInt)f2;
        }
        public static bool operator ==(int f1, FixInt f2)
        {
            return (FixInt)f1 == f2;
        }
        public static bool operator ==(FixInt f1, float f2)
        {
            return f1 == (FixInt)f2;
        }
        public static bool operator ==(float f1, FixInt f2)
        {
            return (FixInt)f1 == f2;
        }
        public static bool operator ==(FixInt f1, double f2)
        {
            return f1 == (FixInt)f2;
        }
        public static bool operator ==(double f1, FixInt f2)
        {
            return (FixInt)f1 == f2;
        }
        public static bool operator ==(FixInt f1, long f2)
        {
            return f1 == (FixInt)f2;
        }
        public static bool operator ==(long f1, FixInt f2)
        {
            return (FixInt)f1 == f2;
        }

        public static FixInt operator >>(FixInt f1, int count)
        {
            return new FixInt(f1.value >> count);
        }
        public static FixInt operator <<(FixInt f1, int count)
        {
            return new FixInt(f1.value << count);
        }

        public static FixInt operator %(FixInt f1, FixInt f2)
        {
            return new FixInt(f1.value % f2.value);
        }
        public static FixInt operator -(FixInt f1)
        {
            return new FixInt(-f1.value);
        }



        public bool Equals(FixInt other)
        {
            return value == other.value;
        }
        public override bool Equals(object obj)
        {
            return value == ((FixInt)obj).value;
        }
        public override int GetHashCode()
        {
            return value.GetHashCode();
        }
        public override string ToString()
        {
            return RawFloat.ToString();
        }
        public string ToStringFloat()
        {
            return RawFloat.ToString("F2");
        }

        /// <summary>
        /// 将当前实例与另一个对象进行比较，该整数表示当前实例的值是大于另一个实例的值还是小于另一个实例的值。
        /// </summary>
        /// <param name="other"></param>
        /// <returns> 小于0 则当前实例小于value  等于0 则当前实例与value相等 大于0则当前实例大于value</returns>
        public int CompareTo(FixInt other)
        {
            return value.CompareTo(other.Value);
        }
    }
}
