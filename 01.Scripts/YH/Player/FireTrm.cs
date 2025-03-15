using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTrm : MonoBehaviour,IPlayerCompo
{
    private Player _player;
    private Transform _playerTrm;
    private InputReader _playerInput;

    public void Init(Player player)
    {
        _player = player;
        _playerTrm = _player.transform;
        _playerInput = _player.GetCompo<InputReader>();
    }
    private void Update()
    {
        RotateGun();
    }
    private void RotateGun()
    {
        Vector2 mousePos = _playerInput.MousePoint;
        Vector2 direction = _playerTrm.InverseTransformPoint(mousePos);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.localRotation = Quaternion.Euler(0, 0, angle);
    }
}
