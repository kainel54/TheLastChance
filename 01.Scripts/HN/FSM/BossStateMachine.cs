using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStateMachine
{
    public BossState CurrentState { get; private set; }

    public Dictionary<BossEnum, BossState> stateDictionary
        = new Dictionary<BossEnum, BossState>();

    public Boss boss;
    public void Initialize(BossEnum startState, Boss enemy)
    {
        boss = enemy;
        CurrentState = stateDictionary[startState];
        CurrentState.Enter();
    }
    public void ChangeState(BossEnum newState, bool forceMode = false)
    {
        if (boss.CanStateChangeable == false && forceMode == false) return;
        if (boss.IsDead) return;

        CurrentState.Exit();
        CurrentState = stateDictionary[newState];
        CurrentState.Enter();
    }
    public void AddState(BossEnum stateEnum, BossState enemyState)
    {
        stateDictionary.Add(stateEnum, enemyState);
    }
}
