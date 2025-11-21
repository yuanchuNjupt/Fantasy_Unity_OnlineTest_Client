using System;
using System.Collections;
using System.Collections.Generic;
using GGG.Tool;
using GGG.Tool.Singleton;
using UnityEngine;
using UnityEngine.Serialization;

public class GamePoolManager : Singleton<GamePoolManager>
{
   //1.需要先缓存我们想要缓存的对象(外面先配置对象)
   //2.缓存
   //3.让外部可以获取对象
   //4.回收对象

   [System.Serializable]
   private class PoolItem
   {
      public string ItemName;
      public GameObject item;

      public int initMaxCount;
      
   }
   
   [FormerlySerializedAs("poolItems")] [SerializeField]
   private List<PoolItem> _poolItems = new List<PoolItem>();
   
   private Dictionary<string, Queue<GameObject>> _poolCenter  = new Dictionary<string, Queue<GameObject>>();
   private GameObject _poolItmeParent;
   
   
   
   //初始化
   private void InitPool()
   {
      //1.判断外部配置是不是空的
      if (_poolItems == null || _poolItems.Count == 0)
         return;
      
      //2.遍历外部配置
      for (int i = 0; i < _poolItems.Count; i++)
      {
         for (int j = 0; j < _poolItems[i].initMaxCount; j++)
         {
            var item = Instantiate(_poolItems[i].item, _poolItmeParent.transform, true);
            item.SetActive(false);
            //判断池中有无这个对象的队列
            if (!_poolCenter.ContainsKey(_poolItems[i].ItemName))
            {
               _poolCenter.Add(_poolItems[i].ItemName, new Queue<GameObject>());
            }
            _poolCenter[_poolItems[i].ItemName].Enqueue(item);
         }
      }
      DevelopmentToos.WTF(_poolCenter.Count);
   }

   private void Start()
   {
      _poolItmeParent = new GameObject("对象池");
      _poolItmeParent.transform.SetParent(transform);
      InitPool();
   }

   public void TryGetPoolItem(string itemName , Vector3 position, Quaternion rotation)
   {
      if (_poolCenter.ContainsKey(itemName))
      {
         var item = _poolCenter[itemName].Dequeue();
         item.transform.position = position;
         item.transform.rotation = rotation;
         item.SetActive(true);
         _poolCenter[itemName].Enqueue(item);
      }
      else
      {
         Debug.LogWarning("没有这个对象池");
      }
   }

   public GameObject TryGetPoolItem(string itemName)
   {
      if (_poolCenter.ContainsKey(itemName))
      {
         var item = _poolCenter[itemName].Dequeue();
         
         item.SetActive(true);
         _poolCenter[itemName].Enqueue(item);
         return item;
      }
     
      Debug.LogWarning("没有这个对象池");
      
      return null;
   }
}
