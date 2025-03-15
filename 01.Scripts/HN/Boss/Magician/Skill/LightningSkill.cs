using ObjectPooling;
using System.Collections;
using UnityEngine;

public class LightningSkill : Skill
{
    [SerializeField] private FeedbackPlayer _impactFeedback;

    [SerializeField] private int _attackCnt;
    [SerializeField] private float _attackTerm;

    private Coroutine _coroutine;
    private WaitForSeconds _sec;

    private void Awake()
    {
        _sec = new WaitForSeconds(_attackTerm);
    }

    private void OnDisable()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);
    }

    public override void Play<T>()
    {
        base.Play<T>();

        _coroutine = StartCoroutine(AttackCoroutine());
    }

    private IEnumerator AttackCoroutine()
    {
        yield return new WaitForSeconds(1f);

        _magicianBoss.SetHeart(this);
        _magicianBoss.MagicianHeart.SetSortingLayer("Agent");

        for (int i = 0; i < _attackCnt; i++)
        {
            CameraManager.Instance.ShakeCam(1f, 1.5f);

            LightningAttack lightningAttack = PoolManager.Instance.Pop(PoolingType.LightningAttack) as LightningAttack;
            lightningAttack.StartAttack(this, _magicianBoss, _magicianBoss.PlayerTrm.position);

            Vector2 lightningPos = new Vector2(lightningAttack.transform.position.x, lightningAttack.transform.position.y - 2.1f);

            _magicianBoss.MagicianHeart.transform.position = lightningPos;

            yield return _sec;
        }

        bool isLastType = _magicianBoss.IsLastType();
        float time = isLastType ? 0 : 2.3f;

        yield return new WaitForSeconds(time);

        if (!isLastType) _magicianBoss.MagicianHeart.DisableHeat(this);
        _magicianBoss.MagicianHeart.SetSortingLayer("VFX");
        _magicianBoss.ActiveSkill(false);
    }

    public void PlayImpactSound() => _impactFeedback.PlayFeedback();
}
