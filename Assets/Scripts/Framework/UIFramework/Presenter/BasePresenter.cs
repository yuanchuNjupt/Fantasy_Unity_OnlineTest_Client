using System;
using UIFramework.View;
using UnityEngine;

namespace UIFramework.Presenter
{
    public class BasePresenter<TView> : MonoBehaviour , IPresenter where TView : IView
    {
        private TView _view;

        public TView View
        {
            get
            {
                if (_view != null)
                {
                    return _view;
                }

                if (TryGetComponent(out TView view))
                {
                    _view = view;
                    return _view;
                }

                throw new Exception($"{this.gameObject}对象上不存在View组件,获取View失败");
            }
        }
        
        
        
        //不使用IOC框架
        //直接通过单例来访问
        //因此不添加获取服务的接口
        
        
        
        
        
    }
}