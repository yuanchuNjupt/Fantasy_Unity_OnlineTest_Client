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