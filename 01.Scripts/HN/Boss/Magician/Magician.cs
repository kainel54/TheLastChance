using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public enum HeartType
{
    Wind,
    Dark,
    Lightning,
    Wind_Dark,
    Dark_Lightning,
    Wind_Lightning,
    Wind_Dark_Lightning
}

[Serializable]
public struct MagicianType
{
    public HeartType heartType;
    public List<Color> heartColors;
    public List<Skill> useSkills;
    public Vector2 activePos;

    public static bool operator !=(MagicianType x, MagicianType y) => !x.Equals(y);

    public static bool operator ==(MagicianType x, MagicianType y) => x.Equals(y);

    public override bool Equals(object obj)
    {
        if (!(obj is MagicianType)) return false;

        MagicianType other = (MagicianType)obj;

        return heartType == other.heartType &&
            heartColors == other.heartColors &&
            useSkills == other.useSkills &&
            activePos == other.activePos;
    }

    public override int GetHashCode()
    {
        int hash = heartType.GetHashCode();
        hash = (hash * 397) ^ (heartColors != null ? heartColors.Aggregate(0, (acc, color) => acc ^ color.GetHashCode()) : 0);
        hash = (hash * 397) ^ (useSkills != null ? useSkills.Aggregate(0, (acc, skill) => acc ^ skill.GetHashCode()) : 0);
        hash = (hash * 397) ^ activePos.GetHashCode();
        return hash;
    }
}

public class Magician : Boss
{
    public UnityEvent OnMoveEvent;

    [field: SerializeField] public Heart MagicianHeart { get; private set; }
    [field: SerializeField] public float ChanceCastTime { get; private set; }
    public MagicianRenderer RendererCompo { get; private set; }
    public MagicianType NowMagicianType { get; private set; }

    [SerializeField] private List<MagicianType> _magicianTypes = new List<MagicianType>();
    [SerializeField] private Transform _heartTrm;
    [SerializeField] private Player _player;

    public Player Player => _player;
    public bool IsAttack { get; private set; }

    private int _playerSortingOrder;
    private int _requireSkillEndCnt = 1;
    private int _currentSkillEndCnt;

    protected override void Awake()
    {
        base.Awake();

        RendererCompo = _visualTrm.GetComponent<MagicianRenderer>();
        RendererCompo.Initialize(this);
        RendererCompo.SpriteRenderer.color = new Color(1, 1, 1, 0);

        _playerSortingOrder = PlayerTrm.Find("Visual").GetComponent<SpriteRenderer>().sortingOrder;

        SkillManager.Instance.Initialize(this);

        stateMachine.AddState(BossEnum.Appear, new MagicianBossAppearState(this, stateMachine, "Idle"));
        stateMachine.AddState(BossEnum.Idle, new MagicianBossIdleState(this, stateMachine, "Idle"));
        stateMachine.AddState(BossEnum.Pattern0, new MagicianBossWaitAttackState(this, stateMachine, "Idle"));
        stateMachine.AddState(BossEnum.Pattern1, new MagicianBossAttackState(this, stateMachine, "Skill"));
        stateMachine.AddState(BossEnum.ChanceCast, new MagicianBossChanceCastState(this, stateMachine, "Idle"));
        stateMachine.AddState(BossEnum.Dead, new MagicianBossDeadState(this, stateMachine, "Dead"));

        stateMachine.Initialize(BossEnum.Idle, this);
    }

    private void Start()
    {
        stateMachine.AddState(BossEnum.ChanceFail, new MagicianBossChanceFailState(this, stateMachine, "Skill"));
    }

    public void HeartAppear()
    {
        MagicianHeart.Initiailze(this);
        MagicianHeart.ActiveHeart(new Vector2(0, -3), _magicianTypes[0], null);
    }

    public void Appear()
    {
        gameObject.SetActive(true);

        StartCoroutine("AppearCoroutine");
    }

    private IEnumerator AppearCoroutine()
    {
        yield return new WaitForSeconds(0.5f);

        stateMachine.ChangeState(BossEnum.Appear);
    }

    public override void AnimationEndTrigger()
    {
    }

    public override void SetDeadState()
    {
    }

    public void SetMagicianType(MagicianType type)
    {
        NowMagicianType = type;
    }

    public override void FunctionTrigger(bool func)
    {

    }

    public override void OnChanceCast()
    {

    }

    public void ActiveSkill(bool active)
    {
        if (active && IsChance) return;

        HashSet<Skill> skills = new HashSet<Skill>(NowMagicianType.useSkills);

        if (!active)
        {
            _currentSkillEndCnt++;

            if (_currentSkillEndCnt >= _requireSkillEndCnt) _currentSkillEndCnt = 0;
            else return;
        }

        foreach (Skill skill in skills)
        {
            switch (skill)
            {
                case WindSkill:
                    SkillManager.Instance.ActiveSkill<WindSkill>(active);
                    break;
                case LightningSkill:
                    SkillManager.Instance.ActiveSkill<LightningSkill>(active);
                    break;
                case DarknessSkill:
                    SkillManager.Instance.ActiveSkill<DarknessSkill>(active);
                    break;
                default:
                    Debug.LogWarning("스킬을 못찾음");
                    break;
            }
        }

        if (!active)
        {
            if (IsLastType())
            {
                stateMachine.ChangeState(BossEnum.ChanceCast);
                ChanceTrigger();
                return;
            }
            else
            {
                stateMachine.ChangeState(BossEnum.Pattern0);
            }
        }
        else
        {
            int heartIndex = (int)NowMagicianType.heartType;
            _requireSkillEndCnt = heartIndex / 3 + 1;
        }

        IsAttack = active;
    }

    public void IncreaseMagicianType() => SetMagicianType(_magicianTypes[(int)NowMagicianType.heartType + 1]);

    public void SetHeart(Skill owner) => MagicianHeart.ActiveHeart(_heartTrm.position, NowMagicianType, owner);

    public bool IsLastType() => _magicianTypes[_magicianTypes.Count - 1] == NowMagicianType;

    public void InitializePattern() => SetMagicianType(_magicianTypes[0]);
}
