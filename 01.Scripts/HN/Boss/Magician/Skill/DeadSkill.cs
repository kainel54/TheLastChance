using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DeadSkill : Skill
{
    [SerializeField] private Color _globalLightColor;
    [SerializeField] private float _attackTerm;

    private Player _player;
    private Light2D _globalLight;

    private void Awake()
    {
        _player = FindObjectOfType<Player>();
        _globalLight = GameObject.FindGameObjectWithTag("GlobalLight").GetComponent<Light2D>();
    }

    public override void Play<T>()
    {
        base.Play<T>();

        OnSkillEvent?.Invoke();
        StartCoroutine("AttackCoroutine");
    }

    private IEnumerator AttackCoroutine()
    {
        yield return new WaitForSeconds(_attackTerm);

        ChangeLight();

        Vector2 attackPos = new Vector2(_magicianBoss.PlayerTrm.position.x, _magicianBoss.PlayerTrm.position.y + 1.85f);

        DeadAttack deadAttack = PoolManager.Instance.Pop(ObjectPooling.PoolingType.DeadAttack) as DeadAttack;
        deadAttack.StartAttack(this, _magicianBoss, attackPos);

        Stop<DeadSkill>();

        CameraManager.Instance.ShakeCam(1.5f, 8f);

        _magicianBoss.stateMachine.ChangeState(BossEnum.Idle);
    }

    private void ChangeLight()
    {
        _globalLight.intensity = 1;
        _globalLight.color = _globalLightColor;
    }
}
