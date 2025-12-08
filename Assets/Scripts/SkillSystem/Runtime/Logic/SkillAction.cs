using FixMath;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Skill 
{
    
    /// <summary>
    /// 行动逻辑帧更新
    /// </summary>
    public void OnLogicFrameUpdateAction()
    {
        //处理行动配置
        if (mSkillData.actionCfgList != null && mSkillData.actionCfgList.Count > 0)
        {
            foreach (var item in mSkillData.actionCfgList)
            {
                if (item.triggerFrame==mCurLogicFrame)
                {
                    //触发行动
                    AddMoveAction(item,mSkillCreater);
                }
            }
        }
    }
    /// <summary>
    /// 添加移动行动
    /// </summary>
    /// <param name="item">行动配置</param>
    /// <param name="logicMoveObj">逻辑移动对象</param>
    public void AddMoveAction(SkillActionConfig item,LogicObject logicMoveObj,Vector3 offset=default(Vector3), Action moveFinish= null,Action moveUpdateCallBack=null)
    {

        void OnActionFinish()
        {
            Debug.Log("MoveToAction Finish");
            moveFinish?.Invoke();
            if (item.actionFinishOpation != MoveActionFinishOpation.None)
            {
                switch (item.actionFinishOpation)
                {
                    case MoveActionFinishOpation.Skill://释放后续技能
                        foreach (var item in item.actionFinishidList)
                        {
                            mSkillCreater.ReleaseSKill(item);
                        }
                        break;
                    case MoveActionFinishOpation.Buff://添加一个buff
                        sKillGuidePos = logicMoveObj.LogicPos;
                        foreach (var buffid in item.actionFinishidList)
                        {
                            BuffSystem.MainInstance.AttachBuff(buffid, mSkillCreater, mSkillCreater, this);
                        }
                        // 1.能在指定地点生成 2.具有碰撞检测 3.能对多人造成伤害。
                        // 群体属性修改Buff
                        break;
                }
            }
        }

        FixIntVector3 movePos = new FixIntVector3(item.movePos);
        FixIntVector3 targetPos;
        FixIntVector3 startPos = logicMoveObj.LogicPos;
        targetPos = logicMoveObj.LogicPos + movePos* logicMoveObj.LogicXAxis; //-1 1
                                                                              //计算移动类型
        // MoveType moveType = MoveType.target;
        // if (item.moveActionType == MoveActionType.TargetPos)
        // {
        //     if (movePos.x != FixInt.Zero && movePos.y == FixInt.Zero && movePos.z == FixInt.Zero)
        //     {
        //         moveType = MoveType.X;
        //     }
        //     else if (movePos.x == FixInt.Zero && movePos.y != FixInt.Zero && movePos.z == FixInt.Zero)
        //     {
        //         moveType = MoveType.Y;
        //     }
        //     else if (movePos.x == FixInt.Zero && movePos.y == FixInt.Zero && movePos.z != FixInt.Zero)
        //     {
        //         moveType = MoveType.Z;
        //     }
        // }
        //处理技能引导位置移动逻辑
        // else if (item.moveActionType == MoveActionType.GuidePos)
        // {
        //     //目标位置
        //     targetPos = sKillGuidePos;
        //     //起始位置
        //     startPos = targetPos + mSkillCreater.LogicXAxis * new FixIntVector3(offset);
        //     startPos.y = FixIntMath.Abs(startPos.y);
        // }
        // else if (item.moveActionType == MoveActionType.BezierPos)
        // {
        //     //1.计算起始位置
        //     startPos = mSkillCreater.LogicPos + mSkillCreater.LogicXAxis * new FixIntVector3 (offset);
        //     startPos.y = FixIntMath.Abs(startPos.y);//不让当前对象，y<0，否则就会跑到地下去
        //     //2.计算最高点位置
        //     FixIntVector3 heightPosOffset = new FixIntVector3(item.heightPos) * mSkillCreater.LogicXAxis;
        //     heightPosOffset.y = FixIntMath.Abs(heightPosOffset.y);
        //     FixIntVector3 heightPos = mSkillCreater.LogicPos + heightPosOffset;
        //     //3.计算结束位置
        //     FixIntVector3 endPosOffset = new FixIntVector3(item.movePos) * mSkillCreater.LogicXAxis;
        //     endPosOffset.y= FixIntMath.Abs(endPosOffset.y);
        //     targetPos = mSkillCreater.LogicPos + endPosOffset;
        //     //3.执行贝塞尔运动
        //     // MoveBezierAction moveBezier = new MoveBezierAction(logicMoveObj,startPos,heightPos,targetPos,item.durationMs, OnActionFinish, moveUpdateCallBack);
        //     // LogicActionController.Instance.RunAciton(moveBezier);
        //     return;
        // }
        // MoveToAction action = new MoveToAction(logicMoveObj, startPos, targetPos,item.durationMs, OnActionFinish, moveUpdateCallBack, moveType);
        // //开始行动
        // LogicActionController.Instance.RunAciton(action);
    }
}
