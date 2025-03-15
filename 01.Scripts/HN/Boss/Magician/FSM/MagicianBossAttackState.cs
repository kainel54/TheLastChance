using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicianBossAttackState : BossState
{
    private Magician _magicianBoss;

    public MagicianBossAttackState(Boss boss, BossStateMachine stateMachine, string animBoolName) : base(boss, stateMachine, animBoolName)
    {
        _magicianBoss = boss as Magician;
    }

    public override void Enter()
    {
        base.Enter();

        if (_magicianBoss.IsLastType() && _magicianBoss.IsChance)
        {
            _magicianBoss.stateMachine.ChangeState(BossEnum.Idle);
            return;
        }

        _magicianBoss.ActiveSkill(true);
    }
}
