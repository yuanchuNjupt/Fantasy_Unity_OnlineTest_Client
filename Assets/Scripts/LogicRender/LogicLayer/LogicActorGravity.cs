using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FixMath;
/// <summary>
/// 处理逻辑对象重力
/// </summary>
public partial class LogicActor
{
    //竖直上抛公式 v=Vo-gt (V速度 vo：初始速度 g：标准重力 t：时间)
    protected FixInt gravity = 9.8; //标准重力

    public FixIntVector3 velocity;//初始速度
    //初始速度
    private FixInt mVo;
    /// <summary>
    /// 上升时间
    /// </summary>
    protected FixInt mRisingTime;

    public bool isAddForce;//是否添加一个力
    /// <summary>
    /// 是否忽略重力
    /// </summary>
    private bool isIgnoreGravity;
    public bool IgnoreGravity { get { return isIgnoreGravity; } set { Debug.Log("isIgnoreGravity:"+value); isIgnoreGravity = value; } }

    /// <summary>
    /// 处理逻辑对象重力
    /// </summary>
    public void OnLogicFrameUpdateGravity()
    {
        if (isAddForce)
        {
            //1.如何让一下的逻辑，也就是上升和下落的一个逻辑，保持在我们配置的一个时间内。
            //1.上浮时间 2.下浮时间 

            //velocity.y 表示y轴的一个运动速度// 6   0.64  1次=1帧，1帧=0.066秒 c*0.066=t t代表怪物上浮所需要的时间 t
            // 6/0.64=c c*L=t  (vo/gt)*L=t t=上升或下降所消耗的时间 t*2=上升+下降所需要的时间 
            //以知上升和下落的时间，求：如何让逻辑在规定的时间内跑完 x n 2 0.2f 2*2/0.2
            //(vo/gt)*L=t (t*2)/risingtime=tRate 
            float logicFrameInterval= LogicFrameConfig.LogicFrameInterval;
            FixInt gt = gravity * logicFrameInterval;
            //计算上升或下降所需要的时间
            FixInt risingForceTime=(mVo / gt) * logicFrameInterval;
            //获取时间缩放倍率
            FixInt timeScale= (risingForceTime * 2) / mRisingTime;  

            velocity.y -= (gravity * logicFrameInterval) * timeScale;

            //计算要移动的新的位置
            FixIntVector3 newPos = new FixIntVector3(LogicPos.x, FixMath.FixIntMath.Clamp(LogicPos.y + velocity.y * logicFrameInterval, 0, FixInt.MaxValue), LogicPos.z);
            //如果已经忽略了重力，就部进行重力位置更新
            if (!IgnoreGravity)
            {
                LogicPos = newPos;
            }
      
            //表示对象落地了
            if (newPos.y <= 0)
            {
                Debug.Log("EndTiem:" + Time.realtimeSinceStartup);
                isAddForce = false;
                TriggerGround();
             }
            else
            {
                //判断对象是否处于上升阶段
                if (velocity.y>=0)
                {
                    Floating(true);
                }
                else
                {
                    Floating(false);
                }
            }
        
        }
    }
     /// <summary>
    /// 添加上升力
    /// </summary>
    /// <param name="risingForceValue">上升力数值</param>
    /// <param name="risingTime">上升时间</param>
    public void AddRisingForce(FixInt risingForceValue,FixInt risingTime)
    {
        Debug.Log("StartTiem:"+Time.realtimeSinceStartup);
        mVo=velocity.y = risingForceValue;
        mRisingTime = risingTime*1.0f/1000;
        isAddForce = true;
     }
}
