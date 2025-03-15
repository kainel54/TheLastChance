using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeballBossAppearState : BossState
{
    //에셋 구매 시 구현 
    public EyeballBossAppearState(Boss boss, BossStateMachine stateMachine, string animBoolName) : base(boss, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void UpdateState()
    {
        base.UpdateState();
        _stateMachine.ChangeState(BossEnum.Idle);
    }
}
