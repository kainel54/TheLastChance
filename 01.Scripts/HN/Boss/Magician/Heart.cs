using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

public class Heart : MonoBehaviour
{
    public UnityEvent OnFirstHeartDamagedEvent;
    public UnityEvent OnHeartDamagedEvent;
    public event Action OnLastHeartDisableEvent;

    public Skill Owner => _owner;

    private Skill _owner;
    private HeartEffect _effect;
    private Magician _magicianBoss;
    private Transform _target;
    private bool _isMoveToTarget;
    private Collider2D _collider;
    private bool _isIncType;
    private Light2D _light;
    private bool _isInitialize;

    public void Initiailze(Magician boss)
    {
        _magicianBoss = boss;

        _effect = GetComponent<HeartEffect>();

        _collider = GetComponent<Collider2D>();
        _collider.enabled = false;

        _light = GetComponentInChildren<Light2D>();
        _light.enabled = false;

        _effect.Initialize();
    }

    public void ActiveHeart(Vector2 pos, MagicianType bossType, Skill owner)
    {
        if (_owner != null) return;

        _owner = owner;
        transform.position = pos;
        _effect.Play(pos, bossType.heartColors[0]);
        _effect.SetColors(bossType.heartColors);
        _collider.enabled = true;
    }

    private void Update()
    {
        MoveToTarget();
        IncreaseMagicianType();
    }

    private void IncreaseMagicianType()
    {
        if (!_isIncType || _magicianBoss.IsAttack) return;

        _magicianBoss.IncreaseMagicianType();
        _isIncType = false;
    }

    private void MoveToTarget()
    {
        if (!_isMoveToTarget || _target == null) return;

        transform.position = _target.position;
    }

    public void SetTarget(bool moveToTarget, Transform target, Skill owner)
    {
        if (_owner != owner) return;

        _isMoveToTarget = moveToTarget;
        _target = moveToTarget ? target : null;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out FireBall bullet))
        {
            CameraManager.Instance.StopShake();
            CameraManager.Instance.ShakeCam(1.5f, 5f);

            OnHeartDamagedEvent?.Invoke();

            if (!_isInitialize)
            {
                _isInitialize = true;
                DisableHeat(_owner);
                _magicianBoss.Appear();
                OnFirstHeartDamagedEvent?.Invoke();
                return;
            }

            if (_magicianBoss.stateMachine.CurrentState.GetType() == typeof(MagicianBossChanceCastState))
            {
                OnLastHeartDisableEvent?.Invoke();
            }

            _isIncType = true;
            DisableHeat(_owner);
        }
    }

    public void DisableHeat(Skill owner)
    {
        if (_owner != owner) return;

        _effect.Stop();
        _owner = null;
        _isMoveToTarget = false;
        _collider.enabled = false;
    }

    public void SetSortingLayer(string name) => _effect.SetSortingLayer(name);

    public void SetLight(bool active) => _light.enabled = active;
}
