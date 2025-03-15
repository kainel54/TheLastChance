using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillState : GolemAIState
{
    [SerializeField] private RunDecisions runDecisions;
    [SerializeField] private GolemSkill golemSkill;
    public override void OnEnterState()
    {
        _brain.GolemRigidbody.velocity = Vector2.zero;
    }

    public override void OnExiState()
    {
        
    }

    public override void UpdateState()
    {
        base.UpdateState();
        if (golemSkill.SkillUse == false && runDecisions.Run == false)
        {
            golemSkill.SkillUse = true;
            golemSkill.ChoicePattern();
        }
        if(!golemSkill.Pattern3Start)
            _brain.GolemRigidbody.velocity = Vector2.zero;

    }
}
