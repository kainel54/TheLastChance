using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAnimationTrigger : MonoBehaviour
{
    public Action OnTriggerEvent;
    public Action OnSoundTriggerEvent;

    public void SoundTrigger() => OnSoundTriggerEvent?.Invoke();
    public void AttackTrigger() => OnTriggerEvent?.Invoke();
}
