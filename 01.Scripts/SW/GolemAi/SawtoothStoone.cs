using ObjectPooling;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class SawtoothStoone : PoolableMono
{
    private Vector2 goalsPos;
    private Rigidbody2D sawtoothStoneRigidbody;

    private bool move;

    private float moveSpeed = 15f;
    private float moveStartTime = 0;
    private float moveEndTime = 1.5f;
    public void MoveStart(Vector2 strikePoint, Vector2 playerPoint)
    {
        sawtoothStoneRigidbody = GetComponent<Rigidbody2D>();
        goalsPos = (new Vector2(playerPoint.x - strikePoint.x, playerPoint.y - strikePoint.y).normalized) * -1f;
        move = true;
    }

    private void FixedUpdate()
    {
        if (move)
        {
            if (moveStartTime >= moveEndTime)
                PoolManager.Instance.Push(this);
            else
            {
                sawtoothStoneRigidbody.velocity = new Vector2(goalsPos.x * moveSpeed, goalsPos.y * moveSpeed);
                moveStartTime += Time.deltaTime;
            }
        }
    }

    public override void ResetItem()
    {
        moveStartTime = 0;
        move = false;
        goalsPos = Vector2.zero;
    }
}
