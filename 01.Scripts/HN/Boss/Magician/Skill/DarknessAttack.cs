using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarknessAttack : Attack
{
    [SerializeField] private FeedbackPlayer _onSoundFeedback;

    private Coroutine _coroutine;

    private void OnDisable()
    {
        if(_coroutine != null )
            StopCoroutine(_coroutine);
    }

    public override void StartAttack(Skill skill, Magician boss, Vector2 pos)
    {
        base.StartAttack(skill, boss, pos);

        transform.position = pos;

        _coroutine = StartCoroutine(PushCoroutine());
    }

    private IEnumerator PushCoroutine()
    {
        yield return null;

        float length = _animCaster.GetAnimationClip().length - 0.01f;

        yield return new WaitForSeconds(length);

        PoolManager.Instance.Push(this);
    }

    protected override void HandleAttackTrigger()
    {
        base.HandleAttackTrigger();

        CameraManager.Instance.StopShake();
        CameraManager.Instance.ShakeCam(0.5f, 0.8f);
    }

    protected override void HandleSoundTrigger() => _onSoundFeedback.PlayFeedback();
}
