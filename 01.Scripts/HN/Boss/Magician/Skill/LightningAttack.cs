using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningAttack : Attack
{
    private Vector2 _size;
    private Coroutine _coroutine;

    private LightningSkill _lightningSkill;

    protected override void Awake()
    {
        base.Awake();

        Transform visual = _anim.transform;
        _size = visual.localScale;
    }

    public override void StartAttack(Skill skill, Magician boss, Vector2 pos)
    {
        transform.position = new Vector2(pos.x, pos.y + 0.3f * _size.y);

        base.StartAttack(skill, boss, pos);

        _lightningSkill ??= _skill as LightningSkill;

        _coroutine = StartCoroutine(AttackCoroutine());
    }

    private IEnumerator AttackCoroutine()
    {
        yield return null;

        float animLength = _animCaster.GetAnimationClip().length - 0.01f;

        yield return new WaitForSeconds(animLength);

        PoolManager.Instance.Push(this);
    }

    protected override void HandleSoundTrigger()
    {
        base.HandleSoundTrigger();

        _lightningSkill.PlayImpactSound();
    }

    private void OnDisable()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);
    }
}
