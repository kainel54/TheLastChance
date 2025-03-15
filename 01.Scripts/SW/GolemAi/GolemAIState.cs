using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GolemAIState : MonoBehaviour, IGolemIState
{
    [SerializeField] protected List<GolemAiTeansition> _golemAiTeansitions;
    protected GolemAIBrain _brain;
    public virtual void SetUP(Transform transform)
    {
        _brain = transform.GetComponent<GolemAIBrain>();

        _golemAiTeansitions = new List<GolemAiTeansition>();
        GetComponentsInChildren(_golemAiTeansitions);
        _golemAiTeansitions.ForEach(brain => brain.SepUP(_brain));
    }

    public abstract void OnEnterState();

    public abstract void OnExiState();

    public virtual void UpdateState()
    {
        foreach (GolemAiTeansition state in _golemAiTeansitions)
        {
            if(state.MakeATeansition())
            {
                _brain.ChangeState(state.NextStste);
            }
        }
    }

}
