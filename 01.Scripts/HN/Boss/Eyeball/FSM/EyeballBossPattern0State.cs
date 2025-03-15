using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class EyeballBossPattern0State : EyeballBulletFireState
{
    private bool _isCast;
    private AnimatorInfoCaster _animCaster;

    private readonly int _bulletAttackHash = Animator.StringToHash("BulletAttack");
    public EyeballBossPattern0State(Boss boss, BossStateMachine stateMachine, string animBoolName) : base(boss, stateMachine, animBoolName)
    {
        _animCaster = new AnimatorInfoCaster(boss.AnimatorCompo);
    }

    public override void Enter()
    {
        base.Enter();

        //캐스팅 애니메이션의 길이 저장
        DOVirtual.DelayedCall(0.01f, () =>
        {
            float animationLength = _animCaster.GetAnimationClip().length;

            //캐스팅 애니메이션이 끝난 후 총알 발사 시작
            DOVirtual.DelayedCall(animationLength, () =>
            {
                _isCast = true;
                _boss.AnimatorCompo.SetBool(_bulletAttackHash, true);
            });
        });
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (!_isCast) return;

        SpawnBulletAndFire();

        if (_endTriggerCalled)
            _stateMachine.ChangeState(BossEnum.Idle);
    }

    protected override void OnPatternEnded() => _boss.AnimatorCompo.SetBool(_bulletAttackHash, false);
}
