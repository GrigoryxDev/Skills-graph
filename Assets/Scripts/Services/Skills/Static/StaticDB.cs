using System.Collections.Generic;
using System.Linq;

public static class StaticDB
{
    public static SkillsInitModel GetInitSkillsModel()
    {
        var model = new SkillsInitModel
        {
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
         },
            learnedOnStart = new HashSet<int> { SkillConstants.BASE_START_ID }
        };
        model.skills = model.skillsDict.Values.ToArray();
        return model;
    }
}

public class SkillsInitModel
{
    public Dictionary<int, SkillStaticData> skillsDict;
    public SkillStaticData[] skills;
    public HashSet<int> learnedOnStart;
}