using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class GolemAnimation : MonoBehaviour
{
    private Animator _golemAni;
    private GolemSkill _skill;

    private void Awake()
    {
        _golemAni = GetComponent<Animator>();
        _skill = GetComponentInParent<GolemSkill>();
    }

    public void GolemAniChoice(GolemAnimationName golemAnimationName)
    {
        GolemAniAllStop();
        switch(golemAnimationName)
        {
            case GolemAnimationName.skill1: GolemAniPlay("Skill1" ,1); break;
            case GolemAnimationName.skill2:/*여기서 Sweep 사운드 출력*/ GolemAniPlay("Skill2", 2); break;
            case GolemAnimationName.skill3Start: GolemAniPlay("Skill3Start", 3); break;
            case GolemAnimationName.kill: GolemAniPlay("Kill",4); break;
            case GolemAnimationName.stem: GolemAniPlay("Stem", 0); break;
            case GolemAnimationName.die: GolemAniPlay("Die", 10); break;
        }
    }

    private void GolemAniPlay(string name, int number)
    {
        GolemAniSetting("Run", false);
        GolemAniSetting(name, true);
        StartCoroutine(GolemAniPlayTime(name, number));
    }

    private IEnumerator GolemAniPlayTime(string name, int aniNumber)
    {
        if(aniNumber == 3) _skill.Pattern3();
        yield return new WaitForSeconds(0.3f);
        switch(aniNumber)
        {
            case 1: GolemAniSetting("Run",true); GolemAniSetting(name, false); _skill.Pattern1(); break;
            case 2: GolemAniSetting("Run", true); GolemAniSetting(name, false); _skill.Pattern2(); break;
            case 3: GolemAniSetting(name, false); GolemAniSetting("Skill3Move", true); break;
            case 4: GolemAniSetting("Run", true); GolemAniSetting(name, false); _skill.Kill(); break;
            case 10: yield return new WaitForSeconds(0.85f); _golemAni.enabled = false; break;
        }
    }

    public void GolemAniSetting(string name,bool setting)
    {
        _golemAni.SetBool(name,setting);
    }

    public void Flip(Rigidbody2D golemRigid)
    {
        if (golemRigid.velocity.x > 0) transform.rotation = new Quaternion(0, 180, 0,0);
        else if(golemRigid.velocity.x < 0) transform.rotation = new Quaternion(0, 0, 0, 0);
    }

    public void GolemAniAllStop()
    {
        GolemAniSetting("Skill3Start", false);
        GolemAniSetting("Skill3Move", false);
        GolemAniSetting("Skill2", false);
        GolemAniSetting("Skill1", false);
    }
}


public enum GolemAnimationName
{
    run,
    stem,
    die,
    kill,
    skill1,
    skill2,
    skill3Start,
    skill3Move,
}


