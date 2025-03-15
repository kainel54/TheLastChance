using ObjectPooling;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GolemAIBrain : MonoBehaviour
{
    public UnityEvent OnDeadEvent;
    public UnityEvent OnPage2Event;
    public UnityEvent OnDeshStartEvent;
    public UnityEvent OnDeshEndEvent;
    public UnityEvent OnBigStoneEvent;
    public UnityEvent OnSmallStoneEvent;
    public UnityEvent OnSwooshEvent;

    private List<GolemAIState> _states;
    public Transform p_transform;
    [SerializeField] private LayerMask playerLayer;

    
    [SerializeField] private GolemAIState _golemState;
    private GolemAIState _currentState;
    [SerializeField] private Vector2 hitDetectionSiez;
    [SerializeField] private Vector2 deshDetectionSiez;

    [SerializeField] private Transform hitPosition;
    [SerializeField] private Transform deshPosition;
    [field:SerializeField] public Clear_UI clearUI { get;private set; }
    public GolemAnimation GolemAnimation { get; private set; }
    public Rigidbody2D GolemRigidbody {  get; private set; }

    private GolemSkill _golemSkill;

    public float GolemHitCount { get; private set; } = 0;

    private void Awake()
    {
        _states = new List<GolemAIState>();
        _golemSkill = GetComponent<GolemSkill>();
        GolemRigidbody = GetComponent<Rigidbody2D>();
        GolemAnimation = GetComponentInChildren<GolemAnimation>();
        GetComponentsInChildren(_states);
        _states.ForEach(state => state.SetUP(transform));
    }

    private void Update()
    {
        if (_currentState == null) return;
        _currentState.UpdateState();
        if (GolemFireHit(PoolingType.FireBall))
            PoolManager.Instance.Push(GetCollider(PoolingType.FireBall).GetComponent<FireBall>());
        else if(GolemFireHit(PoolingType.SSawtoothStoone))
        {
            PoolManager.Instance.Push(GetCollider(PoolingType.SSawtoothStoone).GetComponent<SawtoothStoone>());
            CameraManager.Instance.ShakeCam(0.5f,20f);
            if(++GolemHitCount == 1)
                _golemSkill.Page2();
        }
        if(Physics2D.OverlapBox(deshPosition.position, hitDetectionSiez, 0, playerLayer))
        {
            p_transform.GetComponent<PlayerHealth>().ApplyDamage(0f);
            print("플레이어 근접 펑!");
        }
    }

    public void SettingState()
    {
        _currentState = _golemState;
    }

    public void ChangeState(GolemAIState golemAIState)
    {
        _currentState.OnExiState();
        _currentState = golemAIState;
        _currentState.OnEnterState();
    }

    public bool GolemFireHit(PoolingType type)
    {
        Collider2D overlapCollider = Physics2D.OverlapBox(hitPosition.position, hitDetectionSiez, 0);
        switch(type)
        {
            case PoolingType.SSawtoothStoone:
                return overlapCollider.GetComponent<SawtoothStoone>() != null;
            case PoolingType.FireBall:
                return overlapCollider.GetComponent<FireBall>() != null;
        }
        return false;
    }

    public bool GetDeshPlayerHit()
    {
        return Physics2D.OverlapBox(deshPosition.position,deshDetectionSiez,0,playerLayer);
    }

    public Collider2D GetCollider(PoolingType type)
    {
        switch (type)
        {
            case PoolingType.SSawtoothStoone: if (GolemFireHit(PoolingType.SSawtoothStoone)) return Physics2D.OverlapBox(hitPosition.position, hitDetectionSiez, 0) ; break;
            case PoolingType.FireBall: if (GolemFireHit(PoolingType.FireBall)) return Physics2D.OverlapBox(hitPosition.position, hitDetectionSiez, 0) ; break;
        }
        return null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(hitPosition.position, hitDetectionSiez);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(deshPosition.position, deshDetectionSiez);
    }
}
