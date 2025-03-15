using Cinemachine;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoSingleton<CameraManager>
{
    [SerializeField] private CinemachineVirtualCamera _vCam;
    private CinemachineBasicMultiChannelPerlin _bPerlin;
    private Tween _prevTween = null;

    public CinemachineVirtualCamera VCam => _vCam;

    private void Awake()
    {
        Init();
        _bPerlin.m_AmplitudeGain = 0;
    }

    public void Init()
    {
        _bPerlin = _vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void ShakeCam(float time, float power, Action OnCompelete = null)
    {
        if (_prevTween != null && _prevTween.IsActive())
        {
            _prevTween.Kill();
        }
        _bPerlin.m_AmplitudeGain = power;
        _prevTween = DOTween.To
        (
            () => _bPerlin.m_AmplitudeGain,
            value => _bPerlin.m_AmplitudeGain = value,
            0,
            time
        ).OnComplete(() => OnCompelete?.Invoke());
    }

    public bool ScreenToWorld(Vector2 screenPos, out Vector2 worldPos)
    {
        Camera mainCam = Camera.main;
        Ray ray = mainCam.ScreenPointToRay(screenPos);

        bool result = Physics.Raycast(ray, out RaycastHit hit, mainCam.farClipPlane);

        worldPos = result ? hit.point : Vector2.zero;
        return result;
    }

    public Vector2 TowardMouseDirection(Transform trm, Vector2 mousePos)
    {
        bool isHit = ScreenToWorld(mousePos, out Vector2 worldPos);
        if (isHit)
        {
            Vector2 direction = worldPos - (Vector2)trm.position;
            return direction.normalized;
        }

        return trm.forward;
    }

    public void SetCam(CinemachineVirtualCamera cam)
    {
        _vCam = cam;
        _bPerlin = _vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _bPerlin.m_AmplitudeGain = 0;
    }

    public void StopShake()
    {
        if (_prevTween == null || !_prevTween.IsActive()) return;

        _prevTween.Kill();
        _prevTween = null;
        _bPerlin.m_AmplitudeGain = 0;
    }
}
