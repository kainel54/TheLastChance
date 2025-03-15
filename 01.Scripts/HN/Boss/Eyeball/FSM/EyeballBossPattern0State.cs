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

        //ĳ���� �ִϸ��̼��� ���� ����
        DOVirtual.DelayedCall(0.01f, () =>
        {
            float animationLength = _animCaster.GetAnimationClip().length;

            //ĳ���� �ִϸ��̼��� ���� �� �Ѿ� �߻� ����
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
