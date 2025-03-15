using ObjectPooling;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserImpact : PoolableMono
{
    private readonly int _endHash = Animator.StringToHash("End");
    private Animator _anim;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    public void SetPosAndRotation(Vector2 pos, Vector3 laserRot)
    {
        transform.position = pos;
        transform.eulerAngles = laserRot;
    }

    public void End() => _anim.SetBool(_endHash, true);

    public void GoPool()
    {
        _anim.SetBool(_endHash, false);
        PoolManager.Instance.Push(this);
    }

    public override void ResetItem()
    {
    }
}
