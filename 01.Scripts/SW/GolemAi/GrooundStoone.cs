using ObjectPooling;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrooundStoone : PoolableMono
{
    [SerializeField] private Vector2 hitSize;
    [SerializeField] private Transform gizmosPosition;
    [SerializeField] private LayerMask _playerLayerMask;
    public override void ResetItem()
    {

    }

    public void HitCheck()
    {
        Collider2D playerCollider = Physics2D.OverlapBox(gizmosPosition.position,hitSize,0,_playerLayerMask);
        if(playerCollider != null)
        {
            print("ÇÃ·¹ÀÌ¾î Æã!");
            playerCollider.GetComponent<PlayerHealth>().ApplyDamage(0f);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(gizmosPosition.position, hitSize);
    }


}
