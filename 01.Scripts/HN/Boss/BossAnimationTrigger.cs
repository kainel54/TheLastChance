using UnityEngine;

public class BossAnimationTrigger : MonoBehaviour
{
    [SerializeField] private Boss _boss;

    private void AnimationEndTrigger()
    {
        _boss.AnimationEndTrigger();
    }

    private void FunctionTrueTrigger()
    {
        _boss.FunctionTrigger(true);
    }

    private void FunctionFalseTrigger()
    {
        _boss.FunctionTrigger(false);
    }
}
