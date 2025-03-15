using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class EyeballBossChanceState : BossState
{
    private EyeballBoss _eyeballBoss;
    private ChanceInvoker _chanceInvoker;
    private CinemachineVirtualCamera _bossCam;

    public EyeballBossChanceState(Boss boss, BossStateMachine stateMachine, string animBoolName) : base(boss, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        _eyeballBoss ??= _boss as EyeballBoss;
        _bossCam ??= _eyeballBoss.BossCam;

        _chanceInvoker = new ChanceInvoker(_eyeballBoss.ChanceTime, OnChanceSuccess, OnChanceFail);

        _eyeballBoss.OnChanceSuccessEvent += SetSuccess;
    }

    public override void Exit()
    {
        base.Exit();

        _eyeballBoss.OnChanceSuccessEvent -= SetSuccess;
    }

    private void SetSuccess() => _chanceInvoker.SetSuccessOrNot(true);

    private void OnChanceSuccess()
    {
        _eyeballBoss.RendererCompo.color = Color.white;
        _eyeballBoss.EyeLight.gameObject.SetActive(false);

        CinemachineVirtualCamera playerCam = CameraManager.Instance.VCam;

        CameraManager.Instance.SetCam(_bossCam);

        CameraManager.Instance.StopShake();
        CameraManager.Instance.ShakeCam(2f, 15f, () =>
        {
            CameraManager.Instance.StopShake();
            _eyeballBoss.SetBossCamPriority(9);
            CameraManager.Instance.SetCam(playerCam);
        });

        _stateMachine.ChangeState(BossEnum.Dead);
    }

    private void OnChanceFail()
    {
        _eyeballBoss.EyeLight.gameObject.SetActive(false);
        CameraManager.Instance.SetCam(_bossCam);
        _stateMachine.ChangeState(BossEnum.ChanceFail);
    }
}
