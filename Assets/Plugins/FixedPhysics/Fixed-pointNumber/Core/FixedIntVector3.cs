﻿using System;
using System.Globalization;
using FixedPhysics.Fixed_pointNumber.FixedIntMath;
using FixedPhysics.Fixed_pointNumber.Interfaces;
using UnityEngine;
using UnityEngine.Internal;

namespace FixedPhysics.Fixed_pointNumber.Core
{
  public struct FixedIntVector3 : IFixedIntVector3 , IEquatable<FixedIntVector3>, IFormattable
  {

    /// <summary>
    ///   <para>X component of the vector.</para>
    /// </summary>
    public FixedInt X;

    /// <summary>
    ///   <para>Y component of the vector.</para>
    /// </summary>
    public FixedInt Y;

    /// <summary>
    ///   <para>Z component of the vector.</para>
    /// </summary>
    public FixedInt Z;
    private static readonly FixedIntVector3 zeroVector = new FixedIntVector3(0.0f, 0.0f, 0.0f);
    private static readonly FixedIntVector3 oneVector = new FixedIntVector3(1f, 1f, 1f);
    private static readonly FixedIntVector3 upVector = new FixedIntVector3(0.0f, 1f, 0.0f);
    private static readonly FixedIntVector3 downVector = new FixedIntVector3(0.0f, -1f, 0.0f);
    private static readonly FixedIntVector3 leftVector = new FixedIntVector3(-1f, 0.0f, 0.0f);
    private static readonly FixedIntVector3 rightVector = new FixedIntVector3(1f, 0.0f, 0.0f);
    private static readonly FixedIntVector3 forwardVector = new FixedIntVector3(0.0f, 0.0f, 1f);
    private static readonly FixedIntVector3 backVector = new FixedIntVector3(0.0f, 0.0f, -1f);


    
    
    

    /// <summary>
    ///   <para>Linearly interpolates between two points.</para>
    /// </summary>
    /// <param name="a">Start value, returned when t = 0.</param>
    /// <param name="b">End value, returned when t = 1.</param>
    /// <param name="t">Value used to interpolate between a and b.</param>
    /// <returns>
    ///   <para>Interpolated value, equals to a + (b - a) * t.</para>
    /// </returns>
    
    public static FixedIntVector3 Lerp(FixedIntVector3 a, FixedIntVector3 b, FixedInt t)
    {
      t = FixedIntMathf.Clamp(t , 0 , 1);
      return new FixedIntVector3(a.X + (b.X - a.X) * t, a.Y + (b.Y - a.Y) * t, a.Z + (b.Z - a.Z) * t);
    }

    /// <summary>
    ///   <para>Linearly interpolates between two vectors.</para>
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="t"></param>
    
    public static FixedIntVector3 LerpUnclamped(FixedIntVector3 a, FixedIntVector3 b, FixedInt t)
    {
      return new FixedIntVector3(a.X + (b.X - a.X) * t, a.Y + (b.Y - a.Y) * t, a.Z + (b.Z - a.Z) * t);
    }

    /// <summary>
    ///   <para>Calculate a position between the points specified by current and target, moving no farther than the distance specified by maxDistanceDelta.</para>
    /// </summary>
    /// <param name="current">The position to move from.</param>
    /// <param name="target">The position to move towards.</param>
    /// <param name="maxDistanceDelta">Distance to move current per call.</param>
    /// <returns>
    ///   <para>The new position.</para>
    /// </returns>
    
    public static FixedIntVector3 MoveTowards(FixedIntVector3 current, FixedIntVector3 target, FixedInt maxDistanceDelta)
    {
      FixedInt num1 = target.X - current.X;
      FixedInt num2 = target.Y - current.Y;
      FixedInt num3 = target.Z - current.Z;
      FixedInt d = (FixedInt) ( num1 *  num1 +  num2 *  num2 +  num3 *  num3);
      if ( d == 0.0 ||  maxDistanceDelta >= 0.0 &&  d <=  maxDistanceDelta *  maxDistanceDelta)
        return target;
      FixedInt num4 = FixedIntMathf.Sqrt( d);
      return new FixedIntVector3(current.X + num1 / num4 * maxDistanceDelta, current.Y + num2 / num4 * maxDistanceDelta, current.Z + num3 / num4 * maxDistanceDelta);
    }

    // public static FixedIntVector3 SmoothDamp(
    //   FixedIntVector3 current,
    //   FixedIntVector3 target,
    //   ref FixedIntVector3 currentVelocity,
    //   FixedInt smoothTime,
    //   FixedInt maxSpeed,
    //   FixedInt fixedDeltaTime)
    // {
    //   return FixedIntVector3.SmoothDamp(current, target, ref currentVelocity, smoothTime, maxSpeed, fixedDeltaTime);
    // }

    
    public static FixedIntVector3 SmoothDamp(
      FixedIntVector3 current,
      FixedIntVector3 target,
      ref FixedIntVector3 currentVelocity,
      FixedInt smoothTime,
      FixedInt deltaTime)
    {
      FixedInt maxSpeed = FixedInt.PositiveInfinity;
      return FixedIntVector3.SmoothDamp(current, target, ref currentVelocity, smoothTime, maxSpeed, deltaTime);
    }

    public static FixedIntVector3 SmoothDamp(
      FixedIntVector3 current,
      FixedIntVector3 target,
      ref FixedIntVector3 currentVelocity,
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
      FixedInt num6 = current.Z - target.Z;
      FixedIntVector3 fixedIntVector3 = target;
      FixedInt num7 = maxSpeed * smoothTime;
      FixedInt num8 = num7 * num7;
      FixedInt d = (FixedInt) ( num4 *  num4 +  num5 *  num5 +  num6 *  num6);
      if ( d >  num8)
      {
        FixedInt num9 = FixedIntMathf.Sqrt( d);
        num4 = num4 / num9 * num7;
        num5 = num5 / num9 * num7;
        num6 = num6 / num9 * num7;
      }
      target.X = current.X - num4;
      target.Y = current.Y - num5;
      target.Z = current.Z - num6;
      FixedInt num10 = (currentVelocity.X + num1 * num4) * deltaTime;
      FixedInt num11 = (currentVelocity.Y + num1 * num5) * deltaTime;
      FixedInt num12 = (currentVelocity.Z + num1 * num6) * deltaTime;
      currentVelocity.X = (currentVelocity.X - num1 * num10) * num3;
      currentVelocity.Y = (currentVelocity.Y - num1 * num11) * num3;
      currentVelocity.Z = (currentVelocity.Z - num1 * num12) * num3;
      FixedInt x = target.X + (num4 + num10) * num3;
      FixedInt y = target.Y + (num5 + num11) * num3;
      FixedInt z = target.Z + (num6 + num12) * num3;
      FixedInt num13 = fixedIntVector3.X - current.X;
      FixedInt num14 = fixedIntVector3.Y - current.Y;
      FixedInt num15 = fixedIntVector3.Z - current.Z;
      FixedInt num16 = x - fixedIntVector3.X;
      FixedInt num17 = y - fixedIntVector3.Y;
      FixedInt num18 = z - fixedIntVector3.Z;
      if ( num13 *  num16 +  num14 *  num17 +  num15 *  num18 > 0.0)
      {
        x = fixedIntVector3.X;
        y = fixedIntVector3.Y;
        z = fixedIntVector3.Z;
        currentVelocity.X = (x - fixedIntVector3.X) / deltaTime;
        currentVelocity.Y = (y - fixedIntVector3.Y) / deltaTime;
        currentVelocity.Z = (z - fixedIntVector3.Z) / deltaTime;
      }
      return new FixedIntVector3(x, y, z);
    }

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
          case 2:
            return this.Z;
          default:
            throw new IndexOutOfRangeException("Invalid FixedIntVector3 index!");
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
          case 2:
            this.Z = value;
            break;
          default:
            throw new IndexOutOfRangeException("Invalid FixedIntVector3 index!");
        }
      }
    }

    /// <summary>
    ///   <para>Creates a new vector with given x, y, z components.</para>
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    
    public FixedIntVector3(FixedInt x, FixedInt y, FixedInt z)
    {
      this.X = x;
      this.Y = y;
      this.Z = z;
    }

    /// <summary>
    ///   <para>Creates a new vector with given x, y components and sets z to zero.</para>
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    
    public FixedIntVector3(FixedInt x, FixedInt y)
    {
      this.X = x;
      this.Y = y;
      this.Z = 0.0f;
    }
    
    public FixedIntVector3(Vector3 unityVector)
    {
      this.X = (FixedInt)unityVector.x;
      this.Y = (FixedInt)unityVector.y;
      this.Z = (FixedInt)unityVector.z;
    }
    

    /// <summary>
    ///   <para>Set x, y and z components of an existing FixedIntVector3.</para>
    /// </summary>
    /// <param name="newX"></param>
    /// <param name="newY"></param>
    /// <param name="newZ"></param>
    
    public void Set(FixedInt newX, FixedInt newY, FixedInt newZ)
    {
      this.X = newX;
      this.Y = newY;
      this.Z = newZ;
    }

    /// <summary>
    ///   <para>Multiplies two vectors component-wise.</para>
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    
    public static FixedIntVector3 Scale(FixedIntVector3 a, FixedIntVector3 b) => new FixedIntVector3(a.X * b.X, a.Y * b.Y, a.Z * b.Z);

    /// <summary>
    ///   <para>Multiplies every component of this vector by the same component of scale.</para>
    /// </summary>
    /// <param name="scale"></param>
    
    public void Scale(FixedIntVector3 scale)
    {
      this.X *= scale.X;
      this.Y *= scale.Y;
      this.Z *= scale.Z;
    }

    /// <summary>
    ///   <para>Cross Product of two vectors.</para>
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    
    public static FixedIntVector3 Cross(FixedIntVector3 lhs, FixedIntVector3 rhs)
    {
      return new FixedIntVector3((FixedInt) ( lhs.Y *  rhs.Z -  lhs.Z *  rhs.Y), (FixedInt) ( lhs.Z *  rhs.X -  lhs.X *  rhs.Z), (FixedInt) ( lhs.X *  rhs.Y -  lhs.Y *  rhs.X));
    }

    
    public override int GetHashCode()
    {
      return this.X.GetHashCode() ^ this.Y.GetHashCode() << 2 ^ this.Z.GetHashCode() >> 2;
    }

    /// <summary>
    ///   <para>Returns true if the given vector is exactly equal to this vector.</para>
    /// </summary>
    /// <param name="other"></param>
    
    public override bool Equals(object other) => other is FixedIntVector3 other1 && this.Equals(other1);

    
    public bool Equals(FixedIntVector3 other)
    {
      return  this.X ==  other.X &&  this.Y ==  other.Y &&  this.Z ==  other.Z;
    }

    /// <summary>
    ///   <para>Reflects a vector off the plane defined by a normal.</para>
    /// </summary>
    /// <param name="inDirection">The direction vector towards the plane.</param>
    /// <param name="inNormal">The normal vector that defines the plane.</param>
    
    public static FixedIntVector3 Reflect(FixedIntVector3 inDirection, FixedIntVector3 inNormal)
    {
      FixedInt num = -2f * FixedIntVector3.Dot(inNormal, inDirection);
      return new FixedIntVector3(num * inNormal.X + inDirection.X, num * inNormal.Y + inDirection.Y, num * inNormal.Z + inDirection.Z);
    }

    /// <summary>
    ///   <para>Returns a normalized vector based on the given vector. The normalized vector has a magnitude of 1 and is in the same direction as the given vector. Returns a zero vector If the given vector is too small to be normalized.</para>
    /// </summary>
    /// <param name="value">The vector to be normalized.</param>
    /// <returns>
    ///   <para>A new vector with the same direction as the original vector but with a magnitude of 1.0.</para>
    /// </returns>
    
    public static FixedIntVector3 Normalize(FixedIntVector3 value)
    {
      FixedInt num = FixedIntVector3.Magnitude(value);
      return  num > 9.999999747378752E-06 ? value / num : FixedIntVector3.zero;
    }

    /// <summary>
    ///   <para>Makes this vector have a magnitude of 1.</para>
    /// </summary>
    
    public void Normalize()
    {
      FixedInt num = FixedIntVector3.Magnitude(this);
      if ( num > 9.999999747378752E-06)
        this = this / num;
      else
        this = FixedIntVector3.zero;
    }

    /// <summary>
    ///   <para>Returns a normalized vector based on the current vector. The normalized vector has a magnitude of 1 and is in the same direction as the current vector. Returns a zero vector If the current vector is too small to be normalized.</para>
    /// </summary>
    public FixedIntVector3 normalized
    {
       get => FixedIntVector3.Normalize(this);
    }

    /// <summary>
    ///   <para>Dot Product of two vectors.</para>
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    
    public static FixedInt Dot(FixedIntVector3 lhs, FixedIntVector3 rhs)
    {
      return (FixedInt) ( lhs.X *  rhs.X +  lhs.Y *  rhs.Y +  lhs.Z *  rhs.Z);
    }

    /// <summary>
    ///   <para>Projects a vector onto another vector.</para>
    /// </summary>
    /// <param name="vector"></param>
    /// <param name="onNormal"></param>
    
    public static FixedIntVector3 Project(FixedIntVector3 vector, FixedIntVector3 onNormal)
    {
      FixedInt num1 = FixedIntVector3.Dot(onNormal, onNormal);
      if ( num1 <  Mathf.Epsilon)
        return FixedIntVector3.zero;
      FixedInt num2 = FixedIntVector3.Dot(vector, onNormal);
      return new FixedIntVector3(onNormal.X * num2 / num1, onNormal.Y * num2 / num1, onNormal.Z * num2 / num1);
    }

    /// <summary>
    ///   <para>Projects a vector onto a plane.</para>
    /// </summary>
    /// <param name="vector">The vector to project on the plane.</param>
    /// <param name="planeNormal">The normal which defines the plane to project on.</param>
    /// <returns>
    ///   <para>The orthogonal projection of vector on the plane.</para>
    /// </returns>
    
    public static FixedIntVector3 ProjectOnPlane(FixedIntVector3 vector, FixedIntVector3 planeNormal)
    {
      FixedInt num1 = FixedIntVector3.Dot(planeNormal, planeNormal);
      if ( num1 <  Mathf.Epsilon)
        return vector;
      FixedInt num2 = FixedIntVector3.Dot(vector, planeNormal);
      return new FixedIntVector3(vector.X - planeNormal.X * num2 / num1, vector.Y - planeNormal.Y * num2 / num1, vector.Z - planeNormal.Z * num2 / num1);
    }

    /// <summary>
    ///   <para>Calculates the angle between two vectors.</para>
    /// </summary>
    /// <param name="from">The vector from which the angular difference is measured.</param>
    /// <param name="to">The vector to which the angular difference is measured.</param>
    /// <returns>
    ///   <para>The angle in degrees between the two vectors.</para>
    /// </returns>
    
    public static FixedInt Angle(FixedIntVector3 from, FixedIntVector3 to)
    {
      FixedInt num = FixedIntMathf.Sqrt( from.sqrMagnitude *  to.sqrMagnitude);
      return  num < 1.0000000036274937E-15 ? 0.0f : FixedIntMathf.Acos( FixedIntMathf.Clamp(FixedIntVector3.Dot(from, to) / num, -1f, 1f)) * 57.29578f;
    }

    /// <summary>
    ///   <para>Calculates the signed angle between vectors from and to in relation to axis.</para>
    /// </summary>
    /// <param name="from">The vector from which the angular difference is measured.</param>
    /// <param name="to">The vector to which the angular difference is measured.</param>
    /// <param name="axis">A vector around which the other vectors are rotated.</param>
    /// <returns>
    ///   <para>Returns the signed angle between from and to in degrees.</para>
    /// </returns>
    
    public static FixedInt SignedAngle(FixedIntVector3 from, FixedIntVector3 to, FixedIntVector3 axis)
    {
      FixedInt num1 = FixedIntVector3.Angle(from, to);
      FixedInt num2 = (FixedInt) ( from.Y *  to.Z -  from.Z *  to.Y);
      FixedInt num3 = (FixedInt) ( from.Z *  to.X -  from.X *  to.Z);
      FixedInt num4 = (FixedInt) ( from.X *  to.Y -  from.Y *  to.X);
      FixedInt num5 = FixedIntMathf.Sign((FixedInt) ( axis.X *  num2 +  axis.Y *  num3 +  axis.Z *  num4));
      return num1 * num5;
    }

    /// <summary>
    ///   <para>Returns the distance between a and b.</para>
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    
    public static FixedInt Distance(FixedIntVector3 a, FixedIntVector3 b)
    {
      FixedInt num1 = a.X - b.X;
      FixedInt num2 = a.Y - b.Y;
      FixedInt num3 = a.Z - b.Z;
      return FixedIntMathf.Sqrt( num1 *  num1 +  num2 *  num2 +  num3 *  num3);
    }

    /// <summary>
    ///   <para>Returns a copy of vector with its magnitude clamped to maxLength.</para>
    /// </summary>
    /// <param name="vector"></param>
    /// <param name="maxLength"></param>
    
    public static FixedIntVector3 ClampMagnitude(FixedIntVector3 vector, FixedInt maxLength)
    {
      FixedInt sqrMagnitude = vector.sqrMagnitude;
      if ( sqrMagnitude <=  maxLength *  maxLength)
        return vector;
      FixedInt num1 = FixedIntMathf.Sqrt( sqrMagnitude);
      FixedInt num2 = vector.X / num1;
      FixedInt num3 = vector.Y / num1;
      FixedInt num4 = vector.Z / num1;
      return new FixedIntVector3(num2 * maxLength, num3 * maxLength, num4 * maxLength);
    }

    
    public static FixedInt Magnitude(FixedIntVector3 vector)
    {
      return FixedIntMathf.Sqrt( vector.X *  vector.X +  vector.Y *  vector.Y +  vector.Z *  vector.Z);
    }

    /// <summary>
    ///   <para>Returns the length of this vector (Read Only).</para>
    /// </summary>
    public FixedInt magnitude
    {
       get
      {
        return FixedIntMathf.Sqrt( this.X *  this.X +  this.Y *  this.Y +  this.Z *  this.Z);
      }
    }

    
    public static FixedInt SqrMagnitude(FixedIntVector3 vector)
    {
      return (FixedInt) ( vector.X *  vector.X +  vector.Y *  vector.Y +  vector.Z *  vector.Z);
    }

    /// <summary>
    ///   <para>Returns the squared length of this vector (Read Only).</para>
    /// </summary>
    public FixedInt sqrMagnitude
    {
       get
      {
        return (FixedInt) ( this.X *  this.X +  this.Y *  this.Y +  this.Z *  this.Z);
      }
    }

    /// <summary>
    ///   <para>Returns a vector that is made from the smallest components of two vectors.</para>
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    
    public static FixedIntVector3 Min(FixedIntVector3 lhs, FixedIntVector3 rhs)
    {
      return new FixedIntVector3(FixedIntMathf.Min(lhs.X, rhs.X), FixedIntMathf.Min(lhs.Y, rhs.Y), FixedIntMathf.Min(lhs.Z, rhs.Z));
    }

    /// <summary>
    ///   <para>Returns a vector that is made from the largest components of two vectors.</para>
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    
    public static FixedIntVector3 Max(FixedIntVector3 lhs, FixedIntVector3 rhs)
    {
      return new FixedIntVector3(FixedIntMathf.Max(lhs.X, rhs.X), FixedIntMathf.Max(lhs.Y, rhs.Y), FixedIntMathf.Max(lhs.Z, rhs.Z));
    }

    /// <summary>
    ///   <para>Shorthand for writing FixedIntVector3(0, 0, 0).</para>
    /// </summary>
    public static FixedIntVector3 zero
    {
       get => FixedIntVector3.zeroVector;
    }

    /// <summary>
    ///   <para>Shorthand for writing FixedIntVector3(1, 1, 1).</para>
    /// </summary>
    public static FixedIntVector3 one
    {
       get => FixedIntVector3.oneVector;
    }

    /// <summary>
    ///   <para>Shorthand for writing FixedIntVector3(0, 0, 1).</para>
    /// </summary>
    public static FixedIntVector3 forward
    {
       get => FixedIntVector3.forwardVector;
    }

    /// <summary>
    ///   <para>Shorthand for writing FixedIntVector3(0, 0, -1).</para>
    /// </summary>
    public static FixedIntVector3 back
    {
       get => FixedIntVector3.backVector;
    }

    /// <summary>
    ///   <para>Shorthand for writing FixedIntVector3(0, 1, 0).</para>
    /// </summary>
    public static FixedIntVector3 up
    {
       get => FixedIntVector3.upVector;
    }

    /// <summary>
    ///   <para>Shorthand for writing FixedIntVector3(0, -1, 0).</para>
    /// </summary>
    public static FixedIntVector3 down
    {
       get => FixedIntVector3.downVector;
    }

    /// <summary>
    ///   <para>Shorthand for writing FixedIntVector3(-1, 0, 0).</para>
    /// </summary>
    public static FixedIntVector3 left
    {
       get => FixedIntVector3.leftVector;
    }

    /// <summary>
    ///   <para>Shorthand for writing FixedIntVector3(1, 0, 0).</para>
    /// </summary>
    public static FixedIntVector3 right
    {
       get => FixedIntVector3.rightVector;
    }
    
    public static FixedIntVector3 operator +(FixedIntVector3 a, FixedIntVector3 b)
    {
      return new FixedIntVector3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
    }

    
    public static FixedIntVector3 operator -(FixedIntVector3 a, FixedIntVector3 b)
    {
      return new FixedIntVector3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
    }

    
    public static FixedIntVector3 operator -(FixedIntVector3 a) => new FixedIntVector3(-a.X, -a.Y, -a.Z);

    
    public static FixedIntVector3 operator *(FixedIntVector3 a, FixedInt d) => new FixedIntVector3(a.X * d, a.Y * d, a.Z * d);

    
    public static FixedIntVector3 operator *(FixedInt d, FixedIntVector3 a) => new FixedIntVector3(a.X * d, a.Y * d, a.Z * d);

    
    public static FixedIntVector3 operator /(FixedIntVector3 a, FixedInt d) => new FixedIntVector3(a.X / d, a.Y / d, a.Z / d);

    
    public static bool operator ==(FixedIntVector3 lhs, FixedIntVector3 rhs)
    {
      FixedInt num1 = lhs.X - rhs.X;
      FixedInt num2 = lhs.Y - rhs.Y;
      FixedInt num3 = lhs.Z - rhs.Z;
      return  num1 *  num1 +  num2 *  num2 +  num3 *  num3 < 9.999999439624929E-11;
    }

    
    public static bool operator !=(FixedIntVector3 lhs, FixedIntVector3 rhs) => !(lhs == rhs);

    /// <summary>
    ///   <para>Returns a formatted string for this vector.</para>
    /// </summary>
    /// <param name="format">A numeric format string.</param>
    /// <param name="formatProvider">An object that specifies culture-specific formatting.</param>
    
    public override string ToString() => this.ToString((string) null, (IFormatProvider) null);

    /// <summary>
    ///   <para>Returns a formatted string for this vector.</para>
    /// </summary>
    /// <param name="format">A numeric format string.</param>
    /// <param name="formatProvider">An object that specifies culture-specific formatting.</param>
    
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
        formatProvider = (IFormatProvider) CultureInfo.InvariantCulture.NumberFormat;
      return string.Format(formatProvider,"({0}, {1}, {2})", X.ToString(), Y.ToString(), Z.ToString());
    }


    /// <summary>
    /// Vector3仅用于渲染，不可用于逻辑运算
    /// </summary>
    /// <returns></returns>
    [Obsolete("Vector3仅用于渲染，不可用于逻辑运算", false)]
    public Vector3 ToVector3()
    {
      return new Vector3(X.RenderFloat, Y.RenderFloat, Z.RenderFloat);
    }
    
    /// <summary>
    /// 隐式转换操作符：从 Unity Vector3 转换为 FixedIntVector3
    /// </summary>
    public static implicit operator FixedIntVector3(Vector3 v) => new FixedIntVector3(v.x, v.y, v.z);
    
  }
}
