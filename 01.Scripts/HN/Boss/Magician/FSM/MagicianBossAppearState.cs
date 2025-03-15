using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicianBossAppearState : BossState
{
    private Magician _magicianBoss;
    private MagicianRenderer _magicianRenderer;

    public MagicianBossAppearState(Boss boss, BossStateMachine stateMachine, string animBoolName) : base(boss, stateMachine, animBoolName)
    {
        _magicianBoss = boss as Magician;
        _magicianRenderer = _magicianBoss.RendererCompo;
    }

    public override void Enter()
    {
        base.Enter();

        _magicianRenderer.SpriteRenderer.DOColor(Color.white, 0.8f);
        _magicianRenderer.Dissolve(true, OnDissovled);
    }

    private void OnDissovled()
    {
        _magicianBoss.InitializePattern();

        _magicianBoss.stateMachine.ChangeState(BossEnum.Pattern0);
        _magicianRenderer.Dissolve(false, null);
    }
}
