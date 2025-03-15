using System;
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, IPlayerCompo
{
    [SerializeField] private float _walkSpeed, _runSpeed;
    private Player _player;
    private Vector2 _movement;
    private InputReader _input;
    private float _curDashCooltime, _dashCooltime = 5f;
    private bool _isCancel;
    private bool _canMove = true;
    public bool CanMove
    {
        get => _canMove;
        set
        {
            if (!value)
            {
                if (_dashCoroutine != null)
                    StopCoroutine(_dashCoroutine);
            }

            _canMove = value;
        }
    }
    public bool IsStopedByBossOrDead { get; set; }

    [SerializeField] private float _dashDuration;
    [SerializeField] private float _dashPower;

    public bool IsRunning { get; private set; }

    public event Action<Vector2, bool> OnMovementEvent;
    public event Action<bool> OnRunningEvent;
    public event Action<bool, float, float> OnDashEvent;
    public event Action OnCastingCancelEvent;

    private Coroutine _dashCoroutine;

    public void Init(Player player)
    {
        _player = player;
        _input = _player.GetCompo<InputReader>();
        _input.RunAction += HandleRunEvent;
        _input.DashEvent += HandleDashEvent;
        _input.FireAction += HandleFireEnd;
        _player.GetCompo<Fire>().OnCastingEvent += HandleChargingEvent;
        _player.GetCompo<PlayerHealth>().OnDeadEvent += HandleDeathEvent;
        _curDashCooltime = 0;
    }

    private void HandleDeathEvent()
    {
        _canMove = false;
        _player.RigidCompo.velocity = Vector2.zero;
        IsStopedByBossOrDead = true;

        if(_dashCoroutine != null) StopCoroutine(_dashCoroutine);
    }

    private void HandleFireEnd(bool obj)
    {
        if (!obj)
        {
            _isCancel = true;
        }
    }

    private void HandleChargingEvent(bool charging)
    {
        if (IsStopedByBossOrDead)
        {
            if (!_isCancel) _isCancel = true;
            return;
        }

        if (charging)
        {
            _player.RigidCompo.velocity = Vector2.zero;
            CanMove = !charging;
        }
        else
        {
            if (_isCancel)
            {
                CanMove = true;
                _isCancel = false;
                OnCastingCancelEvent?.Invoke();
            }
        }

    }

    private void HandleRunEvent(bool isRunning)
    {
        if (IsRunning != isRunning)
            OnRunningEvent?.Invoke(isRunning);
        IsRunning = isRunning;
    }

    private void FixedUpdate()
    {
        CalculateMovement();
        Move();
    }


    private void HandleDashEvent(Vector2 dashDir)
    {
        if (_curDashCooltime > Time.time || !CanMove) return;
        _dashCoroutine = StartCoroutine(CorutineOnDash(dashDir, _dashDuration, _dashPower));
        OnDashEvent?.Invoke(true, dashDir.x, dashDir.y);
    }

    private IEnumerator CorutineOnDash(Vector2 dashDir, float duration, float dashPower)
    {
        _curDashCooltime = Time.time + _dashCooltime;
        CanMove = false;
        _player.RigidCompo.velocity = dashDir * dashPower;

        yield return new WaitForSeconds(duration);

        _player.RigidCompo.velocity = Vector2.zero;
        OnDashEvent?.Invoke(false, dashDir.x, dashDir.y);
        CanMove = true;
    }

    private void CalculateMovement()
    {
        Vector2 moveInput = _player.GetCompo<InputReader>().Movement;
        _movement = moveInput;
        OnMovementEvent?.Invoke(_movement, _player.GetCompo<InputReader>().Performd);
        float speed = IsRunning ? _runSpeed : _walkSpeed;
        _movement *= speed;
    }

    private void Move()
    {
        if (CanMove)
        {
            _player.RigidCompo.velocity = _movement;
        }
    }

    private void OnDisable()
    {
        if (_dashCoroutine != null)
            StopCoroutine(_dashCoroutine);
    }

    
}
