using System.Collections;
using System.Collections.Generic;
using System;
using FixMath;

public class LogicRandom
{
    public int seedid;//随机种子
    Random random;

    public LogicRandom(int seedid)
    {
        this.seedid = seedid;
        random = new Random(seedid);
    }
    //public void InitRandom(int seedid)
    //{
 
    //}
 
    public int Range(int min,int max)
    {
        return random.Next(min,max);
    }

    public FixInt Range(FixInt min, FixInt max)
    {
        return random.Next(min.IntValue, max.IntValue) / 1024f;
        //return FixIntMath.Range(random, min.IntValue, max.IntValue);
    }

    
}
