using System.Collections.Generic;

public interface ISkillsStaticService
{
    SkillStaticData[] GetAllSkills();
    HashSet<int> GetLearnedOnStart();
    bool TryGetStaticSkill(int id, out SkillStaticData skill);
}