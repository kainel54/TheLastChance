using DG.Tweening.Core.Easing;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public enum BossEnum
{
    Appear,
    Idle,
    Chase,
    Pattern0,
    Pattern1,
    Pattern2,
    Pattern3,
    Dead,
    ChanceCast,
    ChanceFail,
    CutScene
}

[Serializable]
public struct NeedBossPattern
{
    public float cooltime;
    public float damage;
    public UnityEvent OnPatternEvent;
}

public abstract class Boss : MonoBehaviour
{
    public UnityEvent FinalDeadEvent;
    public Animator AnimatorCompo { get; private set; }
    public bool CanStateChangeable { get; set; } = true;
    public int NowPatternIndex { get; private set; }
    public bool IsDead { get; private set; }
    public bool IsChance { get; private set; }
    [field : SerializeField] public Transform PlayerTrm { get; protected set; }
    [field: SerializeField] public List<NeedBossPattern> PatternList { get; protected set; }
    [field: SerializeField] public Clear_UI ClearUI { get; private set; }
    public BossStateMachine stateMachine;
    protected Transform _visualTrm;
    protected virtual void Awake()
    {
        //������ ���� �� ����
        /*MovementCompo = GetComponent<AgentMovement>();
        MovementCompo.Initalize(this);*/

        //������ ĳ���� ���� �� ����
        /*DamageCasterCompo = transform.Find("DamageCaster").GetComponent<DamageCaster>();
        _colliders = new Collider2D[1];*/

        _visualTrm = transform.Find("Visual");
        AnimatorCompo = _visualTrm.GetComponent<Animator>();

        //Health ���� �� ����
        /*HealthCompo = GetComponent<Health>();
        HealthCompo.Initialize(this);*/

        stateMachine = new BossStateMachine();
    }

    protected virtual void Update()
    {
        stateMachine.CurrentState.UpdateState();
    }

    public abstract void AnimationEndTrigger();

    public virtual void SetDead(bool value)
    {
        IsDead = value;
        CanStateChangeable = !value;
    }

    public abstract void SetDeadState();

    public abstract void OnChanceCast();

    public virtual void Attack()
    {
        //DamageCasterCompo.CastDamage(attackDamage, knokbackPower, 0.1f, false, true);
    }

    public virtual void SetPattern(int index) //0 = Pattern0, 1 = Pattern1, 2 = Pattern2, 3 = Pattern3
    {
        NowPatternIndex = index;

        stateMachine.ChangeState((BossEnum)index + 3); //���� ���������� 3���� �����̹Ƿ� 3�� ���Ѵ�.
    }

    public virtual void ChanceTrigger()
    {
        IsChance = true;
    }

    public abstract void FunctionTrigger(bool func);
}
