using System;
using System.Collections;
using System.Collections.Generic;
using GGG.Tool.Singleton;
using UnityEngine.SceneManagement;

namespace Framework
{
    public class LoadSceneManager : Singleton<LoadSceneManager>
    {
  

        public void LoadSceneAsync(string sceneName,Action<float> onLoadProgressUpdateCallBack = null,Action onLoadFinishedCallBack = null)
        {
            //显示UI进度条   
            StartCoroutine(AsyncLoadScene(sceneName , onLoadProgressUpdateCallBack , onLoadFinishedCallBack));
        }
        
        /// <summary>
        /// 异步加载场景
        /// </summary>
        /// <param name="sceneName">目标场景名称</param>
        /// <param name="onLoadProgress">加载进度回调函数，每次更新加载进度时都会触发，且将加载进度作为参数传入</param>
        /// <param name="onLoadFinished">当场景加载完毕后，触发的回调函数</param>
        IEnumerator AsyncLoadScene(string sceneName, Action<float> onLoadProgress =null,Action onLoadFinished = null)
        {
            
            var ao = SceneManager.LoadSceneAsync(sceneName);
            ao.allowSceneActivation = false;

            float curProgress = 0f;
            
            float maxProgress = 100f;

            while (curProgress < 90)
            {
                curProgress = ao.progress * 100;
                onLoadProgress?.Invoke(curProgress);
                yield return null;
            }
            while (curProgress < maxProgress)
            {
                curProgress++;
                onLoadProgress?.Invoke(curProgress);
                yield return  null;
            }
            ao.allowSceneActivation = true;
            yield return null;
            onLoadFinished?.Invoke();
            
        }
    }
}