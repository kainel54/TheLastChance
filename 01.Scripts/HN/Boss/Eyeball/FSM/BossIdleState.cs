using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIdleState : BossState
{
    private float _currentTime;
    private int _index = -1;
    public BossIdleState(Boss boss, BossStateMachine stateMachine, string animBoolName) : base(boss, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        _currentTime = 0;
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void UpdateState()
    {
        base.UpdateState();

        _currentTime += Time.deltaTime;

        int patternIndex = (_index + 1) % (_boss.PatternList.Count - 1);

        if (_boss.PatternList[patternIndex].cooltime < _currentTime)
        {
            _index = patternIndex;
            
            _boss.SetPattern(_index);
        }
    }
}
