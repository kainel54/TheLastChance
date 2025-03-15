using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrueSikllDecisions : GolemAiDecision
{
    [SerializeField] private DieState rolemDieState;
    public override bool MakeDecision()
    {
        if(!rolemDieState.GolemShockCheck)
        {
            return true;
        }
        else
        {
            return false;
        }    
    }
}
