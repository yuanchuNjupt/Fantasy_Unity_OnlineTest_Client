using System;
using FixedPhysics.Fixed_pointNumber.Core;
using FixMath;
using UnityEngine;

namespace FixedPhysics.Fixed_pointNumber.FixedIntMath
{
    public partial class FixedIntMathf
    {
        /// <summary>
        /// 移位次数（与 FixedInt 保持一致）
        /// </summary>
        private const int Shift = 10;
        
        /// <summary>
        /// 放大倍率（与 FixedInt 保持一致）
        /// </summary>
        private const int Multiple = 1024;
        
        /// <summary>
        /// 弧度转角度系数 (180 / π ≈ 57.29578)
        /// </summary>
        public static readonly FixedInt Rad2Deg = 57.29578f;
        
        /// <summary>
        /// 绝对值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static FixedInt Abs(FixedInt value)
        {
            return value.Magnification > 0 ? value : -value;
        }
        /// <summary>
        /// 最大值
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns>返回两个指定数字中的较大值</returns>
        public static FixedInt Max(FixedInt value1, FixedInt value2)
        {
            return value1 > value2 ? value1 : value2;
        }

        /// <summary>
        /// 最小值 
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns>返回两个指定数字中的较小值</returns>
        public static FixedInt Min(FixedInt value1, FixedInt value2)
        {
            return value1 < value2 ? value1 : value2;
        }
        
        /// <summary>
        /// 随机数
        /// </summary>
        /// <param name="random">要随机的数</param>
        /// <param name="min">最小随机范围</param>
        /// <param name="max">最大随机范围</param>
        /// <returns>大于或等于 0 且小于 Int32.MaxValue 的 32 位有符号整数。</returns>
        public static FixedInt Range(System.Random random, FixedInt min, FixedInt max)
        {
            return random.Next((int)min, (int)max);
        }
        
        /// <summary>
        /// 固定value值的取值范围
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns>返回 value 固定到 min 和 max的非独占范围</returns>
        public static FixedInt Clamp(FixedInt value, FixedInt min, FixedInt max)
        {
            return value < min ? min : value > max ? max : value;
        }
        /// <summary>
        /// 四舍五入
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static FixedInt Round(FixedInt value)
        {
            return (Math.Round(value.RenderFloat));
        }
        /// <summary>
        /// 幂运算
        /// </summary>
        /// <param name="value"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static FixedInt Pow(FixedInt value, int count)
        {
            if (count == 1) return value;
            FixedInt result = 1;
            FixedInt tmp = Pow(value, count >> 1);
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
        /// <summary>
        /// 向下取整
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static FixedInt Floor(FixedInt value)
        {
            //清除小数部分
            FixedInt fx= (long)(((ulong)value.Magnification & ~0xFFFFFFFFFFFFF000) / FixedInt.Multiple * FixedInt.Multiple);
            return fx;
        }
        /// <summary>
        /// 向上取整
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static FixedInt Ceiling(FixedInt value)
        {
            var hasFractionalPart = (value.Magnification & 0x0000000000000FFF) != 0;
            //如果有小数部分，则加 1
            return hasFractionalPart ? Floor(value) + 1 : value;
        }

        /// <summary>
        /// 平方根
        /// </summary>
        /// <param name="f"></param>
        /// <param name="numberIterations"></param>
        /// <returns></returns>
        public static FixedInt Sqrt(FixedInt f, int numberIterations)
        {
            if (f.Magnification < 0)
            {
                throw new ArithmeticException("sqrt error");
            }

            if (f.Magnification == 0)
                return 0;

            FixedInt k = f + 1 >> 1;
            for (int i = 0; i < numberIterations; i++)
                k = (k + (f / k)) >> 1;

            if (k.Magnification < 0)
                throw new ArithmeticException("Overflow");
            else
                return k;
        }
        /// <summary>
        /// 平方根
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static FixedInt Sqrt(FixedInt f)
        {
            byte numberOfIterations = 8;
            if (f.Magnification > 0x64000)
                numberOfIterations = 12;
            if (f.Magnification > 0x3e8000)
                numberOfIterations = 16;
            return Sqrt(f, numberOfIterations);
        }

        /// <summary>
        /// 四象限反正切（定点数版本，全程整数运算，保证帧同步确定性）
        /// </summary>
        public static FixedInt Atan2(FixedInt fy, FixedInt fx)
        {
            int y = (int)fy.Magnification;
            int x = (int)fx.Magnification;
            int num;
            int num2;
            if (x < 0)
            {
                if (y < 0) { x = -x; y = -y; num = 1; }
                else        { x = -x;           num = -1; }
                num2 = -31416;
            }
            else
            {
                if (y < 0) { y = -y; num = -1; }
                else        {          num =  1; }
                num2 = 0;
            }
            int dIM = Atan2LookupTable.DIM;
            long num3 = (long)(dIM - 1);
            long b = (long)((x >= y) ? x : y);
            int num4 = (int)Divide((long)x * num3, b);
            int num5 = (int)Divide((long)y * num3, b);
            int num6 = Atan2LookupTable.table[num5 * dIM + num4];
            return ((num6 + num2) * num) / 10000f;
        }

        /// <summary>
        /// 四象限反正切
        /// </summary>
        /// <param name="y"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public static FixedInt Atan2(float fy, float fx)
        {
            int y = (int)(fy * FixedInt.Multiple); int x = (int)(fx * FixedInt.Multiple);
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
            return ((num6 + num2) * num)/ 10000f;
        }
        /// <summary>
        /// 反余弦函数
        /// </summary>
        /// <param name="nom"></param>
        /// <returns></returns>
        public static FixedInt Acos(FixedInt nom)
        {
            int num = (int)Divide(nom.Magnification * (long)AcosLookupTable.HALF_COUNT, FixedInt.Multiple) + AcosLookupTable.HALF_COUNT;
            num = Mathf.Clamp(num, 0, AcosLookupTable.COUNT);
            return (AcosLookupTable.table[num] / 10000f);
        }
        /// <summary>
        /// 反余弦函数
        /// </summary>
        /// <param name="nom"></param>
        /// <returns></returns>
        public static FixedInt Acos(FixedInt nom, long den)
        {
            int num = (int)Divide(nom.Magnification * (long)AcosLookupTable.HALF_COUNT, den) + AcosLookupTable.HALF_COUNT;
            num = Mathf.Clamp(num, 0, AcosLookupTable.COUNT);
            return AcosLookupTable.table[num] / 10000f;
        }
        /// <summary>
        /// 正弦
        /// </summary>
        /// <param name="nom"></param>
        /// <returns></returns>
        public static FixedInt Sin(FixedInt nom)
        {
            int index = SinCosLookupTable.getIndex(nom.Magnification, FixedInt.Multiple);
            return (SinCosLookupTable.sin_table[index] / 10000f);
        }

        /// <summary>
        /// 余弦
        /// </summary>
        /// <param name="nom"></param>
        /// <returns></returns>
        public static FixedInt Cos(FixedInt nom)
        {
            int index = SinCosLookupTable.getIndex(nom.Magnification, FixedInt.Multiple);
            return (SinCosLookupTable.cos_table[index] / 10000f);
        }

        /// <summary>
        /// 直接以角度（度）为输入的正弦函数。
        /// 用整数除法精确换算查找表索引，避免 Deg2Rad 精度损失。
        /// index = deg_magnification * COUNT / (360 * Multiple)
        /// </summary>
        public static FixedInt SinDeg(FixedInt deg)
        {
            long count = SinCosLookupTable.COUNT;
            long multiple = FixedInt.Multiple;
            long raw = deg.Magnification * count;
            int index = (int)(raw / (360L * multiple));
            index = ((index % SinCosLookupTable.COUNT) + SinCosLookupTable.COUNT) % SinCosLookupTable.COUNT;
            return SinCosLookupTable.sin_table[index] / 10000f;
        }

        /// <summary>
        /// 直接以角度（度）为输入的余弦函数。
        /// 用整数除法精确换算查找表索引，避免 Deg2Rad 精度损失。
        /// index = deg_magnification * COUNT / (360 * Multiple)
        /// </summary>
        public static FixedInt CosDeg(FixedInt deg)
        {
            long count = SinCosLookupTable.COUNT;
            long multiple = FixedInt.Multiple;
            long raw = deg.Magnification * count;
            int index = (int)(raw / (360L * multiple));
            index = ((index % SinCosLookupTable.COUNT) + SinCosLookupTable.COUNT) % SinCosLookupTable.COUNT;
            return SinCosLookupTable.cos_table[index] / 10000f;
        }
        /// <summary>
        /// 插值运算
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static long Divide(long a, long b)
        {
            long num = (long)((ulong)((a ^ b) & -9223372036854775808L) >> 63);
            long num2 = num * -2L + 1L;
            return (a + b / 2L * num2) / b;
        }
        /// <summary>
        /// 插值运算
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static int Divide(int a, int b)
        {
            int num = (int)((uint)((a ^ b) & -2147483648) >> 31);
            int num2 = num * -2 + 1;
            return (a + b / 2 * num2) / b;
        }


        public static FixedInt Sign(FixedInt f)
        {
            return f >= 0 ? 1 : -1;
        }
    }
}
