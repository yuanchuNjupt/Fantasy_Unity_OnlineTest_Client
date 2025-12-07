
namespace FixMath
{
    using System;
    using UnityEngine;

    public partial class FixIntMath
    {

        public static FixInt Abs(FixInt value)
        {
            return value.Value > 0 ? value : new FixInt(-value.Value);
        }
        public static FixInt Max(FixInt value1, FixInt value2)
        {
            return value1 > value2 ? value1 : value2;
        }
        public static FixInt Min(FixInt value1, FixInt value2)
        {
            return value1 < value2 ? value1 : value2;
        }
        public static FixInt Range(System.Random random, FixInt min, FixInt max)
        {
            return random.Next(min.IntValue, max.IntValue) / 1024f;
        }
        public static FixInt Clamp(FixInt value, FixInt min, FixInt max)
        {
            return value < min ? min : value > max ? max : value;
        }
        public static FixInt Round(FixInt value)
        {
            return new FixInt(Math.Round(value.RawFloat));
        }
        public static FixInt Pow(FixInt value, int count)
        {
            if (count == 1) return value;
            FixInt result = FixInt.Zero;
            FixInt tmp = Pow(value, count >> 1);
            if ((count & 1) != 0) //奇数    
            {
                result = value * tmp * tmp;
            }
            else
            {
                result = tmp * tmp;
            }
            return result;
        }

        public static FixInt Floor(FixInt value)
        {
            return new FixInt((long)((ulong)value.Value & 0xFFFFFFFFFFFFF000) >> 10);
        }
        public static FixInt Ceiling(FixInt value)
        {
            var hasFractionalPart = (value.Value & 0x0000000000000FFF) != 0;
            return hasFractionalPart ? Floor(value) + FixInt.One : value;
        }
        public static FixInt Sqrt(FixInt f, int numberIterations)
        {
            if (f.Value < 0)
            {
                throw new ArithmeticException("sqrt error");
            }

            if (f.Value == 0)
                return FixInt.Zero;

            FixInt k = f + FixInt.One >> 1;
            for (int i = 0; i < numberIterations; i++)
                k = (k + (f / k)) >> 1;

            if (k.Value < 0)
                throw new ArithmeticException("Overflow");
            else
                return k;
        }

        public static FixInt Sqrt(FixInt f)
        {
            byte numberOfIterations = 8;
            if (f.Value > 0x64000)
                numberOfIterations = 12;
            if (f.Value > 0x3e8000)
                numberOfIterations = 16;
            return Sqrt(f, numberOfIterations);
        }

    
        public static FixInt Atan2(int y, int x)
        {
            int num;
            int num2;
            if (x < 0)
            {
                if (y < 0)
                {
                    x = -x;
                    y = -y;
                    num = 1;
                }
                else
                {
                    x = -x;
                    num = -1;
                }
                num2 = -31416;
            }
            else
            {
                if (y < 0)
                {
                    y = -y;
                    num = -1;
                }
                else
                {
                    num = 1;
                }
                num2 = 0;
            }
            int dIM = Atan2LookupTable.DIM;
            long num3 = (long)(dIM - 1);
            long b = (long)((x >= y) ? x : y);
            int num4 = (int)Divide((long)x * num3, b);
            int num5 = (int)Divide((long)y * num3, b);
            int num6 = Atan2LookupTable.table[num5 * dIM + num4];
            return new FixInt((long)((num6 + num2) * num));
        }

        public static FixInt Acos(FixInt nom)
        {
            int num = (int)Divide(nom.RawLong * (long)AcosLookupTable.HALF_COUNT, 1) + AcosLookupTable.HALF_COUNT;
            num = Clamp(num, 0, AcosLookupTable.COUNT).RawInt;
            return new FixInt((long)AcosLookupTable.table[num]);
        }
        public static FixInt Acos(FixInt nom, long den)
        {
            int num = (int)Divide(nom.RawLong * (long)AcosLookupTable.HALF_COUNT, den) + AcosLookupTable.HALF_COUNT;
            num = Clamp(num, 0, AcosLookupTable.COUNT).RawInt;
            return new FixInt((long)AcosLookupTable.table[num]);
        }

        public static FixInt Sin(FixInt nom)
        {
            int index = SinCosLookupTable.getIndex(nom.RawLong, 1);
            return new FixInt((long)SinCosLookupTable.sin_table[index]);
        }


        public static FixInt Cos(FixInt nom)
        {
            int index = SinCosLookupTable.getIndex(nom.RawLong, 1);
            UnityEngine.Debug.Log("index:" + index + "  rawint:" + nom.RawLong + " (long)SinCosLookupTable.cos_table[index]:" + (long)SinCosLookupTable.cos_table[index] + "Result:" + (float)SinCosLookupTable.cos_table[index] / 10000);
            return new FixInt((long)SinCosLookupTable.cos_table[index]);
        }

        public static long Divide(long a, long b)
        {
            long num = (long)((ulong)((a ^ b) & -9223372036854775808L) >> 63);
            long num2 = num * -2L + 1L;
            return (a + b / 2L * num2) / b;
        }

        public static int Divide(int a, int b)
        {
            int num = (int)((uint)((a ^ b) & -2147483648) >> 31);
            int num2 = num * -2 + 1;
            return (a + b / 2 * num2) / b;
        }

        //public static VInt3 Divide(VInt3 a, long m, long b)
        //{
        //	a.x = (int)Divide((long)a.x * m, b);
        //	a.y = (int)Divide((long)a.y * m, b);
        //	a.z = (int)Divide((long)a.z * m, b);
        //	return a;
        //}

        //public static VInt2 Divide(VInt2 a, long m, long b)
        //{
        //	a.x = (int)Divide((long)a.x * m, b);
        //	a.y = (int)Divide((long)a.y * m, b);
        //	return a;
        //}

        //public static VInt3 Divide(VInt3 a, int b)
        //{
        //	a.x = Divide(a.x, b);
        //	a.y = Divide(a.y, b);
        //	a.z = Divide(a.z, b);
        //	return a;
        //}

        //public static VInt3 Divide(VInt3 a, long b)
        //{
        //	a.x = (int)Divide((long)a.x, b);
        //	a.y = (int)Divide((long)a.y, b);
        //	a.z = (int)Divide((long)a.z, b);
        //	return a;
        //}

        //public static VInt2 Divide(VInt2 a, long b)
        //{
        //	a.x = (int)Divide((long)a.x, b);
        //	a.y = (int)Divide((long)a.y, b);
        //	return a;
        //}
    }
}
