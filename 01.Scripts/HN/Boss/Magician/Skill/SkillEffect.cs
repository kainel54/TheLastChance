using DG.Tweening;
using ObjectPooling;
using System;
using System.Collections;
using UnityEngine;

public class SkillEffect : PoolableMono
{
    private Animator _animator;
    private readonly int _endHash = Animator.StringToHash("EndSkill");
    private Coroutine _pushCoroutine;
    private AnimatorInfoCaster _animCaster;
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _animCaster = new AnimatorInfoCaster(_animator);
    }

    public override void ResetItem()
    {
    }

    public void PlayEffect<T>(Vector2 pos, Vector2 size) where T : Skill
    {
        transform.position = pos;
        transform.localScale = size;

        SkillDataSO data = SkillManager.Instance.GetData<T>();
        
        if(_animator.runtimeAnimatorController != data.animatorController)
        {
            _animator.runtimeAnimatorController = data.animatorController;
        }
    }

    private void OnDisable()
    {
        if (_pushCoroutine != null)
            StopCoroutine(_pushCoroutine);
    }

    public void EndAnimaton()
    {
        _animator.SetTrigger(_endHash);

        _pushCoroutine = StartCoroutine(PushCoroutine());
    }

    private IEnumerator PushCoroutine()
    {
        yield return null;

        float length = _animCaster.GetAnimationClip().length - 0.01f;

        yield return new WaitForSeconds(length);

        PoolManager.Instance.Push(this);
    }
}
