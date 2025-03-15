using ObjectPooling;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BuiltStone : PoolableMono
{
    [SerializeField] private Vector2 hitSize;
    [SerializeField] private Transform gizmosPosition;

    private Animator _builtStoneAnimator;

    private float builtStoneCreationStartTime = 0;
    private float builtStoneCreationEndTime = 0.3f;
    public Transform _playerFirePosition {  get; set; }
    public override void ResetItem()
    {

    }

    private void Update()
    {
        if (builtStoneCreationStartTime >= builtStoneCreationEndTime)
            HitCheck();
        else
            builtStoneCreationStartTime += Time.deltaTime;
    }

    private void HitCheck()
    {
        Collider2D fierCollider = Physics2D.OverlapBox(gizmosPosition.position, hitSize, 0);
        if (fierCollider.GetComponent<FireBall>())
        {
            SawtoothStoone sawtoothStone = PoolManager.Instance.Pop(PoolingType.SSawtoothStoone) as SawtoothStoone;
            sawtoothStone.transform.position = transform.position;
            sawtoothStone.MoveStart(fierCollider.transform.position,_playerFirePosition.GetComponent<Fire>().FirePosition);
            PoolManager.Instance.Push(fierCollider.GetComponent<PoolableMono>());
            PoolManager.Instance.Push(this);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(gizmosPosition.position, hitSize);
    }

    public void CountOutBuiltStone()
    {
        _builtStoneAnimator = GetComponent<Animator>();
        _builtStoneAnimator.SetBool("Removal", true);
        StartCoroutine(CountOutBuiltStoneTime());
    }

    private IEnumerator CountOutBuiltStoneTime()
    {
        yield return new WaitForSeconds(0.5f);
        PoolManager.Instance.Push(this);
    }
}
