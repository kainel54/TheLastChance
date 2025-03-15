using UnityEngine;

public class EyeballBossChanceFailState : BossState
{
    private EyeballBoss _eyeballBoss;
    public EyeballBossChanceFailState(Boss boss, BossStateMachine stateMachine, string animBoolName) : base(boss, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        _boss.SetDead(true);
        _boss.FinalDeadEvent?.Invoke();

        CameraManager.Instance.StopShake();
        CameraManager.Instance.ShakeCam(2f, 30f);

        _eyeballBoss ??= _boss as EyeballBoss;

        EyeballBulletPatternSO deadPattern = _eyeballBoss.OnChanceFailedBulletPattern;

        float angle = 0;
        float angleAdder = deadPattern.regularAngleAdder;
        bool isRegularAngle = !Mathf.Approximately(angleAdder, 0);

        for (int i = 0; i < deadPattern.speeds.Count; i++)
        {
            EyeballBullet bullet = PoolManager.Instance.Pop(ObjectPooling.PoolingType.EyeballBullet) as EyeballBullet;

            angle = isRegularAngle ? angle + angleAdder : deadPattern.angles[i];
            bullet.transform.eulerAngles = new Vector3(0, 0, angle +deadPattern.angleOffset);

            Vector2 firePos = new Vector2(_boss.transform.position.x - 0.14f, _boss.transform.position.y + 0.92f);

            bullet.SetData(_eyeballBoss.GetStageData(), deadPattern, i);
            bullet.InitializeAndFire(_boss.gameObject, firePos, bullet.transform.right);
        }
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }
}
