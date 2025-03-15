using ObjectPooling;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Stone : PoolableMono
{
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private Vector2 overlapSize;

    private Animator _stoneAnimator;
    private Transform p_transform;
    private GolemSkill _skill;

    private float _lifeTime;
    private float moveSpeed;
    private bool move;

    public override void ResetItem()
    {
           
    }

    private void Update()
    {
        if (move)
        {
            Collider2D playerCollider = Physics2D.OverlapBox(transform.position, overlapSize, 0, _playerLayer);
            transform.position = Vector3.MoveTowards(transform.position, p_transform.position, moveSpeed * Time.deltaTime);
            if (_lifeTime < Time.time)
            {
                _stoneAnimator.SetBool("Bomb",true);
                StartCoroutine(StoneAnimatorTime());
                move = false;
            }
            if(playerCollider != null)
            {
                _stoneAnimator.SetBool("Bomb", true);
                playerCollider.GetComponent<PlayerHealth>().ApplyDamage(0);
                StartCoroutine(StoneAnimatorTime());
                print("ÇÃ·¹ÀÌ¾î Æã");
                move = false;
            }
        }
    }

    private IEnumerator StoneAnimatorTime()
    {
        yield return new WaitForSeconds(0.5f);
        BuiltStone builtStone = PoolManager.Instance.Pop(PoolingType.BuiltStone) as BuiltStone;
        _skill.GetComponent<GolemAIBrain>().OnBigStoneEvent?.Invoke();
        _skill.BuiltStonesPush(builtStone);
        builtStone.transform.position = transform.position;
        builtStone._playerFirePosition = p_transform;
        PoolManager.Instance.Push(this);
    }

    public void MovingStone(Transform player, GolemSkill golemSkill, float speed)
    {
        _lifeTime = Time.time + 1.5f;
        if (move == false)
        {
            _stoneAnimator = GetComponent<Animator>();
            p_transform = player;
            _skill = golemSkill;
            moveSpeed = speed;
            move = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position,overlapSize);
    }
}
