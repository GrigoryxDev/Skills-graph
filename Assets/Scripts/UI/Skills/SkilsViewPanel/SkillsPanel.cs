﻿using System;
using UnityEngine;
using Zenject;

public class SkillsPanel : BaseSkillScreenElement, ISkillScreenLearnUpdate
{
    private ISkillsDynamicService skillsDynamicService;

    [SerializeField, Header("Index equils id")] private SkillUiElement[] spawnedElements;
    [SerializeField, Header("0-learned, 1-not")] private Color[] colors;

    [Inject]
    private void Constructor(ISkillsDynamicService skillsDynamicService)
    {
        this.skillsDynamicService = skillsDynamicService;

    }

    private void Start()
    {
        foreach (var spawnedElement in spawnedElements)
        {
            spawnedElement.OnClick += () =>
            {
                if (model.Selected == spawnedElement)
                {
                    model.Selected.ShowSelect(false);
                    model.OnDeselect?.Invoke();
                    model.SetSkillUiElement(null);
                }
                else
                {
                    model.Selected?.ShowSelect(false);
                    model.SetSkillUiElement(spawnedElement);
                    model.Selected.ShowSelect(true);
                    model.OnSelect(spawnedElement);
                }
            };
        }
    }

    public override void OnInit(SkillsScreenModel model)
    {
        UpdateAllSkillUis();
    }

    private void UpdateAllSkillUis()
    {
        var allSkills = skillsDynamicService.GetAllSkills();

        for (int i = 0; i < allSkills.Values.Count; i++)
        {
            var skill = allSkills[i];
            var spawnedElement = spawnedElements[i];

            Color skillUiState = GetSkillColor(skill);

            string label = skill.GetStaticData.ID == SkillConstants.BASE_START_ID ? "Base" : $"Skill{skill.GetStaticData.ID}";
            spawnedElement.Init(skill.GetStaticData.ID, skillUiState, label);
        }
    }

    private Color GetSkillColor(SkillDynamicData skill)
    {
        return (skill.State is SkillDynamicState.Learned or
        SkillDynamicState.LearnedImmutable) ? colors[0] : colors[1];
    }

    public override void OnReset()
    {
        model.SetSkillUiElement(null);
        UpdateAllSkillUis();
    }

    public void OnLearn(SkillUiElement skillUiElement)
    {
        TryUpdateColor(skillUiElement);
    }

    public void OnForget(SkillUiElement skillUiElement)
    {
        TryUpdateColor(skillUiElement);
    }

    private void TryUpdateColor(SkillUiElement skillUiElement)
    {
        if (skillsDynamicService.TryGetSkillData(skillUiElement.SkillId, out var skill))
        {
            Color skillUiState = GetSkillColor(skill);
            skillUiElement.SetSelectedColor(skillUiState);
        }
    }
}
