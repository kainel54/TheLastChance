using UnityEngine;

public class EyeballBullet : BossProjectile
{
    private bool _isFired;
    private bool _isCrashed;
    private EyeBossStageSO _stageData;
    private EyeballBulletPatternSO _bulletPatternData;
    private int _bulletIndex;
    private Animator _anim;

    private readonly int _loopHash = Animator.StringToHash("Loop");
    private readonly int _impactHash = Animator.StringToHash("Impact");

    protected override void Awake()
    {
        base.Awake();

        _anim = GetComponent<Animator>();
    }

    public void SetData(EyeBossStageSO bossData, int patternIndex, int bulletIndex)
    {
        _stageData = bossData;
        _bulletIndex = bulletIndex;
        _bulletPatternData = _stageData.bulletPatterns[patternIndex];
    }

    public void SetData(EyeBossStageSO bossData, EyeballBulletPatternSO pattern, int bulletIndex)
    {
        _stageData = bossData;
        _bulletIndex = bulletIndex;
        _bulletPatternData = pattern;
    }

    public override void InitializeAndFire(GameObject owner, Vector2 position, Vector3 dir)
    {
        base.InitializeAndFire(owner, position, dir);

        transform.localScale = (Vector3)(_stageData.bulletSize * _bulletPatternData.sizeMultiplier);
    }

    private void Update()
    {
        if (!_isFired || _isCrashed) return;

        _rigid.velocity = _dir * _stageData.bulletSpeed * _bulletPatternData.speeds[_bulletIndex];
    }

    protected override void OnFired()
    {
        _isFired = true;
        _anim.SetBool(_loopHash, true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(_isCrashed) return;

        int collisionLayer = 1 << collision.gameObject.layer;
        if ((collisionLayer & 1 << _owner.layer) != 0 || collision as PolygonCollider2D) return;

        _isCrashed = true;
        _rigid.velocity = Vector2.zero;

        _anim.SetBool(_impactHash, true);
        _anim.SetBool(_loopHash, false);

        if(collision.gameObject.TryGetComponent(out Player player))
        {
            player.GetCompo<PlayerHealth>().ApplyDamage(1);
        }
    }

    public override void ResetItem()
    {
        _isFired = false;
        _isCrashed = false;
        _rigid.velocity = Vector2.zero;
        _anim.SetBool(_impactHash, false);
    }

    public void GoPool()
    {
        _anim.SetBool(_impactHash, false);
        PoolManager.Instance.Push(this);
    }
}
