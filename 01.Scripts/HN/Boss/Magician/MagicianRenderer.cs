using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicianRenderer : MonoBehaviour
{
    [SerializeField] private float _dissovleDuration;

    private Magician _magicianBoss;
    public SpriteRenderer SpriteRenderer { get; private set; }
    private Material _mat;

    private readonly int _fadeId = Shader.PropertyToID("_Fade");

    public void Initialize(Magician boss)
    {
        _magicianBoss = boss;
        SpriteRenderer = GetComponent<SpriteRenderer>();
        _mat = SpriteRenderer.material;
    }

    public void Dissolve(bool active, Action callback)
    {
        int endValue = active? 1 : 0;

        _mat.DOFloat(endValue, _fadeId, _dissovleDuration).
            OnComplete(() => callback?.Invoke());
    }
}
