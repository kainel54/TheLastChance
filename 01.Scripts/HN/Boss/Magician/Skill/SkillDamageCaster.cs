using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDamageCaster : MonoBehaviour
{
    [SerializeField] private Transform _castTrm;
    [SerializeField] private float _radius;
    [SerializeField] private int _castCnt;
    [SerializeField] private ContactFilter2D _filter;

    private Collider2D[] _collider;

    private void Awake()
    {
        _collider = new Collider2D[_castCnt];
    }

    public void Cast()
    {
        int cnt = Physics2D.OverlapCircle(_castTrm.position, _radius, _filter, _collider);
        if(cnt > 0 && _collider[0].TryGetComponent(out Player player))
        {
            player.GetCompo<PlayerHealth>().ApplyDamage(1);
            print("Damaged");
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_castTrm.position, _radius);
    }
}
