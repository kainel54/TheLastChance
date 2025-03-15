using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public class SkillManager : MonoSingleton<SkillManager>
{
    private List<Skill> _skills = new List<Skill>();
    private Dictionary<Type, SkillDataSO> _dataPairs = new Dictionary<Type, SkillDataSO>();

    public void Initialize(Magician magician)
    {
        magician.GetComponents<Skill>().ToList().ForEach(skill =>
        {
            _skills.Add(skill);
            skill.Initialize(magician);

            Type skillType = skill.GetType();

            _dataPairs.Add(skillType, skill.SkillData);

            FieldInfo field = skillType.BaseType.GetField("_skillAnimator", BindingFlags.NonPublic | BindingFlags.Instance);
            field.SetValue(skill, skill.SkillData.animatorController);
        });
    }

    public bool ActiveSkill<T>(bool active) where T : Skill
    {
        bool isActive = false;

        foreach (Skill skill in _skills)
        {
            if (skill as T is null) continue;

            if (active) skill.Play<T>();
            else skill.Stop<T>();

            isActive = true;
        }

        return isActive;
    }

    public SkillDataSO GetData<T>() where T : Skill => _dataPairs.GetValueOrDefault(typeof(T));
}
