using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeballBossPattern2State : EyeballBulletFireState
{
    private float _enterTime;
    private float _lastLaserTime;
    private bool _isLaserAttack;
    private bool _isBulletAttack;
    private bool _isFireTrigger;
    private bool _isEyeDamaged;
    private AnimatorInfoCaster _animCaster;
    private bool _isBlindEnd;

    private int _laserIdleHash = Animator.StringToHash("Blind");
    private int _laserCastHash = Animator.StringToHash("BlindLaserCast");
    public EyeballBossPattern2State(Boss boss, BossStateMachine stateMachine, string animBoolName) : base(boss, stateMachine, animBoolName)
    {
        _animCaster = new AnimatorInfoCaster(boss.AnimatorCompo);
    }

    public override void Enter()
    {
        base.Enter();

        if (_eyeballBoss == null)
        {
            _eyeballBoss = _boss as EyeballBoss;
        }

        _enterTime = Time.time;
        _lastLaserTime = Time.time;
        _isBulletAttack = true;
        _isFireTrigger = false;
        _isEyeDamaged = false;
        _isBlindEnd = false;

        _eyeballBoss.OnEyeDamagedEvent += () => _isEyeDamaged = true;

        DOVirtual.DelayedCall(_stageData.blindTime - 1f, () => _eyeballBoss.SetEyes(true));
    }

    public override void Exit()
    {
        base.Exit();

        _eyeballBoss.OnEyeDamagedEvent -= () => _isEyeDamaged = true;
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (_isBlindEnd) return;

        if(_isFireTrigger) SpawnBulletAndFire();

        //만약 눈알이 파괴되었다면
        if (_isEyeDamaged && !_eyeballBoss.AllEyesDamaged())
        {
            CameraManager.Instance.ShakeCam(2f, 7f);
            _stateMachine.ChangeState(BossEnum.Idle);
        }

        //블라인드 시간 끝나면 상태 넘기기
        if (_enterTime + _stageData.blindTime < Time.time && !_isLaserAttack && !_isBulletAttack)
        {
            _isBlindEnd = true;
            
            DOVirtual.DelayedCall(2f, () =>
            {
                if (_eyeballBoss.AllEyesDamaged()) return;

                _eyeballBoss.SetEyes(false);
                _stateMachine.ChangeState(BossEnum.Idle);
            });
        }
        //레이저 발사
        else if(_lastLaserTime + _stageData.blindLaserTerm < Time.time && !_isLaserAttack)
        {
            _isLaserAttack = true;

            _boss.AnimatorCompo.SetBool(_laserCastHash, true);

            //한프레임 기다리고 캐스트 애니메이션의 길이만큼 기다린 후 레이저 발사
            DOVirtual.DelayedCall(0.01f, () =>
            {
                float length = _animCaster.GetAnimationClip().length - 0.08f;
                _eyeballBoss.OnLaserChargeEvent?.Invoke();

                DOVirtual.DelayedCall(length, () =>
                {
                    _eyeballBoss.OnLaserAttackEvent?.Invoke();

                    _isFireTrigger = true;

                    //애니메이션 && 레이저 발사 구현하기
                    _boss.AnimatorCompo.SetBool(_laserCastHash, false);
                    int laserCnt = _stageData.blindLaserCount;
                    float divideAngle = 360 / laserCnt;

                    List<Laser> lasers = new List<Laser>();

                    for (int i = 0; i < laserCnt; i++)
                    {
                        Laser laser = PoolManager.Instance.Pop(ObjectPooling.PoolingType.Laser) as Laser;
                        laser.InitializeAndFire(_boss.gameObject, new Vector2(0, 0.8f), Vector2.zero);
                        laser.transform.rotation = Quaternion.Euler(new Vector3(0, 0, divideAngle * i));
                        laser.transform.localScale = new Vector3(1.5f, 1, 1);

                        int sortingOrder = i == 0 ? 11 : 9;
                        laser.SetSortingLayer(sortingOrder);

                        lasers.Add(laser);
                    }

                    CameraManager.Instance.ShakeCam(1.5f, 6f);

                    DOVirtual.DelayedCall(0.8f, () =>
                    {
                        foreach (Laser laser in lasers) laser.AnimationEnd();

                        _eyeballBoss.OnLaserEndEvent?.Invoke();

                        //레이저 End애니메이션 시간 고려
                        DOVirtual.DelayedCall(0.5f, () =>
                        {
                            _isLaserAttack = false;
                            _lastLaserTime = Time.time;
                        });
                    });
                });
            });
        }
    }

    protected override void OnPatternEnded() => _isBulletAttack = false;
}
