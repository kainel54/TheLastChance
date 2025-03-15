using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable, IPlayerCompo,IDataPerisistence
{
    public event Action OnDeadEvent;

    public bool IsDead { get; private set; } = false;

    [SerializeField] private DataSO dataSO;
    private Player _player;
    private bool _canHit = true;
    private PlayerMovement _playerMovement;
    [SerializeField] private PlayerDie_UI _playerDie;

    public void Init(Player player)
    {
        _player = player;
        OnDeadEvent += _player.OnDeadEvent.Invoke;

        _playerMovement = _player.GetCompo<PlayerMovement>();
        _playerMovement.OnDashEvent += HandleDashEvent;
    }

    private void OnDestroy()
    {
        _playerMovement.OnDashEvent -= HandleDashEvent;
        OnDeadEvent -= _player.OnDeadEvent.Invoke;
    }

    private void HandleDashEvent(bool isDash, float dirX, float dirY)
    {
        _canHit = !isDash;
    }

    public void ApplyDamage(float amount)
    {
        if (_canHit && !IsDead)
        {
            OnDeadEvent?.Invoke();
            _playerDie.OnDeadEvent();
            IsDead = true;
        }
    }

    public void LoadData(GameData data)
    {
        dataSO.deathCount = data.deathCount;
    }

    public void SaveData(ref GameData data)
    {
        data.deathCount = dataSO.deathCount;
    }
}
