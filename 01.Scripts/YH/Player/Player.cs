
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    public UnityEvent OnChargingEvent;
    public UnityEvent OnFireEvent;
    public UnityEvent OnDeadEvent;

    [SerializeField] private InputReader _inputCompo;
    public Rigidbody2D RigidCompo { get; private set; }

    private Dictionary<Type, IPlayerCompo> _components;

    private void Awake()
    {
        _components = new Dictionary<Type, IPlayerCompo>();
        GetComponentsInChildren<IPlayerCompo>().ToList().ForEach(compo => _components.Add(compo.GetType(), compo));
        _components.Add(_inputCompo.GetType(), _inputCompo);
        RigidCompo = GetComponent<Rigidbody2D>();
        foreach (var compo in _components.Values)
        {
            compo.Init(this);
        }

    }

    public T GetCompo<T>() where T : class
    {
        if(_components.TryGetValue(typeof(T), out IPlayerCompo compo))
        {
            return compo as T;
        }
        return default;
    }

}
