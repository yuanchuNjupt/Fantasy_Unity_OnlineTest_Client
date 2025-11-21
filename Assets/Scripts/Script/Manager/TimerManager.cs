using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GGG.Tool.Singleton;
using UnityEngine;

public class TimerManager : Singleton<TimerManager>
{
    //note : 开始的时候 先创建一些计时器 不然 空闲计时器 一个也没有
    //1.有一个集合用来保存所有的空闲计时器
    //2.有一个集合用来保存当前正在工作的计时器
    //3.更新当前工作中的计时器
    //4.当某个计时器工作完毕后 我们将他回收在空闲计时器的集合中
    
    
    //默认创建多少个计时器  20
    [SerializeField] private int _initMaxTimerCount = 20;
    
    
    private Queue<GameTimer> _freeTimers = new Queue<GameTimer>();
    private List<GameTimer> _workingTimers = new List<GameTimer>();


    private void Start()
    {
        InitTimerManager();
    }

    private void Update()
    {
        UpdateWorkingTimers();
    }


    private void InitTimerManager()
    {
        for (int i = 0; i < _initMaxTimerCount; i++)
        {
            CreateTimer();
        }
    }

    private void CreateTimer()
    {
        var timer = new GameTimer();
        _freeTimers.Enqueue(timer);
    }

    public void TryGetOneTimer(float time, Action task)
    {
        if (_freeTimers.Count == 0)
            CreateTimer();
        
        //然后让他去工作
        var timer = _freeTimers.Dequeue();
        timer.StartTimer(time , task);
        _workingTimers.Add(timer);
       
    }


    private void UpdateWorkingTimers()
    {
        if(_workingTimers.Count == 0) return;
        for (var i = 0; i < _workingTimers.Count; i++)
        {
            //正在进行任务
            if (_workingTimers[i].GetTimerState() == TimerState.WORKING)
            {
                _workingTimers[i].UpdateTimer();
            }
            else
            {
                _freeTimers.Enqueue(_workingTimers[i]);
                _workingTimers[i].ResetTimer();
                _workingTimers.RemoveAt(i);
                
                
            }
        }
    }
}
