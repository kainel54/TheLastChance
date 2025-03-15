using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneState : BossState
{
    public CutSceneState(Boss boss, BossStateMachine stateMachine, string animBoolName) : base(boss, stateMachine, animBoolName)
    {
    }
    private float _startTime;
    public override void Enter()
    {
        base.Enter();
        _startTime = Time.time;
    }
    public override void UpdateState()
    {
        base.UpdateState();
        if (_startTime + 5f < Time.time)
        {
            _stateMachine.ChangeState(BossEnum.Idle);
        }
    }
}
