using DG.Tweening;

public class MagicianBossWaitAttackState : BossState
{
    private Magician _magicianBoss;
    private Tween _moveTween;

    public MagicianBossWaitAttackState(Boss boss, BossStateMachine stateMachine, string animBoolName) : base(boss, stateMachine, animBoolName)
    {
        _magicianBoss = boss as Magician;
    }

    public override void Enter()
    {
        base.Enter();

        _magicianBoss.RendererCompo.Dissolve(true, MoveNextPos);
    }

    private void MoveNextPos()
    {
        _magicianBoss.OnMoveEvent?.Invoke();
        _moveTween = _magicianBoss.transform.DOMove(_magicianBoss.NowMagicianType.activePos, 0.8f).
            OnComplete(() =>
            {
                _magicianBoss.RendererCompo.Dissolve(false, () => _stateMachine.ChangeState(BossEnum.Pattern1));
            });
    }

    public override void Exit()
    {
        base.Exit();

        _moveTween?.Kill();
    }
}
