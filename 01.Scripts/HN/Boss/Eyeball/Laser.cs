using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Laser : BossProjectile
{
    [SerializeField] private LayerMask _whatIsCollision;

    private LaserImpact _laserImpact;
    private RaycastHit2D _hitInfo;
    private Animator _anim;
    private SpriteRenderer _renderer;
    private bool _isLowOrder = true;
    private int _funcCount; //3, 6번째 트리거 때 레이저 위치 조정
    private readonly int _endHash = Animator.StringToHash("End");
    private AnimatorInfoCaster _animCaster;

    protected override void Awake()
    {
        base.Awake();

        Transform visual = transform.Find("Visual");
        _anim = visual.GetComponent<Animator>();
        _animCaster = new AnimatorInfoCaster(_anim);
        _renderer = visual.GetComponent<SpriteRenderer>();
    }

    public override void InitializeAndFire(GameObject owner, Vector2 position, Vector3 dir)
    {
        base.InitializeAndFire(owner, position, dir);
    }

    private void OnDisable()
    {
        _laserImpact = null;
        _funcCount = 0;
    }

    private void Update()
    {
        _hitInfo = Physics2D.Raycast(transform.position, -transform.up, 25f, _whatIsCollision);

        if (_laserImpact == null)
        {
            _laserImpact = PoolManager.Instance.Pop(ObjectPooling.PoolingType.LaserImpact) as LaserImpact;
        }

        _laserImpact.SetPosAndRotation(_hitInfo.point, transform.eulerAngles);

        float hitDistance = _hitInfo.distance / 2; //비주얼의 스케일 만큼 나누어 1:1의 비율을 맞춘다.
        Vector3 myScale = transform.localScale;
        transform.localScale = new Vector3(myScale.x, hitDistance, myScale.z);
    }

    public void AnimationEnd()
    {
        _anim.SetBool(_endHash, true);
        _laserImpact.End();

        DOVirtual.DelayedCall(0.01f, () =>
        {
            float length = _animCaster.GetAnimationClip().length - 0.1f;

            DOVirtual.DelayedCall(length, () => PoolManager.Instance.Push(this));
        });
    }

    public void SetSortingLayerAndPos(bool changeSortingLayer)
    {
        _funcCount++;
        if (_funcCount % 3 == 0)
        {
            transform.position = new Vector3(0, transform.position.y, 0);
            return;
        }

        if (changeSortingLayer)
        {
            int sortingOrder = _isLowOrder ? 11 : 9;
            SetSortingLayer(sortingOrder);
        }
        else transform.position += _isLowOrder ? Vector3.right : Vector3.left;

        _isLowOrder = !_isLowOrder;
    }
    public void SetSortingLayer(int sortingOrder) => _renderer.sortingOrder = sortingOrder;

    protected override void OnFired()
    {
    }

    public override void ResetItem()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if((1 << collision.gameObject.layer & 1 << 6) > 0) //6 : PlayerLayer
        {
            Player player = collision.GetComponent<Player>();
            player.GetCompo<PlayerHealth>().ApplyDamage(1);
        }
    }
}
