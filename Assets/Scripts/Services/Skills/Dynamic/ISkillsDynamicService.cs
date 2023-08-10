using System.Collections.Generic;

public interface ISkillsDynamicService : IInitOnStart
{
    bool CouldBeLearned(int skillId);
    bool CouldBeForget(int skillId);

    void Learn(int skillId);
    void Forget(int skillId);

    void Reset();
    Dictionary<int, SkillDynamicData> GetAllSkills();
    bool TryGetSkillData(int skillId, out SkillDynamicData skill);
}