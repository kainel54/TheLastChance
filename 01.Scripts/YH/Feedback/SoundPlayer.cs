
using ObjectPooling;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class SoundPlayer : PoolableMono
{
    public bool IsPlaying => _audioSource.isPlaying;

    [SerializeField] private AudioMixerGroup _sfxGroup, _musicGroup;

    private AudioSource _audioSource;

    public GameObject GetGameObject() => gameObject;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(SoundSO data)
    {
        if (data.audioType == SoundSO.AudioTypes.SFX)
        {
            _audioSource.outputAudioMixerGroup = _sfxGroup;
        }
        else if (data.audioType == SoundSO.AudioTypes.Music)
        {
            _audioSource.outputAudioMixerGroup = _musicGroup;
        }

        _audioSource.volume = data.volume;
        _audioSource.pitch = data.pitch;
        if (data.randomizePitch)
        {
            _audioSource.pitch += Random.Range(-data.randomPitchModifier, data.randomPitchModifier);
        }
        _audioSource.clip = data.clip;

        _audioSource.loop = data.loop;

        if (!data.loop)
        {
            float time = _audioSource.clip.length + .2f;
            StartCoroutine(DisableSoundTimer(time));
        }
        _audioSource.Play();
    }

    private IEnumerator DisableSoundTimer(float time)
    {
        yield return new WaitForSeconds(time);
        PoolManager.Instance.Push(this);
    }

    public void StopAndGoToPool()
    {
        _audioSource.Stop();
        PoolManager.Instance.Push(this);
    }

    public override void ResetItem()
    {
        //doNothing
    }
}
