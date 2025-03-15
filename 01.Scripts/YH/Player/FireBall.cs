 using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;

public class FireBall : PlayerProjectile
{
    [SerializeField] private float _moveSpeed = 15f;
    [SerializeField] private float _lifeTime = 2f;
    [SerializeField] private float _triggerTime = 0.1f;
    private bool _isTrigger;
    
    private Vector2 _fireDirection;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void InitAndFire(Transform firePosTrm)
    {
        transform.SetPositionAndRotation(firePosTrm.position, firePosTrm.rotation);
        _fireDirection = firePosTrm.right;
        //날라가는 사운드
    }

    private void FixedUpdate()
    {
        _rigidBody.velocity = _fireDirection * _moveSpeed;
        _timer += Time.fixedDeltaTime;

        if (_timer >= _triggerTime)
        {
            _isTrigger = true;
        }

        if (_timer >= _lifeTime)
        {
            _isDead = true;
            DestroyBullet();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_isDead) return;
        if (!_isTrigger) return;
        _isDead = true;
        DestroyBullet();
    }

    private void DestroyBullet()
    {
        PoolManager.Instance.Push(this);
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public override void ResetItem()
    {
        _isTrigger = false;
        _isDead = false;
        _timer = 0;
    }
}
