using System.Collections;
using UnityEngine;

public class DeadAttack : Attack
{
    public override void StartAttack(Skill skill, Magician boss, Vector2 pos)
    {
        base.StartAttack(skill, boss, pos);

        transform.position = pos;

        StartCoroutine(PushCoroutine());
    }

    private IEnumerator PushCoroutine()
    {
        yield return null;

        float length = _animCaster.GetAnimationClip().length;

        yield return new WaitForSeconds(length);

        PoolManager.Instance.Push(this);
    }
}
