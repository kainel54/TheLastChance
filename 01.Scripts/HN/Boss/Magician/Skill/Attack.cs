using ObjectPooling;
using System;
using UnityEngine;

public class Attack : PoolableMono
{
    [SerializeField] protected float _yOffset;

    protected SkillDamageCaster _skillDamageCaster;
    protected Skill _skill;
    protected Magician _magicianBoss;
    protected Animator _anim;
    protected AnimatorInfoCaster _animCaster;
    protected SpriteRenderer _renderer;
    protected AttackAnimationTrigger _animationTrigger;
    protected Collider2D _collider;

    protected virtual void Awake()
    {
        _skillDamageCaster = GetComponent<SkillDamageCaster>();
        _collider = GetComponent<Collider2D>();

        Transform visual = transform.Find("Visual");

        _anim = visual.GetComponent<Animator>();
        _animCaster = new AnimatorInfoCaster(_anim);
        _renderer = visual.GetComponent<SpriteRenderer>();
        _animationTrigger = visual.GetComponent<AttackAnimationTrigger>();

        _animationTrigger.OnTriggerEvent += HandleAttackTrigger;
        _animationTrigger.OnSoundTriggerEvent += HandleSoundTrigger;
    }

    protected virtual void HandleSoundTrigger()
    {
    }

    private void OnDestroy()
    {
        _animationTrigger.OnTriggerEvent -= HandleAttackTrigger;
        _animationTrigger.OnSoundTriggerEvent -= HandleSoundTrigger;
    }

    protected virtual void HandleAttackTrigger()
    {
        _collider.enabled = !_collider.enabled;
    }

    public virtual void StartAttack(Skill skill, Magician boss, Vector2 pos)
    {
        _skill ??= skill;
        _magicianBoss ??= boss;
    }

    public override void ResetItem()
    {
        _collider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _skillDamageCaster.Cast();
    }
}
