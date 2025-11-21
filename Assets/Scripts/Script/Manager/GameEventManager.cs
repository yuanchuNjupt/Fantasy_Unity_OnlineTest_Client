using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GGG.Tool;
using GGG.Tool.Singleton;

public class GameEventManager : SingletonNonMono<GameEventManager>
{
    private interface IEventHelp
    {
        
    }
    //无参管理
    private class EventHelp : IEventHelp
    {
        private event Action _action;

        public EventHelp(Action action)
        {
            //第一次实例的时候 只会执行这一次
            _action = action;
        }
        
        
        //增加事件的注册函数
        public void AddCall(Action action)
        {
            _action += action;
        }
        
        //调用事件
        public void CallEvent()
        {
            _action?.Invoke();
        }
        
        //移除事件的注册函数
        public void Remove(Action action)
        {
            _action -= action;
        }
       
    }
    
    //有参管理 一个参数
    private class EventHelp<T>: IEventHelp
    {
        private event Action<T> _action;

        public EventHelp(Action<T> action)
        {
            //第一次实例的时候 只会执行这一次
            _action = action;
        }
        
        
        //增加事件的注册函数
        public void AddCall(Action<T> action)
        {
            _action += action;
        }
        
        //调用事件
        public void CallEvent(T value)
        {
            _action?.Invoke(value);
        }
        
        //移除事件的注册函数
        public void Remove(Action<T> action)
        {
            _action -= action;
        }
       
    }
    
    
    //有参管理 两个参数
    private class EventHelp<T1,T2>: IEventHelp
    {
        private event Action<T1,T2> _action;

        public EventHelp(Action<T1,T2> action)
        {
            //第一次实例的时候 只会执行这一次
            _action = action;
        }
        
        
        //增加事件的注册函数
        public void AddCall(Action<T1,T2> action)
        {
            _action += action;
        }
        
        //调用事件
        public void CallEvent(T1 value , T2 value2)
        {
            _action?.Invoke(value , value2);
        }
        
        //移除事件的注册函数
        public void Remove(Action<T1,T2> action)
        {
            _action -= action;
        }
       
    }
    
    private class EventHelp<T1, T2, T3> : IEventHelp
    {
        private event Action<T1, T2, T3> _action;

        public EventHelp(Action<T1, T2, T3> action)
        {
            //第一次实例的时候 只会执行这一次
            _action = action;
        }
        
        
        //增加事件的注册函数
        public void AddCall(Action<T1, T2, T3> action)
        {
            _action += action;
        }
        
        //调用事件
        public void CallEvent(T1 value , T2 value2 , T3 value3 )
        {
            _action?.Invoke(value , value2 , value3);
        }
        
        //移除事件的注册函数
        public void Remove(Action<T1, T2, T3> action)
        {
            _action -= action;
        }
    }
    

    private class EventHelp<T1, T2, T3, T4, T5> : IEventHelp
    {
        private event Action<T1, T2, T3, T4, T5> _action;

        public EventHelp(Action<T1, T2, T3, T4, T5> action)
        {
            //第一次实例的时候 只会执行这一次
            _action = action;
        }
        
        
        //增加事件的注册函数
        public void AddCall(Action<T1, T2, T3, T4, T5> action)
        {
            _action += action;
        }
        
        //调用事件
        public void CallEvent(T1 value , T2 value2 , T3 value3 , T4 value4 , T5 value5)
        {
            _action?.Invoke(value , value2 , value3 , value4 , value5);
        }
        
        //移除事件的注册函数
        public void Remove(Action<T1, T2, T3, T4, T5> action)
        {
            _action -= action;
        }
    }

    private Dictionary<string, IEventHelp> _eventCenter = new();
    
    //添加事件监听 无参
    public void AddListener(string actionName, Action action)
    {
        if (_eventCenter.TryGetValue(actionName, out IEventHelp eventHelp))
        {
            (eventHelp as EventHelp)?.AddCall(action);
        }
        else
        {
            //如果事件中心不存在这个名字的事件
            _eventCenter.Add(actionName, new EventHelp(action));
        }
    }
    
    //添加事件监听 有参重构
    public void AddListener<T>(string actionName, Action<T> action)
    {
        if (_eventCenter.TryGetValue(actionName, out IEventHelp eventHelp))
        {
            (eventHelp as EventHelp<T>)?.AddCall(action);
        }
        else
        {
            //如果事件中心不存在这个名字的事件
            _eventCenter.Add(actionName, new EventHelp<T>(action));
        }
    }
    
    public void AddListener<T1,T2>(string actionName, Action<T1,T2> action)
    {
        if (_eventCenter.TryGetValue(actionName, out IEventHelp eventHelp))
        {
            (eventHelp as EventHelp<T1,T2>)?.AddCall(action);
        }
        else
        {
            //如果事件中心不存在这个名字的事件
            _eventCenter.Add(actionName, new EventHelp<T1,T2>(action));
        }
    }
    
    public void AddListener<T1, T2, T3>(string actionName, Action<T1, T2, T3> action)
    {
        if (_eventCenter.TryGetValue(actionName, out IEventHelp eventHelp))
        {
            (eventHelp as EventHelp<T1, T2, T3>)?.AddCall(action);
        }
        else
        {
            //如果事件中心不存在这个名字的事件
            _eventCenter.Add(actionName, new EventHelp<T1, T2, T3>(action));
        }
    }

    public void AddListener<T1, T2, T3, T4, T5>(string actionName, Action<T1, T2, T3, T4, T5> action)
    {
        if (_eventCenter.TryGetValue(actionName, out IEventHelp eventHelp))
        {
            (eventHelp as EventHelp<T1, T2, T3, T4, T5>)?.AddCall(action);
        }
        else
        {
            //如果事件中心不存在这个名字的事件
            _eventCenter.Add(actionName, new EventHelp<T1, T2, T3, T4, T5>(action));
        }
    }
    
    //调用事件 无参
    public void CallEvent(string actionName)
    {
        if (_eventCenter.TryGetValue(actionName, out IEventHelp eventHelp))
        {
            (eventHelp as EventHelp)?.CallEvent();
        }
        else
        {
            Debug.Log("当前未注册事件: " + actionName);
        }
    }
    
    //调用事件 有参
    public void CallEvent<T>(string actionName, T value)
    {
        if (_eventCenter.TryGetValue(actionName, out IEventHelp eventHelp))
        {
            (eventHelp as EventHelp<T>)?.CallEvent(value);
        }
        else
        {
            Debug.Log("当前未注册事件: " + actionName);
        }
    }
    
    public void CallEvent<T1,T2>(string actionName, T1 value, T2 value2)
    {
        if (_eventCenter.TryGetValue(actionName, out IEventHelp eventHelp))
        {
            (eventHelp as EventHelp<T1,T2>)?.CallEvent(value, value2);
        }
        else
        {
            Debug.Log("当前未注册事件: " + actionName);
        }
    }
    
    public void CallEvent<T1, T2, T3>(string actionName, T1 value, T2 value2, T3 value3)
    {
        if (_eventCenter.TryGetValue(actionName, out IEventHelp eventHelp))
        {
            (eventHelp as EventHelp<T1, T2, T3>)?.CallEvent(value, value2 , value3);
        }
        else
        {
            Debug.Log("当前未注册事件: " + actionName);
        }
    }

    public void CallEvent<T1, T2, T3, T4, T5>(string actionName, T1 value, T2 value2, T3 value3, T4 value4, T5 value5)
    {
        if (_eventCenter.TryGetValue(actionName, out IEventHelp eventHelp))
        {
            (eventHelp as EventHelp<T1, T2, T3, T4, T5>)?.CallEvent(value, value2 , value3 , value4 , value5);
        }
        else
        {
            Debug.Log("当前未注册事件: " + actionName);
        }
    }
    
    //移除事件监听 无参
    public void RemoveListener(string actionName, Action action)
    {
        if (_eventCenter.TryGetValue(actionName, out IEventHelp eventHelp))
        {
            (eventHelp as EventHelp)?.Remove(action);
        }
        else
        {
            Debug.Log("当前未注册事件: " + actionName);
        }
    }
    
    //移除事件监听 有参
    public void RemoveListener<T>(string actionName, Action<T> action)
    {
        if (_eventCenter.TryGetValue(actionName, out IEventHelp eventHelp))
        {
            (eventHelp as EventHelp<T>)?.Remove(action);
        }
        else
        {
            Debug.Log("当前未注册事件: " + actionName);
        }
    }
    
    public void RemoveListener<T1,T2>(string actionName, Action<T1,T2> action)
    {
        if (_eventCenter.TryGetValue(actionName, out IEventHelp eventHelp))
        {
            (eventHelp as EventHelp<T1,T2>)?.Remove(action);
        }
        else
        {
            Debug.Log("当前未注册事件: " + actionName);
        }
    }
    
    public void RemoveListener<T1, T2, T3>(string actionName, Action<T1, T2, T3> action)
    {
        if (_eventCenter.TryGetValue(actionName, out IEventHelp eventHelp))
        {
            (eventHelp as EventHelp<T1, T2, T3>)?.Remove(action);
        }
        else
        {
            Debug.Log("当前未注册事件: " + actionName);
        }
    }

    public void RemoveListener<T1, T2, T3, T4, T5>(string actionName, Action<T1, T2, T3, T4, T5> action)
    {
        if (_eventCenter.TryGetValue(actionName, out IEventHelp eventHelp))
        {
            (eventHelp as EventHelp<T1, T2, T3, T4, T5>)?.Remove(action);
        }
        else
        {
            Debug.Log("当前未注册事件: " + actionName);
        }
    }
    
    
}
