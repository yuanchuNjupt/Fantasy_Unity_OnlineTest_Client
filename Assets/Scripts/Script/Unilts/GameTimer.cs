using System;
using UnityEngine;

public enum TimerState
{
    NOTWORKING,//没有工作
    WORKING,//正在工作
    DONE//工作完成
}


public class GameTimer
{
    //1.计算时长
    //2.计时结束后执行任务
    //3.当前计时器的状态
    //4.计时器的暂停与恢复
    
    private float _startTime;
    private Action _task;
    private bool _isStopTimer;
    private TimerState _timerState;

    public GameTimer()
    {
        ResetTimer();
    }
    
    //1.开始计时
    public void StartTimer(float time, Action task)
    {
        _startTime = time;
        _task = task;
        _isStopTimer = false;
        _timerState = TimerState.WORKING;
    }
    
    //2.更新计时器
    public void UpdateTimer()
    {
        if (_isStopTimer) return;

        _startTime -= Time.deltaTime;
        if (_startTime < 0f)
        {
            _task?.Invoke();
            _timerState = TimerState.DONE;
            _isStopTimer = true;
        }
    }
    
    //3.确定计时器的状态
    public TimerState GetTimerState() => _timerState;
    
    //4.重置计时器
    public void ResetTimer()
    {
        _startTime = 0f;
        _isStopTimer = true;
        _timerState = TimerState.NOTWORKING;
        _task = null;
    }
}
