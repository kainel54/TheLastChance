using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindAttack : Attack
{
    [SerializeField] private float _speed;

    private Coroutine _coroutine;
    private Rigidbody2D _rigid;
    private float _saveSpeed;
    private readonly int _endAttackHash = Animator.StringToHash("EndAttack");

    protected override void Awake()
    {
        base.Awake();

        _rigid = GetComponent<Rigidbody2D>();
    }

    public override void StartAttack(Skill skill, Magician boss, Vector2 pos)
    {
        transform.position = new Vector2(pos.x, pos.y - 0.3f);

        base.StartAttack(skill, boss, pos);

        Vector2 playerPos = boss.PlayerTrm.position;

        Vector2 movePos = new Vector2(playerPos.x, playerPos.y + 1.5f);

        _saveSpeed = _speed;

        transform.DOMove(movePos, _speed);
    }

    private void OnDisable()
    {
        if(_coroutine != null)
            StopCoroutine(_coroutine);

        if(_saveSpeed != 0)
            _speed = _saveSpeed;
    }

    private IEnumerator PushCoroutine()
    {
        _anim.SetTrigger(_endAttackHash);

        yield return null;

        float length = _animCaster.GetAnimationClip().length;

        yield return new WaitForSeconds(length);

        PoolManager.Instance.Push(this);
    }

    public void ReturnTornado()
    {
        transform.DOMove(_magicianBoss.transform.position, _speed / 1.5f).SetEase(Ease.InCubic).
            OnComplete(() =>
            {
                _coroutine = StartCoroutine(PushCoroutine());
            });
    }
}
