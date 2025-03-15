using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class MagicianBossChanceCastState : BossState
{
    private Light2D _globalLight;
    private Magician _magicianBoss;
    private ChanceInvoker _chanceInvoker;

    public MagicianBossChanceCastState(Boss boss, BossStateMachine stateMachine, string animBoolName) : base(boss, stateMachine, animBoolName)
    {
        _magicianBoss = boss as Magician;
        _globalLight = GameObject.FindGameObjectWithTag("GlobalLight").GetComponent<Light2D>();
    }

    public override void Enter()
    {
        base.Enter();

        _magicianBoss.MagicianHeart.OnLastHeartDisableEvent += HandleChanceSuccess;

        CameraManager.Instance.ShakeCam(1f, 2.5f);

        DOTween.To(() => _globalLight.intensity, x => _globalLight.intensity = x, 0.25f, 0.5f);
        _magicianBoss.MagicianHeart.SetLight(true);

        _chanceInvoker = new ChanceInvoker(_magicianBoss.ChanceCastTime, () =>
        {
            _stateMachine.ChangeState(BossEnum.Dead);
        }, 
        () =>
        {
            _stateMachine.ChangeState(BossEnum.ChanceFail);
        });
    }

    public override void Exit()
    {
        base.Exit();

        _magicianBoss.MagicianHeart.OnLastHeartDisableEvent -= HandleChanceSuccess;
    }

    private void HandleChanceSuccess()
    {
        _globalLight.intensity = 1;
        _chanceInvoker.SetSuccessOrNot(true);
        _magicianBoss.MagicianHeart.SetLight(false);
    }
}
