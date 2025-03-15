using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunState : GolemAIState
{
    [SerializeField] private float moveSpeed = 5f;
    public override void OnEnterState()
    {
        _brain.GolemAnimation.GolemAniAllStop();
    }

    public override void OnExiState()
    {
    }

    public override void UpdateState()
    {
        base.UpdateState();
        _brain.GolemAnimation.Flip(_brain.GolemRigidbody);
        Vector2 golemDir = (_brain.p_transform.position - transform.position).normalized;
        _brain.GolemRigidbody.velocity = new Vector2(golemDir.x * moveSpeed, golemDir.y * moveSpeed);
        

    }

}
