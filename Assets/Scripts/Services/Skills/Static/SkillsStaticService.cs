using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillsStaticService : ISkillsStaticService
{
    private readonly Dictionary<int, SkillStaticData> skillsDict;
    private readonly SkillStaticData[] skills;
    private readonly HashSet<int> learnedOnStart;

    public SkillsStaticService()
    {
        //Could be loaded from db or ets
        skillsDict = new()
        {
            {SkillConstants.BASE_START_ID,new SkillStaticData{ID = SkillConstants.BASE_START_ID, Price = 0, Linked = new int[] { 1,2,4,8,9 } }},
            {1,new SkillStaticData{ID = 1, Price = 1, Linked = new int[] {}}},
            {2,new SkillStaticData{ID = 2, Price = 1, Linked = new int[] {3}}},
            {3,new SkillStaticData{ID = 3, Price = 1, Linked = new int[] {}}},
            {4,new SkillStaticData{ID = 4, Price = 1, Linked = new int[] {5,6}}},
            {5,new SkillStaticData{ID = 5, Price = 1, Linked = new int[] {7}}},
            {6,new SkillStaticData{ID = 6, Price = 1, Linked = new int[] {7}}},
            {7,new SkillStaticData{ID = 7, Price = 2, Linked = new int[] {}}},
            {8,new SkillStaticData{ID = 8, Price = 1, Linked = new int[] {10}}},
            {9,new SkillStaticData{ID = 9, Price = 1, Linked = new int[] {10}}},
            {10,new SkillStaticData{ID = 10, Price = 3, Linked = new int[] {}}},
        };

        skills = skillsDict.Values.ToArray();

        learnedOnStart = new HashSet<int> { SkillConstants.BASE_START_ID };
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
                    var linkedWithSkill = new List<int>
                    {
                        skill.ID
                    };
                    linkedWithSkill.AddRange(linkedSkill.Linked);
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