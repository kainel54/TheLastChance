using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDecisions : GolemAiDecision
{
    [SerializeField] private RunDecisions runDecisions;
    [SerializeField] private DieState rolemDieState;
    [SerializeField] private float _range;
    private bool _isCutSceneEnd;

    private float presentTime = 99;
    [SerializeField]private float futureTime = 2f;

    private void Awake()
    {
        _isCutSceneEnd = false;
    }
    public override bool MakeDecision()
    {
        if(!rolemDieState.GolemShockCheck)
        {
            if (_isCutSceneEnd && presentTime >= futureTime && Vector3.Distance(_brain.transform.position, _brain.p_transform.position) < _range)
            {
                runDecisions.Run = false;
                presentTime = 0;
                return true;
            }
            else
            {
                presentTime += Time.deltaTime;
                return false;
            }

        }
        else
        {
            return false;
        }
    }

    public void CutSceneEnd()
    {
        _isCutSceneEnd = true;
    }
}
