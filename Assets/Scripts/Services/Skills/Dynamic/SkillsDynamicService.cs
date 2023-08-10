﻿using System.Collections.Generic;

public class SkillsDynamicService : ISkillsDynamicService
{
    private readonly ISpendEarnService spendEarnService;
    private readonly UserSkillsModel userSkillsModel;

    private readonly DynamicSkillsModelWorker modelWorker;
    private readonly SkillDynamicServiceModel skillsModel = new();


    public SkillsDynamicService(ISkillsStaticService skillsStaticService,
    IUserStorageService userStorageService,
    ISpendEarnService spendEarnService)
    {

        this.spendEarnService = spendEarnService;

        userSkillsModel = userStorageService.GetSkillsData();

        modelWorker = new DynamicSkillsModelWorker(skillsStaticService, skillsModel);
    }

    public void Init()
    {
        Reset();
    }

    public void Reset()
    {
        modelWorker.CreateNewDynamicDict();
    }

    public Dictionary<int, SkillDynamicData> GetAllSkills() => skillsModel.dynamicSkills;
    public bool TryGetSkillData(int skillId, out SkillDynamicData skill) => skillsModel.TryGetSkillData(skillId, out skill);


    public bool CouldBeLearned(int skillId)
    {
        if (TryGetSkillData(skillId, out var skillDynamic))
        {
            bool isCouldSpend = spendEarnService.IsCouldSpend(ItemTypes.SkillPoint, skillDynamic.GetStaticData.Price);
            return skillDynamic.State == SkillDynamicState.CouldBeLearn && isCouldSpend;
        }
        return false;
    }

    public bool CouldBeForget(int skillId)
    {
        if (TryGetSkillData(skillId, out var forgetSkill))
        {
            if (forgetSkill.State == SkillDynamicState.Learned)
            {
                if (forgetSkill.GetStaticData.Linked.Length == 1)
                {
                    return true;
                }
                else
                {
                    forgetSkill.ChangeState(SkillDynamicState.NotLearned);
                    
                    //Check that linked learned skills connect with any base 
                    foreach (var forgetLinkedId in forgetSkill.GetStaticData.Linked)
                    {
                        if (TryGetSkillData(forgetLinkedId, out SkillDynamicData forgetLinkedSkill))
                        {
                            if (forgetLinkedId != forgetSkill.GetStaticData.ID && forgetLinkedSkill.IsLearned())
                            {
                                if (!IsLearnedElementLinkedWithBase(forgetLinkedSkill))
                                {
                                    forgetSkill.ChangeState(SkillDynamicState.Learned);
                                    return false;
                                }
                            }
                        }
                    }

                    forgetSkill.ChangeState(SkillDynamicState.Learned);
                    return true;
                }
            }
        }

        return false;
    }

    private bool IsLearnedElementLinkedWithBase(SkillDynamicData forgetLinkedSkill)
    {
        var currentSkills = new Queue<SkillDynamicData>();
        var visited = new HashSet<SkillDynamicData>();

        currentSkills.Enqueue(forgetLinkedSkill);

        while (currentSkills.Count > 0)
        {
            var checkedSkill = currentSkills.Dequeue();
            if (checkedSkill.State == SkillDynamicState.LearnedImmutable)
            {
                return true;
            }

            visited.Add(checkedSkill);

            foreach (var item in checkedSkill.GetStaticData.Linked)
            {
                if (TryGetSkillData(item, out var linkedSkill))
                {
                    if (checkedSkill.IsLearned() && !visited.Contains(linkedSkill))
                    {
                        currentSkills.Enqueue(linkedSkill);
                    }
                }
            }
        }

        return false;
    }

    public void Forget(int skillId)
    {
        //TODO: test forget for 2 linked
        if (TryGetSkillData(skillId, out var forgetSkillData))
        {
            userSkillsModel.LearnedSkills.Remove(skillId);
            spendEarnService.Earn(ItemTypes.SkillPoint, forgetSkillData.GetStaticData.Price);

            forgetSkillData.ChangeState(SkillDynamicState.CouldBeLearn);

            foreach (var item in forgetSkillData.GetStaticData.Linked)
            {
                if (TryGetSkillData(item, out var linkedSkill))
                {
                    if (linkedSkill.State == SkillDynamicState.CouldBeLearn &&
                    !IsAnyLearnedLinked(linkedSkill, out _))
                    {
                        linkedSkill.ChangeState(SkillDynamicState.NotLearned);
                    }
                }
            }
        }
    }

    private bool IsAnyLearnedLinked(SkillDynamicData skill, out SkillDynamicData firstLearned)
    {
        foreach (var item in skill.GetStaticData.Linked)
        {
            if (TryGetSkillData(item, out var linkedSkill))
            {
                if (linkedSkill.IsLearned())
                {
                    firstLearned = linkedSkill;
                    return true;
                }
            }
        }

        firstLearned = null;
        return false;
    }

    public void Learn(int skillId)
    {
        if (TryGetSkillData(skillId, out var learnSkillData))
        {
            //TODO: test learned skill aded to storage
            userSkillsModel.LearnedSkills.Add(skillId);
            spendEarnService.Spend(ItemTypes.SkillPoint, learnSkillData.GetStaticData.Price);

            learnSkillData.ChangeState(SkillDynamicState.Learned);

            foreach (var item in learnSkillData.GetStaticData.Linked)
            {
                if (TryGetSkillData(item, out var linkedSkill))
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