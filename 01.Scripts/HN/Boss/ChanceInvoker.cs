using DG.Tweening;
using System;

public class ChanceInvoker
{
    private bool _isEnded;
    private bool _isSuccess;
    private Tween _tween;

    public ChanceInvoker(float chanceTime, Action successCallback, Action failureCallback)
    {
        _tween = DOVirtual.DelayedCall(chanceTime, () => failureCallback?.Invoke()).OnUpdate(() =>
        {
            if (!_isEnded) return;

            _tween.Kill();

            if(_isSuccess) successCallback?.Invoke();
            else failureCallback?.Invoke();

            _isEnded = false;
        });
    }

    public void SetSuccessOrNot(bool success)
    {
        _isSuccess = success;
        _isEnded = true;
    }
}
