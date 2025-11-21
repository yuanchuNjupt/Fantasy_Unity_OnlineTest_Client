using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorMatchSMB : StateMachineBehaviour
{
    [SerializeField , Header("动画匹配信息")] private float _startTime;  //动画匹配开始匹配时间
    [SerializeField] private float _endTime;    //动画匹配结束匹配时间
    [SerializeField] private AvatarTarget _avatarTarget; //匹配的骨骼

    
    [SerializeField , Header("手动激活重力")] private bool _isEnabmleGravity; //是否提前启用重力
    [SerializeField] private float _enableTime;    //在哪一个时间点启用重力
    
    
    
    
    private Vector3 _matchPosition;
    private Quaternion _matchRotation;


    private void OnEnable()
    {
        GameEventManager.MainInstance.AddListener<Vector3 , Quaternion>("SetAnimatorMatchInfo" , GetMatchInfo);
    }

    private void OnDisable()
    {
        GameEventManager.MainInstance.RemoveListener<Vector3 , Quaternion>("SetAnimatorMatchInfo" , GetMatchInfo);
    }


    private void GetMatchInfo(Vector3 position, Quaternion rotation)
    {
        //从外部获取
        _matchPosition = position;
        _matchRotation = rotation;
        
        
    }




    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //如果没在匹配中
        if (!animator.isMatchingTarget && !animator.IsInTransition(0))
        {
            animator.MatchTarget(_matchPosition, _matchRotation, _avatarTarget , new MatchTargetWeightMask(Vector3.one , 0f) , _startTime , _endTime);
        }
        
        //如果需要提前启用重力
        if (_isEnabmleGravity)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > _enableTime)
            {
                //TODO:激活重力
                GameEventManager.MainInstance.CallEvent<bool>("EnableCharacterGravity" , true);
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
