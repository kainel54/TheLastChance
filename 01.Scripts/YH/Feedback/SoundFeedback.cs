using UnityEngine;

public class SoundFeedback : Feedback
{
    [SerializeField] private SoundSO _soundData;

    private SoundPlayer _sound;

    public override void CreateFeedback()
    {
        _sound = PoolManager.Instance.Pop(ObjectPooling.PoolingType.SoundPlayer) as SoundPlayer;
        _sound.PlaySound(_soundData);
    }

    public override void FinishFeedback()
    {
        if (_sound == null || _sound.IsPlaying) return;

        _sound.StopAndGoToPool();
    }

    public void StopSound() => _sound?.StopAndGoToPool();
}
