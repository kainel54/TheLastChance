using ObjectPooling;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour, IPlayerCompo
{
    [SerializeField] private float _delay;
    [SerializeField] private float _realodCooltime;
    [SerializeField] private Transform _fireTrm;
    [SerializeField] private float _chargingValue;
    private float _availableFireTime = 0;
    private float _realoadingTime = 0;
    private float _currentChargingValue;
    private bool _isCasting;
    private bool _isCastingStart;
    private bool _isFire;
    private bool _isBossCutScene;

    private Player _player;
    private InputReader _playerInput;
    public event Action<bool> OnCastingEvent;
    public event Action OnCastingStartEvent;
    public Vector2 FirePosition {  get; private set; }

    public void Init(Player player)
    {
        _player = player;
        OnCastingStartEvent += _player.OnChargingEvent.Invoke;
    }

    private void Start()
    {
        _playerInput = _player.GetCompo<InputReader>();
        _playerInput.FireAction += HandleFireEvent;
        _player.GetCompo<PlayerHealth>().OnDeadEvent += HandleDeadEvent;
    }

    private void HandleDeadEvent()
    {
        _isFire = false;
    }

    private void OnDestroy()
    {
        OnCastingStartEvent -= _player.OnChargingEvent.Invoke;
    }

    private void Update()
    {
        if (_player.GetCompo<PlayerHealth>().IsDead) return;
        if (_isBossCutScene) return;

        if (_isFire)
        {
            OnCastingEvent?.Invoke(false);
        }

        if (_realoadingTime < Time.time)
        {
            if (_isCasting)
            {
                if (!_isCastingStart)
                {
                    OnCastingStartEvent?.Invoke();
                    _isCastingStart = true;
                    OnCastingEvent?.Invoke(true);
                    TryToShoot();
                }
                else
                {
                    TryToShoot();
                    OnCastingEvent?.Invoke(true);
                }
            }
            else
            { 
                _currentChargingValue = 0;
                _isCastingStart = false;
                OnCastingEvent?.Invoke(false);
            }
        }
        
    }

    private void HandleFireEvent(bool isClick)
    {
        _isCasting = isClick;
    }

    private void ResetFireTime()
    {
        _availableFireTime = Time.time + _delay;
    }


    private void TryToShoot()
    {
        if (_availableFireTime < Time.time)
        {
            if (_currentChargingValue >= _chargingValue)
            {
                ShootFireBall();
                _currentChargingValue = 0;
                _realoadingTime = Time.time + _realodCooltime;
            }
            else
            {
                _currentChargingValue += 0.1f;
            }
            ResetFireTime();
        }
    }

    private void ShootFireBall()
    {
        _player.OnFireEvent?.Invoke();

        CameraManager.Instance.ShakeCam(1.5f,3f);
        FireBall newBullet = PoolManager.Instance.Pop(PoolingType.FireBall) as FireBall;
        _isFire = true;
        //발사 사운드
        newBullet.InitAndFire(_fireTrm) ;
        FirePosition = transform.position;
    }

    public void SetFire(bool isFire)
    {
        _isBossCutScene = isFire;
    }
}
