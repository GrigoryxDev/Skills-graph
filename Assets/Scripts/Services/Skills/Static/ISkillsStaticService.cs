using System.Collections.Generic;

public interface ISkillsStaticService
{
    void UpdateSkillsData(SkillsInitModel skillsInitModel);

    SkillStaticData[] GetAllSkills();
    HashSet<int> GetLearnedOnStart();
    bool TryGetStaticSkill(int id, out SkillStaticData skill);
}