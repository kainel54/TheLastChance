using Cinemachine.Utility;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GolemSkill : MonoBehaviour
{
    [SerializeField] private RunDecisions _runDecisions;
    [SerializeField] private GameObject hammerStone;
    private GolemAIBrain _brain;
    private DieState _golemDieState;
    private List<BuiltStone> _builtStones = new List<BuiltStone>();

    public bool SkillUse { get; set; }

    private float groundStoneCreateTime = 0.3f;
    private int stoneNumber = 10;
    private int stoneCount = 1;

    private float stoneSpeed = 7f;
    private float pattenrn2CountNull;

    public bool Pattern3Start {  get;private set; }
    private float golemDushSpeed = 10f;
    private float pattenrn3StartTime = 0;
    private float pattenrn3EndTime = 1.7f;
    private float pattenrn3CountNull;

    private Vector2 _targetPos;

    private void Awake()
    {
        _brain = GetComponent<GolemAIBrain>();
        _golemDieState = GetComponentInChildren<DieState>();
    }
    private void Update()
    {
        if (Pattern3Start)
        {
            if (pattenrn3StartTime >= pattenrn3EndTime)
            {
                print("시간 끝");
                Pattern3End();
            }
            else
            {
                _brain.GolemRigidbody.velocity = new Vector2(_targetPos.x * golemDushSpeed, _targetPos.y * golemDushSpeed);
                pattenrn3StartTime += Time.deltaTime;
                SkillUse = true;
                _runDecisions.Run = false;
            }
            if(_brain.GetDeshPlayerHit())
            {
                Debug.Log("플레이어 펑");
                _brain.p_transform.GetComponent<PlayerHealth>().ApplyDamage(1);
                Pattern3End();
            }
        }
    }
    public void ChoicePattern()
    {
        int rnd = Random.Range(1,4);
        while (true)
        {

            if(pattenrn2CountNull >= 3)
            {
                rnd = 2;
                break;
            }
            else if(pattenrn3CountNull >= 5)
            {
                rnd = 3;
                break;
            }
            else if(_golemDieState.GolemStemOneCheck)
            {
                rnd = 4;
                break;
            }
            else if (rnd == 1 && stoneCount == 1)
            {
                pattenrn2CountNull++;
                pattenrn3CountNull++;
                break;
            }
            else if (rnd != 1)
            {
                pattenrn2CountNull++;
                pattenrn3CountNull++;
                break;
            }
            rnd = Random.Range(1, 4);
        }
        switch (rnd)
        {
            case 1:
                _brain.GolemAnimation.GolemAniChoice(GolemAnimationName.skill1);
                break;
            case 2:
                _brain.GolemAnimation.GolemAniChoice(GolemAnimationName.skill2);
                break;
            case 3:
                _brain.GolemAnimation.GolemAniChoice(GolemAnimationName.skill3Start);
                break;
            case 4:
                _brain.GolemAnimation.GolemAniChoice(GolemAnimationName.kill);
                break;
        }
    }

    public void Pattern1()
    {
        GrooundStoone groundStone = PoolManager.Instance.Pop(ObjectPooling.PoolingType.GrooundStoone) as GrooundStoone;
        _brain.OnSmallStoneEvent.Invoke();
        groundStone.transform.position = _brain.p_transform.position;
        StartCoroutine(Pattern1Time(groundStone));
        _runDecisions.Run = true;
        SkillUse = false;
        return;
       
    }

    private IEnumerator Pattern1Time(GrooundStoone groundStone)
    {
        yield return new WaitForSeconds(groundStoneCreateTime);
        if (stoneCount == stoneNumber)
        {
            stoneCount = 0;
        }
        else Pattern1();
        groundStone.HitCheck();
        stoneCount++;
        yield return new WaitForSeconds(0.5f);

        PoolManager.Instance.Push(groundStone);
    }

    public void Pattern2()
    {
        _brain.OnSwooshEvent.Invoke();
        Stone stone = PoolManager.Instance.Pop(ObjectPooling.PoolingType.Stone) as Stone;
        stone.transform.position = _brain.transform.position;
        stone.MovingStone(_brain.p_transform,this, stoneSpeed);
        pattenrn2CountNull = 0;
        _runDecisions.Run = true;
        SkillUse = false;
        return;
    }

    public void Pattern3()
    {
        _targetPos = new Vector2(_brain.p_transform.position.x - transform.position.x, _brain.p_transform.position.y - transform.position.y).normalized;
        pattenrn3CountNull = 0;
        Pattern3Start = true;
        _brain.OnDeshStartEvent.Invoke();
        return;
    }

    public void Kill()
    {
        HammerStone hammer = Instantiate(hammerStone).GetComponent<HammerStone>();
        hammer.PlayerPoint = _brain.p_transform;
    }

    private void Pattern3End()
    {
        _brain.GolemRigidbody.velocity = Vector2.zero;
        _brain.GolemAnimation.GolemAniSetting("Run", true);
        _brain.GolemAnimation.GolemAniSetting("Skill3Move", false);
        pattenrn3StartTime = 0;
        Pattern3Start = false;
        _runDecisions.Run = true;
        SkillUse = false;
    }

    public void Page2()
    {
        groundStoneCreateTime = 0.25f;
        stoneSpeed = 8.5f;
        golemDushSpeed = 12f;
        for (int i = 0; i < _builtStones.Count; i++)
        {
            PoolManager.Instance.Push(_builtStones[i]);
            _builtStones.Remove(_builtStones[i]);
        }
    }

    public void BuiltStonesPush(BuiltStone builtStone)
    {
        _builtStones.Add(builtStone);
        if(_builtStones.Count >= 4)
        {
            _builtStones[0].CountOutBuiltStone();
            _builtStones.Remove(_builtStones[0]);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (Pattern3Start)
        {
            print("물체의 닿음");
            Pattern3End();
        }
    }

}
