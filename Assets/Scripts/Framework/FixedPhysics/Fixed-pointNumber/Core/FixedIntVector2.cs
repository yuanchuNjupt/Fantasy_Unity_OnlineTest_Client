using System;
using System.Globalization;
using FixedPhysics.Fixed_pointNumber.FixedIntMath;
using FixedPhysics.Fixed_pointNumber.Interfaces;
using UnityEngine;
using UnityEngine.Internal;

namespace FixedPhysics.Fixed_pointNumber.Core
{
  public struct FixedIntVector2 : IFixedIntVector2 , IEquatable<FixedIntVector2>, IFormattable
  {
    /// <summary>
    ///   <para>X component of the vector.</para>
    /// </summary>
    public FixedInt X;

    /// <summary>
    ///   <para>Y component of the vector.</para>
    /// </summary>
    public FixedInt Y;
    private static readonly FixedIntVector2 ZeroVector = new FixedIntVector2(0.0f, 0.0f);
    private static readonly FixedIntVector2 OneVector = new FixedIntVector2(1f, 1f);
    private static readonly FixedIntVector2 UpVector = new FixedIntVector2(0.0f, 1f);
    private static readonly FixedIntVector2 DownVector = new FixedIntVector2(0.0f, -1f);
    private static readonly FixedIntVector2 LeftVector = new FixedIntVector2(-1f, 0.0f);
    private static readonly FixedIntVector2 RightVector = new FixedIntVector2(1f, 0.0f);

  


    #region 构造

    public FixedIntVector2(FixedInt x, FixedInt y)
    {
      this.X = x;
      this.Y = y;
    }

    #endregion
  
  

    public FixedInt this[int index]
    {
      get
      {
        switch (index)
        {
          case 0:
            return this.X;
          case 1:
            return this.Y;
          default:
            throw new IndexOutOfRangeException("Invalid FixedIntVector2 index!");
        }
      }
      set
      {
        switch (index)
        {
          case 0:
            this.X = value;
            break;
          case 1:
            this.Y = value;
            break;
          default:
            throw new IndexOutOfRangeException("Invalid FixedIntVector2 index!");
        }
      }
    }

    /// <summary>
    ///   <para>Set x and y components of an existing FixedIntVector2.</para>
    /// </summary>
    /// <param name="newX"></param>
    /// <param name="newY"></param>
  
    public void Set(FixedInt newX, FixedInt newY)
    {
      this.X = newX;
      this.Y = newY;
    }

    /// <summary>
    ///   <para>Linearly interpolates between vectors a and b by t.</para>
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="t"></param>
  
    public static FixedIntVector2 Lerp(FixedIntVector2 a, FixedIntVector2 b, FixedInt t)
    {
      t = FixedIntMathf.Clamp(t , 0 , 1);
      return new FixedIntVector2(a.X + (b.X - a.X) * t, a.Y + (b.Y - a.Y) * t);
    }

    /// <summary>
    ///   <para>Linearly interpolates between vectors a and b by t.</para>
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="t"></param>
  
    public static FixedIntVector2 LerpUnclamped(FixedIntVector2 a, FixedIntVector2 b, FixedInt t)
    {
      return new FixedIntVector2(a.X + (b.X - a.X) * t, a.Y + (b.Y - a.Y) * t);
    }

    /// <summary>
    ///   <para>Moves a point current towards target.</para>
    /// </summary>
    /// <param name="current"></param>
    /// <param name="target"></param>
    /// <param name="maxDistanceDelta"></param>
  
    public static FixedIntVector2 MoveTowards(FixedIntVector2 current, FixedIntVector2 target, FixedInt maxDistanceDelta)
    {
      FixedInt num1 = target.X - current.X;
      FixedInt num2 = target.Y - current.Y;
      FixedInt d = ( num1 *  num1 +  num2 *  num2);
      if ( d == 0.0 ||  maxDistanceDelta >= 0.0 &&  d <=  maxDistanceDelta *  maxDistanceDelta)
        return target;
      FixedInt num3 = FixedIntMathf.Sqrt( d);
      return new FixedIntVector2(current.X + num1 / num3 * maxDistanceDelta, current.Y + num2 / num3 * maxDistanceDelta);
    }

    /// <summary>
    ///   <para>Multiplies two vectors component-wise.</para>
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
  
    public static FixedIntVector2 Scale(FixedIntVector2 a, FixedIntVector2 b) => new FixedIntVector2(a.X * b.X, a.Y * b.Y);

    /// <summary>
    ///   <para>Multiplies every component of this vector by the same component of scale.</para>
    /// </summary>
    /// <param name="scale"></param>
  
    public void Scale(FixedIntVector2 scale)
    {
      this.X *= scale.X;
      this.Y *= scale.Y;
    }

    /// <summary>
    ///   <para>Makes this vector have a magnitude of 1.</para>
    /// </summary>
  
    public void Normalize()
    {
      FixedInt magnitude = this.Magnitude;
      if ( magnitude > 9.999999747378752E-06)
        this = this / magnitude;
      else
        this = FixedIntVector2.zero;
    }

    /// <summary>
    ///   <para>Returns a normalized vector based on the current vector. The normalized vector has a magnitude of 1 and is in the same direction as the current vector. Returns a zero vector If the current vector is too small to be normalized.</para>
    /// </summary>
    public FixedIntVector2 normalized
    {
      get
      {
        FixedIntVector2 normalized = new FixedIntVector2(this.X, this.Y);
        normalized.Normalize();
        return normalized;
      }
    }

    /// <summary>
    ///   <para>Returns a formatted string for this vector.</para>
    /// </summary>
  
    public override string ToString() => this.ToString((string) null, (IFormatProvider) null);

    /// <summary>
    ///   <para>Returns a formatted string for this vector.</para>
    /// </summary>
    /// <param name="format">A numeric format string.</param>
  
    public string ToString(string format) => this.ToString(format, (IFormatProvider) null);

    /// <summary>
    ///   <para>Returns a formatted string for this vector.</para>
    /// </summary>
    /// <param name="format">A numeric format string.</param>
    /// <param name="formatProvider">An object that specifies culture-specific formatting.</param>
  
    public string ToString(string format, IFormatProvider formatProvider)
    {
      if (string.IsNullOrEmpty(format))
        format = "F2";
      if (formatProvider == null)
        formatProvider = CultureInfo.InvariantCulture.NumberFormat;
      return string.Format(formatProvider , "({0}, {1})", X.ToString(),Y.ToString());
    }

  
    public override int GetHashCode() => this.X.GetHashCode() ^ this.Y.GetHashCode() << 2;

    /// <summary>
    ///   <para>Returns true if the given vector is exactly equal to this vector.</para>
    /// </summary>
    /// <param name="other"></param>
  
    public override bool Equals(object other) => other is FixedIntVector2 other1 && this.Equals(other1);

  
    public bool Equals(FixedIntVector2 other)
    {
      return  this.X ==  other.X &&  this.Y ==  other.Y;
    }

    /// <summary>
    ///   <para>Reflects a vector off the surface defined by a normal.</para>
    /// </summary>
    /// <param name="inDirection">The direction vector towards the surface.</param>
    /// <param name="inNormal">The normal vector that defines the surface.</param>
  
    public static FixedIntVector2 Reflect(FixedIntVector2 inDirection, FixedIntVector2 inNormal)
    {
      FixedInt num = -2f * FixedIntVector2.Dot(inNormal, inDirection);
      return new FixedIntVector2(num * inNormal.X + inDirection.X, num * inNormal.Y + inDirection.Y);
    }

    /// <summary>
    ///   <para>Returns the 2D vector perpendicular to this 2D vector. The result is always rotated 90-degrees in a counter-clockwise direction for a 2D coordinate system where the positive Y axis goes up.</para>
    /// </summary>
    /// <param name="inDirection">The input direction.</param>
    /// <returns>
    ///   <para>The perpendicular direction.</para>
    /// </returns>
  
    public static FixedIntVector2 Perpendicular(FixedIntVector2 inDirection)
    {
      return new FixedIntVector2(-inDirection.Y, inDirection.X);
    }

    /// <summary>
    ///   <para>Dot Product of two vectors.</para>
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
  
    public static FixedInt Dot(FixedIntVector2 lhs, FixedIntVector2 rhs)
    {
      return (FixedInt) ( lhs.X *  rhs.X +  lhs.Y *  rhs.Y);
    }

    /// <summary>
    ///   <para>Returns the length of this vector (Read Only).</para>
    /// </summary>
    public FixedInt Magnitude => FixedIntMathf.Sqrt(X * X + Y * Y);

    /// <summary>
    ///   <para>Returns the squared length of this vector (Read Only).</para>
    /// </summary>
    public FixedInt sqrMagnitude => X * X + Y * Y;

    /// <summary>
    ///   <para>Gets the unsigned angle in degrees between from and to.</para>
    /// </summary>
    /// <param name="from">The vector from which the angular difference is measured.</param>
    /// <param name="to">The vector to which the angular difference is measured.</param>
    /// <returns>
    ///   <para>The unsigned angle in degrees between the two vectors.</para>
    /// </returns>
  
    public static FixedInt Angle(FixedIntVector2 from, FixedIntVector2 to)
    {
      FixedInt num = FixedIntMathf.Sqrt( from.sqrMagnitude *  to.sqrMagnitude);
      return  num < 1.0000000036274937E-15 ? 0.0f : (FixedInt) FixedIntMathf.Acos( FixedIntMathf.Clamp(FixedIntVector2.Dot(from, to) / num, -1f, 1f)) * 57.29578f;
    }

    /// <summary>
    ///   <para>Gets the signed angle in degrees between from and to.</para>
    /// </summary>
    /// <param name="from">The vector from which the angular difference is measured.</param>
    /// <param name="to">The vector to which the angular difference is measured.</param>
    /// <returns>
    ///   <para>The signed angle in degrees between the two vectors.</para>
    /// </returns>
  
    public static FixedInt SignedAngle(FixedIntVector2 from, FixedIntVector2 to)
    {
      return FixedIntVector2.Angle(from, to) * FixedIntMathf.Sign((FixedInt) ( from.X *  to.Y -  from.Y *  to.X));
    }

    /// <summary>
    ///   <para>Returns the distance between a and b.</para>
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
  
    public static FixedInt Distance(FixedIntVector2 a, FixedIntVector2 b)
    {
      FixedInt num1 = a.X - b.X;
      FixedInt num2 = a.Y - b.Y;
      return FixedIntMathf.Sqrt( num1 *  num1 +  num2 *  num2);
    }

    /// <summary>
    ///   <para>Returns a copy of vector with its magnitude clamped to maxLength.</para>
    /// </summary>
    /// <param name="vector"></param>
    /// <param name="maxLength"></param>
  
    public static FixedIntVector2 ClampMagnitude(FixedIntVector2 vector, FixedInt maxLength)
    {
      FixedInt sqrMagnitude = vector.sqrMagnitude;
      if ( sqrMagnitude <=  maxLength *  maxLength)
        return vector;
      FixedInt num1 = FixedIntMathf.Sqrt( sqrMagnitude);
      FixedInt num2 = vector.X / num1;
      FixedInt num3 = vector.Y / num1;
      return new FixedIntVector2(num2 * maxLength, num3 * maxLength);
    }

  
    public static FixedInt SqrMagnitude(FixedIntVector2 a)
    {
      return (FixedInt) ( a.X *  a.X +  a.Y *  a.Y);
    }

  
    public FixedInt SqrMagnitude()
    {
      return (FixedInt) ( this.X *  this.X +  this.Y *  this.Y);
    }

    /// <summary>
    ///   <para>Returns a vector that is made from the smallest components of two vectors.</para>
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
  
    public static FixedIntVector2 Min(FixedIntVector2 lhs, FixedIntVector2 rhs)
    {
      return new FixedIntVector2(FixedIntMathf.Min(lhs.X, rhs.X), FixedIntMathf.Min(lhs.Y, rhs.Y));
    }

    /// <summary>
    ///   <para>Returns a vector that is made from the largest components of two vectors.</para>
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
  
    public static FixedIntVector2 Max(FixedIntVector2 lhs, FixedIntVector2 rhs)
    {
      return new FixedIntVector2(FixedIntMathf.Max(lhs.X, rhs.X), FixedIntMathf.Max(lhs.Y, rhs.Y));
    }

  
    // public static FixedIntVector2 SmoothDamp(
    //   FixedIntVector2 current,
    //   FixedIntVector2 target,
    //   ref FixedIntVector2 currentVelocity,
    //   FixedInt smoothTime,
    //   FixedInt maxSpeed)
    // {
    //   FixedInt deltaTime = Time.deltaTime;
    //   return SmoothDamp(current, target, ref currentVelocity, smoothTime, maxSpeed, deltaTime);
    // }

    public static FixedIntVector2 SmoothDamp(
      FixedIntVector2 current,
      FixedIntVector2 target,
      ref FixedIntVector2 currentVelocity,
      FixedInt smoothTime,
      FixedInt deltaTime)
    {
      FixedInt maxSpeed = FixedInt.PositiveInfinity;
      return FixedIntVector2.SmoothDamp(current, target, ref currentVelocity, smoothTime, maxSpeed, deltaTime);
    }

    public static FixedIntVector2 SmoothDamp(
      FixedIntVector2 current,
      FixedIntVector2 target,
      ref FixedIntVector2 currentVelocity,
      FixedInt smoothTime,
      [DefaultValue("FixedInt.PositiveInfinity")] FixedInt maxSpeed,
      FixedInt deltaTime)
    {
      smoothTime = FixedIntMathf.Max(0.0001f, smoothTime);
      FixedInt num1 = 2f / smoothTime;
      FixedInt num2 = num1 * deltaTime;
      FixedInt num3 = (FixedInt) (1.0 / (1.0 +  num2 + 0.47999998927116394 *  num2 *  num2 + 0.23499999940395355 *  num2 *  num2 *  num2));
      FixedInt num4 = current.X - target.X;
      FixedInt num5 = current.Y - target.Y;
      FixedIntVector2 fixedIntVector2 = target;
      FixedInt num6 = maxSpeed * smoothTime;
      FixedInt num7 = num6 * num6;
      FixedInt d = ( num4 *  num4 +  num5 *  num5);
      if ( d >  num7)
      {
        FixedInt num8 = FixedIntMathf.Sqrt( d);
        num4 = num4 / num8 * num6;
        num5 = num5 / num8 * num6;
      }
      target.X = current.X - num4;
      target.Y = current.Y - num5;
      FixedInt num9 = (currentVelocity.X + num1 * num4) * deltaTime;
      FixedInt num10 = (currentVelocity.Y + num1 * num5) * deltaTime;
      currentVelocity.X = (currentVelocity.X - num1 * num9) * num3;
      currentVelocity.Y = (currentVelocity.Y - num1 * num10) * num3;
      FixedInt x = target.X + (num4 + num9) * num3;
      FixedInt y = target.Y + (num5 + num10) * num3;
      FixedInt num11 = fixedIntVector2.X - current.X;
      FixedInt num12 = fixedIntVector2.Y - current.Y;
      FixedInt num13 = x - fixedIntVector2.X;
      FixedInt num14 = y - fixedIntVector2.Y;
      if ( num11 *  num13 +  num12 *  num14 > 0.0)
      {
        x = fixedIntVector2.X;
        y = fixedIntVector2.Y;
        currentVelocity.X = (x - fixedIntVector2.X) / deltaTime;
        currentVelocity.Y = (y - fixedIntVector2.Y) / deltaTime;
      }
      return new FixedIntVector2(x, y);
    }

  
    public static FixedIntVector2 operator +(FixedIntVector2 a, FixedIntVector2 b) => new FixedIntVector2(a.X + b.X, a.Y + b.Y);

  
    public static FixedIntVector2 operator -(FixedIntVector2 a, FixedIntVector2 b) => new FixedIntVector2(a.X - b.X, a.Y - b.Y);

  
    public static FixedIntVector2 operator *(FixedIntVector2 a, FixedIntVector2 b) => new FixedIntVector2(a.X * b.X, a.Y * b.Y);

  
    public static FixedIntVector2 operator /(FixedIntVector2 a, FixedIntVector2 b) => new FixedIntVector2(a.X / b.X, a.Y / b.Y);

  
    public static FixedIntVector2 operator -(FixedIntVector2 a) => new FixedIntVector2(-a.X, -a.Y);

  
    public static FixedIntVector2 operator *(FixedIntVector2 a, FixedInt d) => new FixedIntVector2(a.X * d, a.Y * d);

  
    public static FixedIntVector2 operator *(FixedInt d, FixedIntVector2 a) => new FixedIntVector2(a.X * d, a.Y * d);

  
    public static FixedIntVector2 operator /(FixedIntVector2 a, FixedInt d) => new FixedIntVector2(a.X / d, a.Y / d);

  
    public static bool operator ==(FixedIntVector2 lhs, FixedIntVector2 rhs)
    {
      return lhs.X.Magnification == rhs.X.Magnification &&
             lhs.Y.Magnification == rhs.Y.Magnification;
    }

  
    public static bool operator !=(FixedIntVector2 lhs, FixedIntVector2 rhs) => !(lhs == rhs);

  
    public static implicit operator FixedIntVector2(Vector3 v) => new FixedIntVector2(v.x, v.y);
    
    public static implicit operator FixedIntVector2(Vector2 v) => new FixedIntVector2(v.x, v.y);

  
    public static implicit operator FixedIntVector3(FixedIntVector2 v) => new FixedIntVector3(v.X, v.Y, 0.0f);

    /// <summary>
    ///   <para>Shorthand for writing FixedIntVector2(0, 0).</para>
    /// </summary>
    public static FixedIntVector2 zero
    {
      get => FixedIntVector2.ZeroVector;
    }

    /// <summary>
    ///   <para>Shorthand for writing FixedIntVector2(1, 1).</para>
    /// </summary>
    public static FixedIntVector2 one
    {
      get => FixedIntVector2.OneVector;
    }

    /// <summary>
    ///   <para>Shorthand for writing FixedIntVector2(0, 1).</para>
    /// </summary>
    public static FixedIntVector2 up
    {
      get => FixedIntVector2.UpVector;
    }

    /// <summary>
    ///   <para>Shorthand for writing FixedIntVector2(0, -1).</para>
    /// </summary>
    public static FixedIntVector2 down
    {
      get => FixedIntVector2.DownVector;
    }

    /// <summary>
    ///   <para>Shorthand for writing FixedIntVector2(-1, 0).</para>
    /// </summary>
    public static FixedIntVector2 left
    {
      get => FixedIntVector2.LeftVector;
    }

    /// <summary>
    ///   <para>Shorthand for writing FixedIntVector2(1, 0).</para>
    /// </summary>
    public static FixedIntVector2 right
    {
      get => FixedIntVector2.RightVector;
    }
    
    /// <summary>
    /// Vector2仅用于渲染，不可用于逻辑运算
    /// </summary>
    /// <returns></returns>
    public Vector2 ToVector2()
    {
      return new Vector2(this.X.RenderFloat, this.Y.RenderFloat);
    }
  }
}
