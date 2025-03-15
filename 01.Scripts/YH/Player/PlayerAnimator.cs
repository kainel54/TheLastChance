using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAnimator : MonoBehaviour, IPlayerCompo
{
    private readonly int _xVelocityHash = Animator.StringToHash("xVelocity");
    private readonly int _yVelocityHash = Animator.StringToHash("yVelocity");
    private readonly int _runHash = Animator.StringToHash("Run");
    private readonly int _idleHash = Animator.StringToHash("Idle");
    private readonly int _castingHash = Animator.StringToHash("Casting");
    private readonly int _dashHash = Animator.StringToHash("Dash");
    private readonly int _deathHash = Animator.StringToHash("Death");

    public UnityEvent OnCastingEvent;

    private Player _player;
    private Animator _animatorCompo;
    private bool _isdash;
    private Sequence _sequence;
    [SerializeField] private SpriteRenderer _magicCircle;
    public event Action OnDeathEndEvent;

    private bool _isRunAnimatonPlay;
    private PlayerMovement _moveCompo;

    private void OnEnable()
    {
        OnCastingEvent.AddListener(HandleCastingStartEvent);
    }
    private void OnDisable()
    {
        OnCastingEvent.RemoveListener(HandleCastingStartEvent);
    }

    public void Init(Player player)
    {
        _animatorCompo = GetComponent<Animator>();
        _player = player;
        _magicCircle.color = new Color(255, 255, 255, 0);

        _moveCompo = _player.GetCompo<PlayerMovement>();
        var fireCompo = _player.GetCompo<Fire>();
        _moveCompo.OnMovementEvent += HandleMovementEvent;
        _moveCompo.OnDashEvent += HandleDashEvent;
        _moveCompo.OnCastingCancelEvent += HandleCastingCancelEvent;
        fireCompo.OnCastingEvent+= HandleCastingEvent;
        fireCompo.OnCastingStartEvent+= HandleCastingStartEvent;
        _player.GetCompo<PlayerHealth>().OnDeadEvent += HandleDeathEvent;
    }

    private void OnDestroy()
    {
        _moveCompo.OnDashEvent -= HandleDashEvent;
    }

    private void HandleDeathEvent()
    {
        _animatorCompo.SetTrigger(_deathHash);
    }

    private void HandleCastingCancelEvent()
    {
        Debug.Log("castingCancel");
        _sequence.Kill();
        _sequence = DOTween.Sequence();
        _sequence.Append(_magicCircle.DOFade(0, 0.2f));
        _magicCircle.transform.rotation = Quaternion.identity;
    }

    private void HandleDashEvent(bool obj,float x,float y)
    {
        _isdash = obj;
        _animatorCompo.SetFloat(_xVelocityHash,x);
        _animatorCompo.SetFloat(_yVelocityHash,y);
    }

    
    private void HandleCastingStartEvent()
    {
        Debug.Log("castingStart");
        _sequence.Kill();
        _sequence = DOTween.Sequence();
        _sequence.Append(_magicCircle.DOFade(1, 0.2f));
        _sequence.Append(_magicCircle.transform.DORotate(new Vector3(0, 0, 180), 1.8f));
        _sequence.Append(_magicCircle.DOFade(0, 0.2f)).OnComplete(() =>
        {
            _magicCircle.transform.rotation = Quaternion.identity;
        });
        
    }

    private void HandleCastingEvent(bool isCasting)
    {
        _animatorCompo.SetBool(_castingHash, isCasting);
        if (isCasting)
        {
            _animatorCompo.SetFloat(_xVelocityHash, 0);
            _animatorCompo.SetFloat(_yVelocityHash, 0);
        }
        
    }

    private void HandleMovementEvent(Vector2 movement,bool input)
    {
        bool isPlayRun = movement.magnitude > 0;
        if (input)
        {
            _animatorCompo.SetFloat(_xVelocityHash, movement.x);
            _animatorCompo.SetFloat(_yVelocityHash, movement.y);
        }

        if (_isdash)
        {
            _animatorCompo.SetBool(_dashHash, true);
            _animatorCompo.SetBool(_idleHash, false);
            _animatorCompo.SetBool(_runHash, false);
        }
        else
        {
            _animatorCompo.SetBool(_dashHash, false);
            _animatorCompo.SetBool(_idleHash, !isPlayRun);
            _animatorCompo.SetBool(_runHash,isPlayRun);
        }
    }

    private void AnimationEnd()
    {
        OnDeathEndEvent?.Invoke();
    }
}
