using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorInfoCaster
{
    private Animator _anim;
    private List<AnimatorClipInfo> _animInfo = new List<AnimatorClipInfo>();
    private int _lastAnimInfoHash;

    public AnimatorInfoCaster(Animator animator)
    {
        _anim = animator;
    }

    public AnimationClip GetAnimationClip()
    {
        int currentAnimInfoHash = _anim.GetCurrentAnimatorStateInfo(0).shortNameHash;

        if(_lastAnimInfoHash != currentAnimInfoHash)
        {
            _anim.GetCurrentAnimatorClipInfo(0, _animInfo);
            _lastAnimInfoHash = currentAnimInfoHash;
        }

        return _animInfo.Count > 0 ? _animInfo[0].clip : null;
    }
}
