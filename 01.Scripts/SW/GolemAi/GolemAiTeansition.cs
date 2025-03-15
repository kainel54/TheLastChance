using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemAiTeansition : MonoBehaviour
{
    private List<GolemAiDecision> _aiDecisions;
    public GolemAIState NextStste;
    public void SepUP(GolemAIBrain brain)
    {
        _aiDecisions = new List<GolemAiDecision>();
        GetComponents(_aiDecisions);
        _aiDecisions.ForEach(aiDecisions => aiDecisions.SepUP(brain));
    }

    public bool MakeATeansition()
    {
        bool _return = false;
        foreach (GolemAiDecision decision in  _aiDecisions)
        {
            _return = decision.MakeDecision();
            if (decision.isReverse)
                _return = !_return;
            if(_return == false) return false;
        }
        return _return;
    }
}
