using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Effect : MonoBehaviour
{
    [field : SerializeField] public bool IsChangeableMatColor { get; protected set; }

    protected List<ParticleSystem> _particles = new List<ParticleSystem>();
    protected List<ParticleSystemRenderer> _particleRenderers = new List<ParticleSystemRenderer>();
    protected List<Material> _materials = new List<Material>();
    protected List<Material> _trailMaterials = new List<Material>();
    private List<Tween> _tweens = new List<Tween>();

    public void Initialize()
    {
        _particles = GetComponentsInChildren<ParticleSystem>().ToList();

        for(int i = 0; i < _particles.Count; i++)
        {
            ParticleSystemRenderer renderer = 
                _particles[i].GetComponent<ParticleSystemRenderer>();

            _particleRenderers.Add(renderer);

            _materials.Add(renderer.material);

            Material trailMat = null;

            if(_particles[i].trails.enabled && renderer.trailMaterial != null)
            {
                trailMat = new Material(renderer.trailMaterial);
                renderer.trailMaterial = trailMat;
            }

            _trailMaterials.Add(trailMat);
        }
    }
    public virtual void Play(Vector2 position, Color color)
    {
        transform.position = position;

        ChangeColor(color, 0, null);

        foreach(ParticleSystem particle in _particles)
        {
            particle.Play();
        }
    }

    //이펙트 색 변경
    protected void ChangeColor(Color color, float delay, Action oncomplete)
    {
        if (!IsChangeableMatColor) return;
        
        foreach (Tween tween in _tweens)
        {
            if (tween != null)
                tween.Kill();
        }

        _tweens.Clear();

        for (int i = 0; i < _materials.Count; i++)
        {
            _materials[i].SetColor("_EmissionColor", color);

            _tweens.Add(_materials[i].DOColor(color, delay)
                .OnComplete(() =>
                {
                    oncomplete?.Invoke();
                }));

            if (_trailMaterials[i] != null)
                _tweens.Add(_trailMaterials[i].DOColor(color, delay));
        }
    }

    private void OnDisable()
    {
        foreach(Tween tween in _tweens)
        {
            if(tween != null)
                tween.Kill();
        }
    }

    public void Stop()
    {
        foreach (Tween tween in _tweens)
        {
            if (tween != null)
                tween.Kill();
        }

        foreach(ParticleSystem particle in _particles)
        {
            particle.Stop();
        }
    }

    public void SetSortingLayer(string name)
    {
        foreach(var renderer in _particleRenderers)
        {
            renderer.sortingLayerName = name;
        }
    }
}
