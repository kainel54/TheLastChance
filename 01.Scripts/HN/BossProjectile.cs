using ObjectPooling;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class BossProjectile : PoolableMono
{
    [SerializeField] protected float _moveSpeed;
    [SerializeField] private float _randomizeSize;

    protected GameObject _owner;
    protected Rigidbody2D _rigid;
    protected Vector3 _dir;

    protected virtual void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
    }

    public virtual void InitializeAndFire(GameObject owner, Vector2 position, Vector3 dir)
    {
        _owner = owner;
        transform.position = position;
        _dir = dir;

        if (_randomizeSize != 0)
        {
            float rand = Random.Range(_randomizeSize, -_randomizeSize);
            transform.localScale = transform.localScale + new Vector3(rand, rand, 1);
        }

        _rigid.velocity = Vector3.zero;
        OnFired();
    }

    protected abstract void OnFired();
}
