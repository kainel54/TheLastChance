using ObjectPooling;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Skill : MonoBehaviour
{
    public UnityEvent OnSkillEvent;

    protected RuntimeAnimatorController _skillAnimator = null;
    [field : SerializeField] public SkillDataSO SkillData { get; protected set; }
    protected Magician _magicianBoss;
    protected SkillEffect _skillEffect;

    public void Initialize(Magician magician)
    {
        _magicianBoss = magician;
    }

    public virtual void Play<T>() where T : Skill
    {
        _skillEffect = PoolManager.Instance.Pop(PoolingType.SkillEffect) as SkillEffect;
        Vector2 magicianSize = _magicianBoss.RendererCompo.transform.localScale;
        _skillEffect.PlayEffect<T>(transform.position, magicianSize);
    }

    public virtual void Stop<T>()
    {
        if(_skillEffect == null) return;

        _skillEffect.EndAnimaton();
    }
}
