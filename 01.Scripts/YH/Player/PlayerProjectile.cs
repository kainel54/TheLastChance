using ObjectPooling;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerProjectile : PoolableMono
{
    [SerializeField] protected LayerMask _targetLayer;

    protected bool _isDead = false;
    protected float _timer = 0;

    protected Rigidbody2D _rigidBody;

    public abstract void InitAndFire(Transform firePosTrm);

    protected virtual void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }
}
