using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieDecisions : GolemAiDecision
{
    public override bool MakeDecision()
    {
        return _brain.GolemFireHit(ObjectPooling.PoolingType.SSawtoothStoone);
    }
}
