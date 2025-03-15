using ObjectPooling;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WindSkill : Skill
{
    public UnityEvent OnWindReverseEvent;

    [SerializeField] private int _spawnCount;
    [SerializeField] private float _spawnTerm;
    [SerializeField] private float _l;

    private Coroutine _coroutine;
    private WaitForSeconds _sec;
    private List<WindAttack> _attacks = new List<WindAttack>();

    private void Awake()
    {
        _sec = new WaitForSeconds(_spawnTerm);
    }

    public override void Play<T>()
    {
        base.Play<T>();

        _coroutine = StartCoroutine(SpawnCoroutine());
    }

    private void OnDisable()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);
    }

    private IEnumerator SpawnCoroutine()
    {
        yield return new WaitForSeconds(0.7f);

        int randNum = Random.Range(0, _spawnCount);

        for (int i = 0; i < _spawnCount; i++)
        {
            CameraManager.Instance.ShakeCam(1.5f, 2f);

            WindAttack windAttack = PoolManager.Instance.Pop(PoolingType.WindAttack) as WindAttack;
            windAttack.StartAttack(this, _magicianBoss, transform.position);

            OnSkillEvent?.Invoke();

            _attacks.Add(windAttack);

            if (i == randNum)
            {
                _magicianBoss.SetHeart(this);
                _magicianBoss.MagicianHeart.SetSortingLayer("Agent");
                _magicianBoss.MagicianHeart.SetTarget(true, windAttack.transform, this);
            }

            yield return _sec;
        }

        yield return new WaitForSeconds(1);

        OnWindReverseEvent?.Invoke();
        foreach (WindAttack windAttack in _attacks) windAttack.ReturnTornado();

        CameraManager.Instance.StopShake();
        CameraManager.Instance.ShakeCam(1.5f, 4.5f);

        _magicianBoss.MagicianHeart.SetSortingLayer("VFX");
        bool isLastType = _magicianBoss.IsLastType();
        float time = isLastType ? 0 : 1.7f;

        yield return new WaitForSeconds(time);

        _attacks.Clear();
        _magicianBoss.MagicianHeart.SetTarget(false, null, this);
        if (!isLastType) _magicianBoss.MagicianHeart.DisableHeat(this);
        _magicianBoss.ActiveSkill(false);
    }
}
