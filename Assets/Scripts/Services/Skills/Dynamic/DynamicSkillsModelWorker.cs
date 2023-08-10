using System.Collections.Generic;

public class DynamicSkillsModelWorker
{
    private readonly ISkillsStaticService skillsStaticService;

    private readonly SkillDynamicServiceModel model;

    public DynamicSkillsModelWorker(ISkillsStaticService skillsStaticService, SkillDynamicServiceModel model)
    {
        this.skillsStaticService = skillsStaticService;
        this.model = model;
    }

    public void CreateNewDynamicDict()
    {
        model.dynamicSkills.Clear();

        CreateNewDict();
        CheckAllLinkedSkills();
    }

    private void CreateNewDict()
    {
        var allStatic = skillsStaticService.GetAllSkills();
        var learnedOnStart = skillsStaticService.GetLearnedOnStart();

        foreach (var staticData in allStatic)
        {
            var dynamic = new SkillDynamicData(staticData);

            if (learnedOnStart.Contains(staticData.ID))
            {
                dynamic.ChangeState(SkillDynamicState.LearnedImmutable);
            }

            model.dynamicSkills.Add(staticData.ID, dynamic);
        }
    }

    private void CheckAllLinkedSkills()
    {
        var currentSkills = new Queue<SkillDynamicData>();
        var visited = new HashSet<SkillDynamicData>();

        if (model.TryGetSkillData(SkillConstants.BASE_START_ID, out var startSkill))
        {
            currentSkills.Enqueue(startSkill);

            while (currentSkills.Count > 0)
            {
                var checkedSkill = currentSkills.Dequeue();
                if (visited.Contains(checkedSkill))
                {
                    continue;
                }

                visited.Add(checkedSkill);

                foreach (var item in checkedSkill.GetStaticData.Linked)
                {
                    if (model.TryGetSkillData(item, out var linkedSkill))
                    {
                        if (!visited.Contains(linkedSkill))
                        {
                            currentSkills.Enqueue(linkedSkill);
                        }

                        if (checkedSkill.IsLearned())
                        {
                            if (linkedSkill.State == SkillDynamicState.NotLearned)
                            {
                                linkedSkill.ChangeState(SkillDynamicState.CouldBeLearn);
                            }
                        }
                    }
                }
            }
        }
    }
}