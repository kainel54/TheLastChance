using DG.Tweening;
using UnityEngine;

public class EyeballBossPattern1State : BossState
{
    private EyeballBoss _eyeballBoss;
    private Laser _laser;
    private float _angle;
    private float _rotateSpeed = 60;
    private readonly int _laserHash = Animator.StringToHash("LaserAttack");
    private bool _isCastEnd;
    private EyeBossStageSO _stageData;
    private AnimatorInfoCaster _animCaster;

    private float _currentTime;

    public EyeballBossPattern1State(Boss boss, BossStateMachine stateMachine, string animBoolName) : base(boss, stateMachine, animBoolName)
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

        _stageData = _eyeballBoss.GetStageData();
        _laser = PoolManager.Instance.Pop(ObjectPooling.PoolingType.Laser) as Laser;
        _laser.gameObject.SetActive(false);
        _laser.InitializeAndFire(_boss.gameObject, new Vector2(0, _boss.transform.position.y - 1f), Vector2.zero);
        _laser.transform.eulerAngles = Vector3.zero;
        _laser.transform.localScale = new Vector3(1, 1, 1);
        _laser.SetSortingLayer(11);

        _eyeballBoss.OnLaserChargeEvent?.Invoke();

        _angle = 0;

        //캐스팅 애니메이션의 길이 저장
        DOVirtual.DelayedCall(0.01f, () =>
        {
            float animationLength = _animCaster.GetAnimationClip().length - 0.08f;

            //레이저 쏘기
            DOVirtual.DelayedCall(animationLength, () =>
            {
                _eyeballBoss.OnLaserAttackEvent?.Invoke();

                _laser.gameObject.SetActive(true);
                CameraManager.Instance.ShakeCam(1.5f, 5.5f);

                _boss.AnimatorCompo.SetBool(_laserHash, true);
                _boss.AnimatorCompo.speed = _stageData.laserSpeed;

                _isCastEnd = true;
            });
        });

        OnFunTriggerd += _laser.SetSortingLayerAndPos;
    }


    public override void Exit()
    {
        base.Exit();

        _boss.AnimatorCompo.speed = 1;

        _isCastEnd = false;

        OnFunTriggerd -= _laser.SetSortingLayerAndPos;
        _laser = null;

        _currentTime = 0;
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (!_isCastEnd) return;

        Mathf.Clamp(_angle += Time.deltaTime * _rotateSpeed * _stageData.laserSpeed, 0, 360);

        _laser.transform.rotation = Quaternion.Euler(0, 0, _angle);

        _currentTime += Time.deltaTime;

        if (_angle >= 360)
        {
            _boss.AnimatorCompo.SetBool(_laserHash, false);
            _laser.AnimationEnd();
            _boss.SetPattern(_boss.NowPatternIndex + 1);

            _eyeballBoss.OnLaserEndEvent?.Invoke();
        }
        /*else if(_currentTime > _laserAttackTerm)
        {
            _eyeballBoss.OnLaserAttackEvent?.Invoke();
            _currentTime = 0;
        }*/
    }
}
