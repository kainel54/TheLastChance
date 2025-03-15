using Cinemachine;
using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

public class EyeballBoss : Boss
{
    public UnityEvent OnBulletFireEvent;
    public UnityEvent OnLaserChargeEvent;
    public UnityEvent OnLaserAttackEvent;
    public UnityEvent OnLaserEndEvent;

    public event Action OnEyeDamagedEvent;
    public event Action OnChanceSuccessEvent;

    public SpriteRenderer RendererCompo { get; private set; }

    [SerializeField] private List<Eye> _eyes = new List<Eye>();
    [SerializeField] private float _appearDelay;
    [SerializeField] private Vector2 _playerCheckOffset;
    [SerializeField] private FeedbackPlayer _laserAttackFeedback;
    [SerializeField] private SoundFeedback _startFeedback;

    [Header("Chance Setting")]
    [SerializeField] private CinemachineVirtualCamera _bossCenterCam;
    [SerializeField] private Light2D _globalLight;
    [field : SerializeField] public Light2D EyeLight { get; private set; }
    [field: SerializeField] public float ChanceTime { get; private set; }
    [SerializeField] private float _camImpulsePower;
    

    public CinemachineVirtualCamera BossCam => _bossCenterCam;

    [field: SerializeField] public List<EyeBossStageSO> StageData { get; private set; } = new();
    [field: SerializeField] public EyeballBulletPatternSO OnChanceFailedBulletPattern { get; private set; }

    private readonly int _idleXHash = Animator.StringToHash("DirX");
    private readonly int _idleYHash = Animator.StringToHash("DirY");

    protected override void Awake()
    {
        base.Awake();

        stateMachine.AddState(BossEnum.Idle, new BossIdleState(this, stateMachine, "Idle"));
        stateMachine.AddState(BossEnum.Pattern0, new EyeballBossPattern0State(this, stateMachine, "BulletAttackCast"));
        stateMachine.AddState(BossEnum.Pattern1, new EyeballBossPattern1State(this, stateMachine, "LaserCast"));
        stateMachine.AddState(BossEnum.Pattern2, new EyeballBossPattern2State(this, stateMachine, "Blind"));
        stateMachine.AddState(BossEnum.Dead, new EyeballBossDeadState(this, stateMachine, "Dead"));
        stateMachine.AddState(BossEnum.ChanceCast, new EyeballBossChanceState(this, stateMachine, "ChanceCast"));
        stateMachine.AddState(BossEnum.ChanceFail, new EyeballBossChanceFailState(this, stateMachine, "Dead"));
        stateMachine.AddState(BossEnum.CutScene, new CutSceneState(this, stateMachine, "Idle"));

        stateMachine.Initialize(BossEnum.CutScene, this);

        RendererCompo = transform.Find("Visual").GetComponent<SpriteRenderer>();

        OnLaserAttackEvent.AddListener(_laserAttackFeedback.PlayFeedback);
        OnLaserEndEvent.AddListener(_laserAttackFeedback.FinishFeedback);

        _eyes.ForEach(e => e.Initialize(this));
    }

    private void Start()
    {
        _startFeedback.CreateFeedback();
    }

    private void OnDestroy()
    {
        OnLaserAttackEvent.RemoveListener(_laserAttackFeedback.PlayFeedback);
        OnLaserEndEvent.RemoveListener(_laserAttackFeedback.FinishFeedback);
    }

    protected override void Update()
    {
        base.Update();

        if (PlayerTrm == null) return;

        float x = 0, y = 0;

        if (transform.position.x + _playerCheckOffset.x < PlayerTrm.position.x) x = 1;
        else if (transform.position.x - _playerCheckOffset.x > PlayerTrm.position.x) x = -1;

        if (transform.position.y + _playerCheckOffset.y < PlayerTrm.position.y) y = 1;
        else if (transform.position.y - _playerCheckOffset.y > PlayerTrm.position.y) y = -1;

        AnimatorCompo.SetFloat(_idleXHash, x);
        AnimatorCompo.SetFloat(_idleYHash, y);
    }
    public override void AnimationEndTrigger()
    {
        stateMachine.CurrentState.AnimationEndTrigger();
    }

    public override void SetDead(bool value)
    {
        base.SetDead(value);

        _startFeedback.StopSound();
    }

    public override void Attack()
    {
        base.Attack();
    }
    public void SetEyes(bool open)
    {
        foreach (Eye eye in _eyes)
            eye.SetEyeValue(open);

        if (!open) OnEyeDamagedEvent?.Invoke();
        else CameraManager.Instance.ShakeCam(1f, 3.5f);
    }

    public bool AllEyesDamaged()
    {
        foreach (Eye eye in _eyes)
        {
            if (!eye.IsDamaged) return false;
        }

        return true;
    }

    public EyeBossStageSO GetStageData() => StageData[CheckClosedEyes()];

    public int CheckClosedEyes()
    {
        int cnt = 0;

        foreach (Eye eye in _eyes)
        {
            if (eye.IsDamaged) cnt++;
        }

        return cnt;
    }

    public override void FunctionTrigger(bool func)
    {
        stateMachine.CurrentState.FunctionTrigger(func);
    }

    public override void OnChanceCast()
    {
        SetBossCamPriority(11);
        EyeLight.gameObject.SetActive(true);
        _globalLight.color = new Color(1, 0.3f, 0.3f);

        RendererCompo.DOColor(Color.red, ChanceTime);

        stateMachine.ChangeState(BossEnum.ChanceCast);
        CameraManager.Instance.ShakeCam(ChanceTime - 1, _camImpulsePower);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.TryGetComponent(out FireBall bullet)) OnChanceSuccessEvent?.Invoke();
    }

    public void SetBossCamPriority(int value) => _bossCenterCam.Priority = value;

    public override void SetDeadState()
    {

    }
}
