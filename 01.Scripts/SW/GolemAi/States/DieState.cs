using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DieState : GolemAIState
{
    public Action OnGolemDie;
    public bool GolemShockCheck { get; set; }
    public bool GolemStemOneCheck { get; private set; }

    private float shockStartTime = 0;
    private float shockEndTime = 1f;

    public override void OnEnterState()
    {
        GolemShockCheck = true;
        _brain.GolemAnimation.GolemAniChoice(GolemAnimationName.stem);
        _brain.GolemAnimation.GolemAniAllStop();
    }

    public override void OnExiState()
    {
        _brain.GolemAnimation.GolemAniSetting("Run",true);
        _brain.GolemAnimation.GolemAniSetting("Stem", false);
    }

    public override void UpdateState()
    {
        base.UpdateState();
        if(shockStartTime >= shockEndTime)
        {
            if(_brain.GolemHitCount == 1)
            {
                GolemShockCheck = false;
                CameraManager.Instance.ShakeCam(1.5f, 30f);
                shockEndTime = 9f;
            }
            else
            {
                _brain.OnPage2Event?.Invoke();
                GolemShockCheck = false;
                GolemStemOneCheck = true;
                CameraManager.Instance.ShakeCam(1.5f, 30f);
            }
        }
        else
        {
            if(_brain.GolemFireHit(ObjectPooling.PoolingType.FireBall))
            {
                _brain.GolemAnimation.GolemAniChoice(GolemAnimationName.die);
                _brain.GolemAnimation.GolemAniSetting("Stem", false);
                OnGolemDie?.Invoke();
                shockStartTime = 0;
                _brain.OnDeadEvent?.Invoke();
                _brain.clearUI.Clear();
                print("º¸½º »ç¸Á");
            }
            _brain.GolemRigidbody.velocity = Vector2.zero;
            shockStartTime += Time.deltaTime;
        }
    }

}
