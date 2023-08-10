using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillsStaticService : ISkillsStaticService
{
    private Dictionary<int, SkillStaticData> skillsDict;
    private SkillStaticData[] skills;
    private HashSet<int> learnedOnStart;

    public SkillsStaticService()
    {
        //Could be loaded from db or ets
        var staticModel = StaticDB.GetInitSkillsModel();
        UpdateSkillsData(staticModel);
    }

    public void UpdateSkillsData(SkillsInitModel skillsInitModel)
    {
        skillsDict = skillsInitModel.skillsDict;
        skills = skillsInitModel.skills;
        learnedOnStart = skillsInitModel.learnedOnStart;

        UpdateLinks();
    }

    private void UpdateLinks()
    {
        foreach (var skill in skills)
        {
            foreach (var linkedId in skill.Linked)
            {
                if (TryGetStaticSkill(linkedId, out var linkedSkill))
                {
                    var linkedWithSkill = new HashSet<int>
                    {
                        skill.ID
                    };
                    foreach (var linked in linkedSkill.Linked)
                    {
                        if (!linkedWithSkill.Contains(linked))
                        {
                            linkedWithSkill.Add(linked);
                        }
                    }

                    linkedSkill.Linked = linkedWithSkill.ToArray();
                }
            }
        }
    }

    public SkillStaticData[] GetAllSkills() => skills;
    public HashSet<int> GetLearnedOnStart() => new HashSet<int>(learnedOnStart);

    public bool TryGetStaticSkill(int id, out SkillStaticData skill)
    {
        bool isExists = skillsDict.TryGetValue(id, out skill);
        if (!isExists)
        {
            Debug.LogError($"Try get not exists static skill {id}");
        }
        return isExists;
    }
}