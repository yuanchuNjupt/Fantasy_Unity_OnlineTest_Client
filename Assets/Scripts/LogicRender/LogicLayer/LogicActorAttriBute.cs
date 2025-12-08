using FixMath;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class LogicActor
{
    protected FixInt level = 1;//等级
    protected string name;//名称
    protected FixInt id;//唯一id
    protected FixInt type;//类型

    #region 内部属性(最基础的属性)
    protected FixInt hp;//血量
    protected FixInt mp;//法力值
    protected FixInt ap;//魔法攻击力
    protected FixInt ad;//物理攻击力
    protected FixInt adDef;//物理防御力
    protected FixInt apDef;//魔法防御力
    protected FixInt pct;//物理暴击率
    protected FixInt mct;//魔法暴击率
    protected FixInt adPctRate;//物理暴击倍率
    protected FixInt apMctRate;//魔法暴击倍率
    //四维
    protected FixInt str;//力量
    protected FixInt sta;//体力
    protected FixInt Int;//智力
    protected FixInt spi;//精神

    protected FixInt agl;//敏捷

    protected FixInt atkRange; //攻击距离，用于区别远程怪物和近战怪物的攻击距离
    protected FixInt searchDisRange;//搜寻距离 用于出生后首次搜寻目标进行进行追击
    #endregion

    #region 战斗时通过buff增加的属性
    public FixInt addADDef;//战斗时通过buff增加的防御力
    public FixInt addAPDef;
    public FixInt addAD;
    public FixInt addAP;
    public FixInt addMCT;
    public int addPCT;
    public FixInt addAPMACTRate;
    public FixInt addAdPCTRate;

    public FixInt addStr;//力量
    public FixInt addSta;//体力
    public FixInt addInt;//智力
    public FixInt addSpi;//精神

    public FixInt addAgl;//敏捷
    #endregion

    #region 公开属性
    public FixInt HP { get { return hp ; }}//血量
    public FixInt MP { get { return mp; } }//法力值
    public FixInt AP { get { return addAP + ap; } }//魔法攻击力
    public FixInt AD { get { return addAD + ad; } }//物理攻击力
    public FixInt ADDef { get { return addADDef + adDef; } }//物理防御力
    public FixInt APdef { get { return addAPDef + apDef; } }//魔法防御力
    public FixInt PCT { get { return addPCT + pct; } }//物理暴击率
    public FixInt MCT { get { return addMCT + mct; } }//魔法暴击率
    public FixInt ADPCTRate { get { return addAdPCTRate + adPctRate; } }//物理暴击倍率
    public FixInt APMCTRate { get { return addAPMACTRate + apMctRate; } }//魔法暴击倍率
    //四维
    public FixInt STR { get { return addStr + str; } }//力量
    public FixInt STA { get { return addSta + sta; } }//体力
    public FixInt INT { get { return addInt + Int; } }//智力
    public FixInt SPI { get { return addSpi + spi; } }//精神

    public FixInt AGL { get { return addAgl + agl; } }//敏捷

    public FixInt Level { get { return level; } }//等级
    #endregion
    /// <summary>
    /// 减少血量
    /// </summary>
    /// <param name="reduceHp"></param>
    public void ReduceHP(FixInt reduceHp)
    {
        hp -= reduceHp;
        if (hp<=0)
        {
            hp = 0;
        }
    }
}
