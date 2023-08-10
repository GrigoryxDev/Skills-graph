using System.Collections.Generic;
using UnityEngine;

public class SkillDynamicServiceModel
{
    public readonly Dictionary<int, SkillDynamicData> dynamicSkills = new();

    public bool TryGetSkillData(int skillId, out SkillDynamicData skill)
    {
        bool isExists = dynamicSkills.TryGetValue(skillId, out skill);
        if (!isExists)
        {
            Debug.LogError($"Try get not exists dynamic skill {skillId}");
        }
        return isExists;
    }

}