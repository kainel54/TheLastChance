using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GolemAiDecision : MonoBehaviour
{
    protected GolemAIBrain _brain;
    public bool isReverse;
    internal void SepUP(GolemAIBrain brain)
    {
        _brain = brain;
    }

    public abstract bool MakeDecision();
}
