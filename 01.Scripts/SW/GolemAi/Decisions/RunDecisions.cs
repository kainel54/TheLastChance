using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunDecisions : GolemAiDecision
{
    [SerializeField] private DieState rolemDieState;
    public bool Run { get; set; } = true;
    public override bool MakeDecision()
    {
        if(!rolemDieState.GolemShockCheck)
        {
            return Run;
        }
        else
        {
            return false;
        }
    }
}
