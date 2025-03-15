using ObjectPooling;
using System.Collections;
using UnityEngine;

public class DarknessSkill : Skill
{
    [SerializeField] private float _attackStartDelay;
    [SerializeField] private float _attackCnt;
    [SerializeField] private float _attackTerm;
    [SerializeField] private float _sizeOffset;

    private Coroutine _coroutine;

    public override void Play<T>()
    {
        base.Play<T>();

        _coroutine = StartCoroutine(AttackCoroutine());
    }

    private IEnumerator AttackCoroutine()
    {
        yield return new WaitForSeconds(_attackStartDelay);

        Vector2 playerPos = _magicianBoss.PlayerTrm.position;

        for (int i = 0; i < _attackCnt; i++)
        {
            CameraManager.Instance.ShakeCam(2f, 2f);

            for (int j = 0; j < 3; j++)
            {
                DarknessAttack darknessAttack = PoolManager.Instance.Pop(PoolingType.DarknessAttack) as DarknessAttack;
                darknessAttack.StartAttack(this, _magicianBoss, playerPos);

                darknessAttack.transform.localScale = Vector3.one * (1 + _sizeOffset * j);

                yield return new WaitForSeconds(_attackTerm);
            }

            playerPos = _magicianBoss.PlayerTrm.position;
        }

        _magicianBoss.SetHeart(this);

        bool isLastType = _magicianBoss.IsLastType();
        float time = isLastType ? 0 : 1.7f;

        yield return new WaitForSeconds(time);

        if (!isLastType) _magicianBoss.MagicianHeart.DisableHeat(this);
        _magicianBoss.ActiveSkill(false);
    }
}
