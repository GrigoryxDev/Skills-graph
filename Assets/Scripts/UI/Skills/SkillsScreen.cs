using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class SkillsScreen : MonoBehaviour
{
    private IResetService resetService;
    private ISkillsDynamicService skillsDynamicService;

    [SerializeField] private Button resetAll;

    [SerializeField] private BaseSkillScreenElement[] skillScreenPart;

    private readonly SkillsScreenModel model = new();

    [Inject]
    private void Constructor(IResetService resetService,
    ISkillsDynamicService skillsDynamicService)
    {
        this.resetService = resetService;
        this.skillsDynamicService = skillsDynamicService;
    }

    public void Init()
    {
        PrepareModel();
        InitButtons();

        foreach (var element in skillScreenPart)
        {
            element.Init(model);
        }
    }

    private void PrepareModel()
    {
        model.OnSelect += OnSelect;
        model.OnDeselect += OnDeSelect;

        model.OnLearn += OnLearn;
        model.OnForget += OnForget;

        model.OnUpdateSkillPoints += OnUpdateSkillPoints;
    }

    private void OnUpdateSkillPoints()
    {
        foreach (var element in skillScreenPart)
        {
            element.OnUpdateSkillPoints();
        }
    }

    private void InitButtons()
    {
        resetAll.onClick.AddListener(OnReset);
    }

    private void OnReset()
    {
        resetService.Reset();

        foreach (var element in skillScreenPart)
        {
            element.OnReset();
        }
    }

    private void OnLearn(SkillUiElement skillUiElement)
    {
        skillsDynamicService.Learn(model.Selected.SkillId);

        foreach (var element in skillScreenPart)
        {
            element.OnLearn(skillUiElement);
        }
    }

    private void OnForget(SkillUiElement skillUiElement)
    {
        skillsDynamicService.Forget(model.Selected.SkillId);
        foreach (var element in skillScreenPart)
        {
            element.OnForget(skillUiElement);
        }
    }

    private void OnSelect(SkillUiElement skillUiElement)
    {
        foreach (var element in skillScreenPart)
        {
            element.OnSelect(skillUiElement);
        }
    }

    private void OnDeSelect()
    {
        foreach (var element in skillScreenPart)
        {
            element.OnDeSelect();
        }
    }
}
