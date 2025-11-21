using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ARPG.ComboData.ComboData
{
    [CreateAssetMenu(fileName = "Combo", menuName = "Create/Character/Combo", order = 0)]
    public class CharacterComboSO : ScriptableObject
    {
        [SerializeField] private List<CharacterComboDataSO> _allComboData = new List<CharacterComboDataSO>();
        
        
        
        /// <summary>
        ///只关心给东西
        ///玩家干嘛我不关心
        /// </summary>
        
        
        
        /// <summary>
        /// 尝试去获取连招中的某一个招式
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string TryGetOneComboAction(int index)
        {
            //如果当前连招标啥也没有 就返回null
            if (_allComboData.Count == 0)
                return null;
            
            return _allComboData[index].ComboName;
            
        }
        
        
        /// <summary>
        /// 获取对应的受伤动画
        /// </summary>
        /// <param name="index"></param>
        /// <param name="hitIndex"></param>
        /// <returns></returns>
        public string TryGetOneHitname(int index, int hitIndex)
        {
            //如果当前连招标啥也没有 就返回null
            if (_allComboData.Count == 0)
                return null;
            //防止配置的时候忘填数据 报空引用
            if (_allComboData[index].GetHitAndParryMaxCount() == 0)
                return null;
            Debug.Log("hitIndex:" + hitIndex);
            return _allComboData[index].ComboHitName[hitIndex];
        }
        
        
        /// <summary>
        /// 获取对应的格挡动画
        /// </summary>
        /// <param name="index"></param>
        /// <param name="parryIndex"></param>
        /// <returns></returns>
        public string TryGetOneParryname(int index, int parryIndex)
        {
            //如果当前连招标啥也没有 就返回null
            if (_allComboData.Count == 0)
                return null;
            //防止配置的时候忘填数据 报空引用
            if (_allComboData[index].GetHitAndParryMaxCount() == 0)
                return null;
            return _allComboData[index].ComboParryName[parryIndex];
        }

        public float TryGetComboDamage(int index)
        {
            if (_allComboData.Count == 0)
                return 0f;
            return _allComboData[index].Damage;
        }

        public float TryGetColdTime(int index)
        {
            if (_allComboData.Count == 0)
                return 0f;
            return _allComboData[index].ComboTime;
        }
        
        public float TryGetComboPositionOffset(int index)
        {
            if (_allComboData.Count == 0)
                return 0f;
            Debug.Log("偏移值:" + _allComboData[index].ComboPositionOffest);
            return _allComboData[index].ComboPositionOffest;
            
        }

        public int TryGetHitAndParryMaxCount(int index)
        {
            return _allComboData[index].GetHitAndParryMaxCount();
        }
        
        //连招里面有多少个招式
        public int TryGetComboMaxCount()
        {
            try
            {
                return _allComboData.Count;
            }
            catch (Exception e)
            {
                Debug.Log("获取连招最大数量失败" + _allComboData.Count);
                Debug.LogError(e);
                throw;
            }
        }

    }
}