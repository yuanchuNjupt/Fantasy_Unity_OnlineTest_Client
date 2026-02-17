using FixMath;
using System.Collections;
using System.Collections.Generic;
using FixedPhysics.Fixed_pointNumber.Core;
using UnityEngine;

public partial class LogicActor
{
    protected FixedInt level = 1;//等级
    protected string name;//名称
    protected FixedInt id;//唯一id
    protected FixedInt type;//类型

    #region 内部属性(最基础的属性)
    protected FixedInt hp;//血量
    protected FixedInt mp;//法力值
    protected FixedInt ap;//魔法攻击力
    protected FixedInt ad;//物理攻击力
    protected FixedInt adDef;//物理防御力
    protected FixedInt apDef;//魔法防御力
    protected FixedInt pct;//物理暴击率
    protected FixedInt mct;//魔法暴击率
    protected FixedInt adPctRate;//物理暴击倍率
    protected FixedInt apMctRate;//魔法暴击倍率
    //四维
    protected FixedInt str;//力量
    protected FixedInt sta;//体力
    protected FixedInt Int;//智力
    protected FixedInt spi;//精神

    protected FixedInt agl;//敏捷

    protected FixedInt atkRange; //攻击距离，用于区别远程怪物和近战怪物的攻击距离
    protected FixedInt searchDisRange;//搜寻距离 用于出生后首次搜寻目标进行进行追击
    #endregion

    #region 战斗时通过buff增加的属性
    public FixedInt addADDef;//战斗时通过buff增加的防御力
    public FixedInt addAPDef;
    public FixedInt addAD;
    public FixedInt addAP;
    public FixedInt addMCT;
    public int addPCT;
    public FixedInt addAPMACTRate;
    public FixedInt addAdPCTRate;

    public FixedInt addStr;//力量
    public FixedInt addSta;//体力
    public FixedInt addInt;//智力
    public FixedInt addSpi;//精神

    public FixedInt addAgl;//敏捷
    #endregion

    #region 公开属性
    public FixedInt HP { get { return hp ; }}//血量
    public FixedInt MP { get { return mp; } }//法力值
    public FixedInt AP { get { return addAP + ap; } }//魔法攻击力
    public FixedInt AD { get { return addAD + ad; } }//物理攻击力
    public FixedInt ADDef { get { return addADDef + adDef; } }//物理防御力
    public FixedInt APdef { get { return addAPDef + apDef; } }//魔法防御力
    public FixedInt PCT { get { return addPCT + pct; } }//物理暴击率
    public FixedInt MCT { get { return addMCT + mct; } }//魔法暴击率
    public FixedInt ADPCTRate { get { return addAdPCTRate + adPctRate; } }//物理暴击倍率
    public FixedInt APMCTRate { get { return addAPMACTRate + apMctRate; } }//魔法暴击倍率
    //四维
    public FixedInt STR { get { return addStr + str; } }//力量
    public FixedInt STA { get { return addSta + sta; } }//体力
    public FixedInt INT { get { return addInt + Int; } }//智力
    public FixedInt SPI { get { return addSpi + spi; } }//精神

    public FixedInt AGL { get { return addAgl + agl; } }//敏捷

    public FixedInt Level { get { return level; } }//等级
    #endregion
    /// <summary>
    /// 减少血量
    /// </summary>
    /// <param name="reduceHp"></param>
    public void ReduceHP(FixedInt reduceHp)
    {
        hp -= reduceHp;
        if (hp<=0)
        {
            hp = 0;
        }
    }
}
