using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicianBossDeadState : BossState
{
    public MagicianBossDeadState(Boss boss, BossStateMachine stateMachine, string animBoolName) : base(boss, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _boss.ClearUI.Clear();
        _boss.SetDead(true);
        _boss.FinalDeadEvent?.Invoke();
    }
}
