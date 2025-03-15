using System;
using UnityEngine;

public abstract class BossState
{
    protected Boss _boss;
    protected BossStateMachine _stateMachine;

    protected int _animBoolHash;
    protected bool _endTriggerCalled;

    protected event Action<bool> OnFunTriggerd;

    public BossState(Boss boss, BossStateMachine stateMachine, string animBoolName)
    {
        _boss = boss;
        _stateMachine = stateMachine;
        _animBoolHash = Animator.StringToHash(animBoolName);
    }
    public virtual void UpdateState()
    {

    }
    public virtual void Enter()
    {
        _boss.AnimatorCompo.SetBool(_animBoolHash, true);
        _endTriggerCalled = false;
    }
    public virtual void Exit()
    {
        _boss.AnimatorCompo.SetBool(_animBoolHash, false);
    }
    public void AnimationEndTrigger()
    {
        _endTriggerCalled = true;
    }

    public void FunctionTrigger(bool func)
    {
        OnFunTriggerd?.Invoke(func);
    }
}
