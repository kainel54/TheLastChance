using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicianBossChanceFailState : BossState
{
    private Magician _magicianBoss;
    private PlayerMovement _playerMovement;

    public MagicianBossChanceFailState(Boss boss, BossStateMachine stateMachine, string animBoolName) : base(boss, stateMachine, animBoolName)
    {
        _magicianBoss = boss as Magician;
        _playerMovement = _magicianBoss.Player.GetCompo<PlayerMovement>();
    }

    public override void Enter()
    {
        base.Enter();

        _playerMovement.CanMove = false;
        _playerMovement.IsStopedByBossOrDead = true;
        _magicianBoss.Player.RigidCompo.velocity = Vector2.zero;

        _magicianBoss.MagicianHeart.DisableHeat(_magicianBoss.MagicianHeart.Owner);

        DOVirtual.DelayedCall(0.2f, () => SkillManager.Instance.ActiveSkill<DeadSkill>(true));
    }
}
